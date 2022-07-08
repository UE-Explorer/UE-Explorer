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

        public static string ExportPackageClasses(this UnrealPackage package, bool exportScripts = false)
        {
            Program.LoadConfig();
            string exportPath = package.InitializeExportDirectory();
            package.NTLPackage = new NativesTablePackage();
            package.NTLPackage.LoadPackage(Path.Combine(Application.StartupPath, "Native Tables",
                Program.Options.NTLPath));
            foreach (UClass uClass in package.Objects.Where(o => o is UClass && o.ExportTable != null))
            {
                try
                {
                    string exportContent = exportScripts && uClass.ScriptText != null
                        ? uClass.ScriptText.Decompile()
                        : uClass.Decompile();

                    File.WriteAllText(
                        Path.Combine(exportPath, uClass.Name) + UnrealExtensions.UnrealCodeExt,
                        exportContent,
                        Encoding.ASCII
                    );
                }
                catch (Exception e)
                {
                    Console.WriteLine("Couldn't decompile object " + uClass + "\r\n" + e);
                }
            }

            package.CreateUPKGFile(exportPath);
            return exportPath;
        }
    }
}