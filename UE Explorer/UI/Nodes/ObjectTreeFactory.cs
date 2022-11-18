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
        private static readonly ObjectImageKeySelector s_objectImageKeySelector = new ObjectImageKeySelector();

        [CanBeNull]
        public static TreeNode CreateNode(FieldInfo info, object obj)
        {
            object value = info.GetValue(obj);
            if (value == null) return null;

            var attr = info.GetCustomAttribute<DisplayNameAttribute>();
            var node = new TreeNode(attr != null ? attr.DisplayName : info.Name) { Tag = value };
            return node;
        }

        [CanBeNull]
        public static TreeNode CreateNode(PropertyInfo info, object obj)
        {
            object value = info.GetValue(obj);
            if (value == null) return null;

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
            var node = new TreeNode(GetTreeNodeText(obj))
            {
                Tag = obj, ImageKey = imageKey, SelectedImageKey = imageKey
            };

            if ((int)obj > 0)
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            if (obj.Class == null)
            {
                node.ForeColor = Color.DarkCyan;
            }

            if (obj.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
            {
                node.ForeColor = Color.Red;
            }

            return node;
        }

        public static TreeNode CreateNode(UImportTableItem item)
        {
            string imageKey = item.Object.Accept(s_objectImageKeySelector);
            var node = new TreeNode(GetTreeNodeText(item))
            {
                Tag = item, ImageKey = imageKey, SelectedImageKey = imageKey
            };

            if (item.Owner.Imports.Any(imp => ObjectTreeBuilder.BelongsWithinItem(imp, item)))
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            if (item.ClassName == "Class")
            {
                node.ForeColor = Color.DarkCyan;
            }

            return node;
        }

        public static TreeNode CreateNode(UExportTableItem item)
        {
            string imageKey = item.Object.Accept(s_objectImageKeySelector);
            var node = new TreeNode(GetTreeNodeText(item))
            {
                Tag = item, ImageKey = imageKey, SelectedImageKey = imageKey,
            };

            if (item.Owner.Exports.Any(exp => ObjectTreeBuilder.BelongsWithinItem(exp, item)))
            {
                node.Nodes.Add(DummyNodeKey, "Expandable");
            }

            if (item.Object.DeserializationState.HasFlag(UObject.ObjectState.Errorlized))
            {
                node.ForeColor = Color.Red;
            }

            if (item.Class == null)
            {
                node.ForeColor = Color.DarkCyan;
            }

            return node;
        }

        public static TreeNode CreateNode(PackageReference packageReference)
        {
            string imageKey = s_objectImageKeySelector.Visit(packageReference);
            string name = Path.GetFileNameWithoutExtension(packageReference.FilePath);
            var node = new TreeNode(name)
            {
                Name = packageReference.FilePath, ImageKey = imageKey, SelectedImageKey = imageKey, Tag = packageReference,
            };
            return node;
        }

        public static TreeNode CreateNode([NotNull] UnrealPackage linker)
        {
            string imageKey = s_objectImageKeySelector.Visit(linker);
            // TODO: Displace in UELib 2.0 using an ObjectNode referring the UnrealPackage.RootPackage object.
            var node = new TreeNode(linker.PackageName)
            {
                Name = linker.PackageName, ImageKey = imageKey, SelectedImageKey = imageKey, Tag = linker,
            };
            return node;
        }

        public static string GetTreeNodeToolTipText(TreeNode node)
        {
            switch (node.Tag)
            {
                case UnrealPackage linker:
                    return GetTreeNodeToolTipText(linker);

                case UObjectTableItem item:
                    return GetTreeNodeToolTipText(item);
            }

            return string.Empty;
        }

        public static string GetTreeNodeToolTipText(UObjectTableItem item)
        {
            return $"Path: {item.Object?.GetReferencePath()}\r\n" +
                   $"Index: {(int)item}";
        }

        public static string GetTreeNodeToolTipText(UnrealPackage linker)
        {
            return $"Path: {linker.FullPackageName}\r\n" +
                   $"Version: {linker.Version}/{linker.LicenseeVersion.ToString().PadLeft(3, '0')}\r\n" +
                   $"Build: {linker.Build}";
        }

        public static string GetTreeNodeText(UObject obj)
        {
            return ObjectPathBuilder.GetPath(obj);
        }

        public static string GetTreeNodeText(UObjectTableItem item)
        {
            var fullName = string.Empty;
            for (var outer = item.Outer; outer != null; outer = outer.Outer)
            {
                fullName = $"{outer.ObjectName}.{fullName}";
            }

            return fullName + item.ObjectName;
        }

        public static string GetTreeNodeText(UImportTableItem item)
        {
            return $"{item.ObjectName}: {item.ClassName}";
        }

        public static string GetTreeNodeText(UExportTableItem item)
        {
            return item.ObjectName;
        }

        public const string DummyNodeKey = "DUMMYNODE";
    }
}
