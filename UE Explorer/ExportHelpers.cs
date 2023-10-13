using System;
using System.IO;
using System.Linq;
using System.Text;
using UELib;
using UELib.Core;
using UELib.Flags;

namespace UEExplorer
{
    internal static class ExportHelpers
    {
        private static void CreateUPKGFile(this UnrealPackage package, string path)
        {
            string[] content =
            {
                "[Flags]",
                "AllowDownload=" + package.Summary.PackageFlags.HasFlag(PackageFlags.AllowDownload),
                "ClientOptional=" + package.Summary.PackageFlags.HasFlag(PackageFlags.ClientOptional),
                "ServerSideOnly=" + package.Summary.PackageFlags.HasFlag(PackageFlags.ServerSideOnly)
            };


            string filePath = Path.Combine(path, package.PackageName + UnrealExtensions.UnrealFlagsExt);
            File.WriteAllLines(filePath, content);
        }

        public static void ExportPackageObjects<T>(this UnrealPackage package, string path)
            where T : UObject
        {
            Directory.CreateDirectory(Path.Combine(path, package.PackageName, "Classes"));

            string classesPath = Path.Combine(path, package.PackageName, "Classes");
            foreach (var obj in package.Objects
                         .Where(o => o.ExportTable != null)
                         .OfType<T>())
            {
                string content;
                try
                {
                    content = obj.Decompile();
                }
                catch (Exception e)
                {
                    content = $"/* Couldn't decompile object {obj}\r\n{e} */";
                    Console.Error.WriteLine($"Couldn't decompile object {obj}\r\n{e}");
                }

                File.WriteAllText(
                    Path.Combine(classesPath, obj.Name) + UnrealExtensions.UnrealCodeExt,
                    content,
                    Encoding.ASCII
                );
            }

            package.CreateUPKGFile(classesPath);
        }
    }
}
