using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using UEExplorer.Framework;
using UELib;
using UELib.Annotations;
using UELib.Core;

namespace UEExplorer.UI.Nodes
{
    public static class ObjectTreeFactory
    {
        public const string DummyNodeKey = "DUMMYNODE";
        public static Color ErrorColor = Color.Red;
        public static Color UnknownClassColor = Color.SlateGray;
        public static readonly Color PackageLoadedColor = Color.Black;
        public static readonly Color PackageUnloadedColor = Color.SlateGray;

        private static readonly ObjectImageKeySelector s_objectImageKeySelector = new ObjectImageKeySelector();

        [CanBeNull]
        public static TreeNode CreateNode(FieldInfo info, object obj)
        {
            object value = info.GetValue(obj);
            if (value == null)
            {
                return null;
            }

            var attr = info.GetCustomAttribute<DisplayNameAttribute>();
            var node = new TreeNode(attr != null ? attr.DisplayName : info.Name) { Tag = value };
            return node;
        }

        [CanBeNull]
        public static TreeNode CreateNode(PropertyInfo info, object obj)
        {
            object value = info.GetValue(obj);
            if (value == null)
            {
                return null;
            }

            var attr = info.GetCustomAttribute<DisplayNameAttribute>();
            string displayName = attr != null ? attr.DisplayName : info.Name;

            var uObject = value as UObject;
            string text = uObject != null
                ? $"{displayName}: {ObjectPathBuilder.GetPath(uObject)}"
                : displayName;

            string imageKey = uObject != null
                ? uObject.Accept(s_objectImageKeySelector)
                : "Content";

            var node = new TreeNode(text) { Tag = value, ImageKey = imageKey, SelectedImageKey = imageKey };

            if ((int)uObject > 0)
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            return node;
        }

        public static TreeNode CreateNode(MemberInfo field)
        {
            var attr = field.GetCustomAttribute<DisplayNameAttribute>();
            var node = new TreeNode(attr != null ? attr.DisplayName : field.Name);
            return node;
        }

        public static TreeNode CreateNode(UObject obj)
        {
            string imageKey = obj.Accept(s_objectImageKeySelector);
            var node = new TreeNode(ObjectTextBuilder.GetText(obj))
            {
                Tag = obj, ImageKey = imageKey, SelectedImageKey = imageKey
            };

            if ((int)obj > 0)
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            if (obj.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
            {
                node.ForeColor = ErrorColor;
            }

            return node;
        }

        public static TreeNode CreateNode(UImportTableItem item)
        {
            string imageKey = item.Object.Accept(s_objectImageKeySelector);
            var node = new TreeNode(ObjectTextBuilder.GetText(item))
            {
                Tag = item, ImageKey = imageKey, SelectedImageKey = imageKey
            };

            if (item.Owner.Imports.Any(imp => ObjectTreeBuilder.BelongsWithinItem(imp, item)))
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            if (!item.Owner.HasClassType(item.ClassName) || (item.ClassName == "Class" && !item.Owner.HasClassType(item.ObjectName)))
            {
                node.ForeColor = UnknownClassColor;
            }

            return node;
        }

        public static TreeNode CreateNode(UExportTableItem item)
        {
            string imageKey = item.Object.Accept(s_objectImageKeySelector);
            var node = new TreeNode(ObjectTextBuilder.GetText(item))
            {
                Tag = item, ImageKey = imageKey, SelectedImageKey = imageKey
            };

            if (item.Owner.Exports.Any(exp => ObjectTreeBuilder.BelongsWithinItem(exp, item)))
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            if (item.Object.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
            {
                node.ForeColor = ErrorColor;
            }

            return node;
        }

        public static TreeNode CreateNode(PackageReference packageReference)
        {
            string imageKey = s_objectImageKeySelector.Visit(packageReference);
            string name = Path.GetFileNameWithoutExtension(packageReference.FilePath);
            var node = new TreeNode(name)
            {
                Name = packageReference.FilePath,
                ImageKey = imageKey,
                SelectedImageKey = imageKey,
                Tag = packageReference
            };
            return node;
        }

        public static TreeNode CreateNode([NotNull] UnrealPackage linker)
        {
            string imageKey = s_objectImageKeySelector.Visit(linker);
            // TODO: Displace in UELib 2.0 using an ObjectNode referring the UnrealPackage.RootPackage object.
            var node = new UnsortedTreeNode(linker.PackageName)
            {
                Name = linker.PackageName, ImageKey = imageKey, SelectedImageKey = imageKey, Tag = linker
            };
            return node;
        }

        public static TreeNode CreateNode(UArray<CompressedChunk> summaryCompressedChunks)
        {
            string imageKey = s_objectImageKeySelector.Visit(summaryCompressedChunks);
            var node = new UnsortedTreeNode("Chunks")
            {
                Name = "Chunks", Tag = summaryCompressedChunks, ImageKey = imageKey, SelectedImageKey = imageKey
            };
            node.Nodes.Add(DummyNodeKey, "Expandable");
            return node;
        }

        public static TreeNode CreateNode(List<UImportTableItem> imports)
        {
            string imageKey = s_objectImageKeySelector.Visit(imports);
            var node = new UnsortedTreeNode("Imports")
            {
                Name = "Dependencies", Tag = imports, ImageKey = imageKey, SelectedImageKey = imageKey
            };
            node.Nodes.Add(DummyNodeKey, "Expandable");
            return node;
        }
    }
}
