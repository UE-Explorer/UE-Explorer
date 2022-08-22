using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UELib;
using UELib.Core;
using UELib.Flags;

namespace UEExplorer
{
    internal static class ExportHelpers
    {
        private const string ExportedDir = "Exported";
        private const string ClassesDir = "Classes";

        public static readonly string PackageExportPath = Path.Combine(Application.StartupPath, ExportedDir);

        public static string InitializeExportDirectory(this UnrealPackage package)
        {
            string exportPath = Path.Combine(PackageExportPath, package.PackageName);
            if (Directory.Exists(exportPath))
            {
                string[] files = Directory.GetFiles(exportPath);
                foreach (string file in files) File.Delete(exportPath + file);
            }

            string classPath = Path.Combine(exportPath, ClassesDir);
            Directory.CreateDirectory(classPath);
            return classPath;
        }

        public static void CreateUPKGFile(this UnrealPackage package, string exportPath)
        {
            string[] upkgContent = {
                "[Flags]",
                "AllowDownload=" + package.Summary.PackageFlags.HasFlag(PackageFlags.AllowDownload),
                "ClientOptional=" + package.Summary.PackageFlags.HasFlag(PackageFlags.ClientOptional),
                "ServerSideOnly=" + package.Summary.PackageFlags.HasFlag(PackageFlags.ServerSideOnly)
            };

            File.WriteAllLines(
                Path.Combine(exportPath, package.PackageName) + UnrealExtensions.UnrealFlagsExt,
                upkgContent
            );
        }

        public static string ExportPackageObjects<T>(this UnrealPackage package)
        {
            Program.LoadConfig();
            string exportPath = package.InitializeExportDirectory();
            package.NTLPackage = new NativesTablePackage();
            package.NTLPackage.LoadPackage(Path.Combine(Application.StartupPath, "Native Tables",
                Program.Options.NTLPath));
            foreach (var obj in package.Objects.Where(o => o is T && o.ExportTable != null))
            {
                try
                {
                    string exportContent = obj.Decompile();

                    File.WriteAllText(
                        Path.Combine(exportPath, obj.Name) + UnrealExtensions.UnrealCodeExt,
                        exportContent,
                        Encoding.ASCII
                    );
                }
                catch (Exception e)
                {
                    Console.Error.WriteLine("Couldn't decompile object " + obj + "\r\n" + e);
                }
            }

            package.CreateUPKGFile(exportPath);
            return exportPath;
        }
    }
}