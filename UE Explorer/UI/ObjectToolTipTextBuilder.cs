using UEExplorer.Framework;
using UELib;

namespace UEExplorer.UI
{
    public static class ObjectToolTipTextBuilder
    {
        public static string GetToolTipText(object obj)
        {
            switch (obj)
            {
                case PackageReference packageReference:
                    return GetToolTipText(packageReference);

                case UObjectTableItem item:
                    return GetToolTipText(item);
            }

            return string.Empty;
        }

        public static string GetToolTipText(UObjectTableItem item)
        {
            return $"Path: {item.Object?.GetReferencePath()}\r\n" +
                   $"Index: {(int)item}";
        }

        public static string GetToolTipText(PackageReference packageReference)
        {
            switch (packageReference.Linker)
            {
                case null:
                    return $"Path: {packageReference.FilePath}\r\n";

                default:
                    return GetToolTipText(packageReference.Linker);
            }
        }

        public static string GetToolTipText(UnrealPackage linker)
        {
            return $"Path: {linker.FullPackageName}\r\n" +
                   $"Version: {linker.Version}/{linker.LicenseeVersion.ToString().PadLeft(3, '0')}\r\n" +
                   $"Platform: {linker.Build.Flags}\r\n" +
                   $"Build: {linker.Build}";
        }
    }
}
