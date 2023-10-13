using UEExplorer.Framework;
using UELib;
using UELib.Core;

namespace UEExplorer.UI
{
    public static class ObjectToolTipTextBuilder
    {
        // Safe tooltip builder for any type.
        public static string GetToolTipText(object obj)
        {
            switch (obj)
            {
                case PackageReference packageReference:
                    return GetToolTipText(packageReference);
                
                case UImportTableItem item:
                    return GetToolTipText(item);

                case UExportTableItem item:
                    return GetToolTipText(item);
                
                case UObject uObject:
                    return GetToolTipText(uObject);

                case UStruct.UByteCodeDecompiler.Token token:
                    return token.ToString();
            }

            return string.Empty;
        }
        
        public static string GetToolTipText(UObject uObject) =>
            $"Path: {uObject.GetReferencePath()}\r\n" +
            $"PackageIndex: {(int)uObject}"
            ;
        
        public static string GetToolTipText(UImportTableItem item) =>
            $"Path: {item.GetReferencePath()}\r\n" +
            $"PackageIndex: {(int)item}\r\n" +
            $"PackageName: {item.PackageName}\r\n" +
            $"ClassName: {item.ClassName}"
        ;
        
        public static string GetToolTipText(UExportTableItem item) =>
            $"Path: {item.GetReferencePath()}\r\n" +
            $"PackageIndex: {(int)item}\r\n" +
            $"Class: {UObjectTableItem.GetReferencePath(item.Class)}\r\n" +
            $"Super: {UObjectTableItem.GetReferencePath(item.Super)}\r\n" +
            $"Template: {UObjectTableItem.GetReferencePath(item.Template)}\r\n" +
            $"Archetype: {UObjectTableItem.GetReferencePath(item.Archetype)}"
        ;
        
        public static string GetToolTipText(PackageReference packageReference)
        {
            string tooltip = $"Path: {packageReference.FilePath}\r\n" +
                             $"Date: {packageReference.RegisterDate.ToShortDateString()}";
            
            if (packageReference.Error != null)
            {
                tooltip += $"\r\n\r\nError: {packageReference.Error}";
            }
            
            return packageReference.Linker != null
                ? $"{tooltip}\r\n\r\n{GetToolTipText(packageReference.Linker)}"
                : tooltip;
        }

        public static string GetToolTipText(UnrealPackage linker) =>
            "Package:\r\n" +
            $"  Version: {linker.Version}/{linker.LicenseeVersion.ToString().PadLeft(3, '0')}\r\n" +
            $"  Platform: {linker.Build.Flags}\r\n" +
            $"  Build: {linker.Build}";
    }
}
