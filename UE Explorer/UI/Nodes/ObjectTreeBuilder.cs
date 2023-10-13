using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UEExplorer.Framework;
using UELib;
using UELib.Annotations;
using UELib.Core;
using UELib.Flags;

namespace UEExplorer.UI.Nodes
{
    public sealed class UnsortedTreeNode : TreeNode
    {
        public UnsortedTreeNode(string text) : base(text) { }
    }
    
    public sealed class ObjectTreeBuilder : ObjectVisitor<IEnumerable<TreeNode>>
    {
        public delegate bool NodeFilter<in TFilter>(TFilter item);

        private readonly NodeFilter<object> _NodeFilterDelegate;

        public ObjectTreeBuilder(NodeFilter<object> nodeFilterDelegate)
        {
            _NodeFilterDelegate = nodeFilterDelegate;
        }

        [CanBeNull]
        public override IEnumerable<TreeNode> Visit(IAcceptable visitor)
        {
            var memberNodes = BuildMemberNodes(visitor);
            return memberNodes?.ToArray();
        }

        public IEnumerable<TreeNode> BuildMemberNodes(object visitable)
        {
            var subNodes = new List<TreeNode>();
            var members = visitable.GetType().GetMembers();
            foreach (var member in members)
            {
                switch (member)
                {
                    case FieldInfo field when visitable is IUnrealSerializableClass:
                        {
                            var type = field.FieldType;
                            if (type == typeof(UArray<UObject>))
                            {
                                var value = (UArray<UObject>)field.GetValue(visitable);
                                if (value == null)
                                {
                                    continue;
                                }

                                var memberNode = ObjectTreeFactory.CreateNode(field);
                                foreach (var obj in value)
                                {
                                    memberNode.Nodes.Add(ObjectTreeFactory.CreateNode(obj));
                                }

                                subNodes.Add(memberNode);
                            }
                            else if (type.IsSubclassOf(typeof(UObject)))
                            {
                                var memberNode = ObjectTreeFactory.CreateNode(field, visitable);
                                if (memberNode != null)
                                {
                                    memberNode.Nodes.Add(ObjectTreeFactory.CreateNode((UObject)memberNode.Tag));
                                    subNodes.Add(memberNode);
                                }
                            }
                            else
                            {
                                object value = field.GetValue(visitable);
                                var attr = field.GetCustomAttribute<DisplayNameAttribute>();
                                string name = attr != null ? attr.DisplayName : field.Name;
                                string text = $"{name}: {value}";
                                var memberNode = new TreeNode(text) { Tag = value };
                                memberNode.Nodes.Add(memberNode);
                                subNodes.Add(memberNode);
                            }

                            break;
                        }

                    case PropertyInfo property:
                        {
                            var type = property.PropertyType;
                            if (type == typeof(UArray<UObject>))
                            {
                                var value = (UArray<UObject>)property.GetValue(visitable);
                                if (value == null)
                                {
                                    continue;
                                }

                                var memberNode = ObjectTreeFactory.CreateNode(property);
                                foreach (var obj in value)
                                {
                                    memberNode.Nodes.Add(ObjectTreeFactory.CreateNode(obj));
                                }

                                subNodes.Add(memberNode);
                            }
                            else if (type.IsSubclassOf(typeof(UObject)))
                            {
                                var memberNode = ObjectTreeFactory.CreateNode(property, visitable);
                                if (memberNode != null)
                                {
                                    subNodes.Add(memberNode);
                                }
                            }

                            //else if (type.GetInterface(nameof(IBinaryData)) != null)
                            //{
                            //    var value = (IBinaryData)property.GetValue(visitable);
                            //    if (value == null) continue;
                            //    var attr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                            //    var memberNode = new TreeNode(attr != null ? attr.DisplayName : property.Name)
                            //    {
                            //        Tag = value
                            //    };
                            //    subNodes.Add(memberNode);
                            //}
                            //else if (type.GetInterface(nameof(IUnrealSerializableClass)) != null)
                            //{
                            //    var value = (IUnrealSerializableClass)property.GetValue(visitable);
                            //    if (value == null) continue;
                            //    var attr = property.GetCustomAttribute<System.ComponentModel.DisplayNameAttribute>();
                            //    var memberNode = new TreeNode(attr != null ? attr.DisplayName : property.Name)
                            //    {
                            //        Tag = value
                            //    };
                            //    subNodes.Add(memberNode);
                            //}
                            break;
                        }
                }
            }

            return subNodes.Any() ? subNodes : null;
        }

        public override IEnumerable<TreeNode> Visit(dynamic subject) => null;

        public IEnumerable<TreeNode> Visit(PackageReference packageReference)
        {
            var linker = packageReference.Linker;
            if (linker == null)
            {
                throw new InvalidOperationException("Cannot create package nodes without an active linker.");
            }

            return Visit(linker);
        }

        public IEnumerable<TreeNode> Visit(UnrealPackage linker)
        {
            var nodes = new List<TreeNode>();
            if (linker.Summary.CompressedChunks != null &&
                linker.Summary.CompressedChunks.Any())
            {
                var chunksNode = ObjectTreeFactory.CreateNode(linker.Summary.CompressedChunks);
                nodes.Add(chunksNode);
            }

            if (linker.Imports != null)
            {
                var dependenciesNode = ObjectTreeFactory.CreateNode(linker.Imports);
                nodes.Add(dependenciesNode);
                
                // Lazy-Expand
                //nodes.AddRange(Visit(linker.Imports));
            }

            if (linker.Exports != null)
            {
                nodes.AddRange(Visit(linker.Exports));
            }

            return nodes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsParentItem(UImportTableItem imp) =>
            imp.OuterIndex == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsParentItem(UExportTableItem exp) =>
            exp.OuterIndex == 0
            && (exp.ObjectFlags & ((ulong)ObjectFlagsHO.PropertiesObject << 32)) == 0
            // Filter out deleted exports
            && exp.ObjectName != "None";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BelongsWithinItem(UExportTableItem exp, UObjectTableItem item) =>
            exp.Outer == item || (exp.OuterIndex == 0 && exp.Class == item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool BelongsWithinItem(UImportTableItem imp, UObjectTableItem item) =>
            imp.Outer == item;

        // Commented out OrderBy because we are using the TreeNodeSortComparer instead

        public IEnumerable<TreeNode> Visit(IEnumerable<UImportTableItem> imports) =>
            imports
                .Where(IsParentItem)
                .SkipWhile(exp => _NodeFilterDelegate(exp))
                .Select(ObjectTreeFactory.CreateNode);

        public IEnumerable<TreeNode> Visit(IEnumerable<UExportTableItem> exports) =>
            exports
                .Where(IsParentItem)
                .SkipWhile(exp => _NodeFilterDelegate(exp))
                .Select(ObjectTreeFactory.CreateNode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<TreeNode> Visit(UImportTableItem item) =>
            item.Owner.Imports?
                .Where(imp => BelongsWithinItem(imp, item))
                .SkipWhile(imp => _NodeFilterDelegate(imp))
                .Select(ObjectTreeFactory.CreateNode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IEnumerable<TreeNode> Visit(UExportTableItem item) =>
            item.Owner.Exports?
                .Where(exp => BelongsWithinItem(exp, item))
                .SkipWhile(exp => _NodeFilterDelegate(exp))
                .Select(ObjectTreeFactory.CreateNode);
    }
}
