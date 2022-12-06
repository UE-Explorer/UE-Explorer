using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using UELib;
using UELib.Annotations;
using UELib.Core;
using UELib.Flags;

namespace UEExplorer.UI.Nodes
{
    public sealed class ObjectTreeBuilder : ObjectVisitor<IEnumerable<TreeNode>>
    {
        public delegate bool NodeFilter<in TFilter>(TFilter item);

        public delegate IComparable NodeSorter<in T>(T item);

        private readonly NodeFilter<object> _NodeFilterDelegate;
        private readonly NodeSorter<object> _NodeSorterDelegate;

        public ObjectTreeBuilder(NodeFilter<object> nodeFilterDelegate, NodeSorter<object> nodeSorterDelegate)
        {
            _NodeFilterDelegate = nodeFilterDelegate;
            _NodeSorterDelegate = nodeSorterDelegate;
        }

        [CanBeNull]
        public override IEnumerable<TreeNode> Visit(IAcceptable visitor)
        {
            var memberNodes = BuildMemberNodes(visitor);
            return memberNodes?.ToArray();
            //var visitorNodes = base.Visit(visitor);
            //if (memberNodes == null)
            //{
            //    return visitorNodes;
            //}

            //return visitorNodes != null
            //    ? visitorNodes.Concat(memberNodes).ToArray()
            //    : memberNodes.ToArray();
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

        public override IEnumerable<TreeNode> Visit(dynamic obj) => null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private bool IsTopItem(UExportTableItem exp) =>
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

        [CanBeNull] public IEnumerable<TreeNode> Visit(UnrealPackage linker) =>
            linker.Exports?
                .Where(IsTopItem)
                .SkipWhile(exp => _NodeFilterDelegate(exp))
                .OrderBy(exp => _NodeSorterDelegate(exp))
                .Select(ObjectTreeFactory.CreateNode);

        public IEnumerable<TreeNode> Visit(UObjectTableItem item) =>
            item.Owner.Exports
                .Where(exp => BelongsWithinItem(exp, item))
                .SkipWhile(exp => _NodeFilterDelegate(exp))
                .OrderBy(exp => _NodeSorterDelegate(exp))
                .Select(ObjectTreeFactory.CreateNode);

        public IEnumerable<TreeNode> Visit(UImportTableItem item) =>
            item.Owner.Imports?
                .Where(imp => BelongsWithinItem(imp, item))
                .SkipWhile(imp => _NodeFilterDelegate(imp))
                .OrderBy(imp => _NodeSorterDelegate(imp))
                .Select(ObjectTreeFactory.CreateNode);

        public IEnumerable<TreeNode> Visit(UExportTableItem item) =>
            item.Owner.Exports?
                .Where(exp => BelongsWithinItem(exp, item))
                .SkipWhile(exp => _NodeFilterDelegate(exp))
                .OrderBy(exp => _NodeSorterDelegate(exp))
                .Select(ObjectTreeFactory.CreateNode);
    }
}
