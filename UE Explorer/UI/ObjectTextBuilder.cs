using UEExplorer.Framework;
using UELib;
using UELib.Core;

namespace UEExplorer.UI
{
    public static class ObjectTextBuilder
    {
        public static string GetText(UObject obj)
        {
            return ObjectPathBuilder.GetPath(obj);
        }

        public static string GetText(UObjectTableItem item)
        {
            var fullName = string.Empty;
            for (var outer = item.Outer; outer != null; outer = outer.Outer)
            {
                fullName = $"{outer.ObjectName}.{fullName}";
            }

            return fullName + item.ObjectName;
        }

        public static string GetText(UImportTableItem item)
        {
            return $"{item.ObjectName}: {item.ClassName}";
        }

        public static string GetText(UExportTableItem item)
        {
            return item.ObjectName;
        }
    }
}
