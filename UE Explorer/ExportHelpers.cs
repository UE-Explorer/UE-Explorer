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

		public static readonly string PackageExportPath = Path.Combine( Application.StartupPath, ExportedDir );

		public static string InitializeExportDirectory( this UnrealPackage package )
		{
			var exportPath = Path.Combine( PackageExportPath, package.PackageName );
			if( Directory.Exists( exportPath ) )
			{
				var files = Directory.GetFiles( exportPath );
				foreach( var file in files )
				{
					File.Delete( exportPath + file );
				}			
			}
			var classPath = Path.Combine( exportPath, ClassesDir );
			Directory.CreateDirectory( classPath );
			return classPath;
		}

		public static void CreateUPKGFile( this UnrealPackage package, string exportPath )
		{
			var upkgContent = new[]
			{
				"[Flags]",
				"AllowDownload=" + package.HasPackageFlag( PackageFlags.AllowDownload ),
				"ClientOptional=" + package.HasPackageFlag( PackageFlags.ClientOptional ),
				"ServerSideOnly=" + package.HasPackageFlag( PackageFlags.ServerSideOnly )
			};

			File.WriteAllLines( 
				Path.Combine( exportPath, package.PackageName ) + UnrealExtensions.UnrealFlagsExt, 
				upkgContent 
			);
		}

		public static string ExportPackageClasses( this UnrealPackage package, bool exportScripts = false )
		{
			var exportPath = package.InitializeExportDirectory();
			package.NTLPackage = new NativesTablePackage();
			package.NTLPackage.LoadPackage( Path.Combine( Application.StartupPath, "Native Tables", Program.Options.NTLPath ) ); 
			foreach( UClass uClass in package.Objects.Where( o => o is UClass && o.ExportTable != null ) )
			{
				var exportContent = exportScripts && uClass.ScriptText != null 
					? uClass.ScriptText.Decompile() 
					: uClass.Decompile();

				File.WriteAllText( 
					Path.Combine( exportPath, uClass.Name ) + UnrealExtensions.UnrealCodeExt,
					exportContent,
					Encoding.ASCII
				);
			}
		   	package.CreateUPKGFile( exportPath );
			return exportPath;
		}
	}
}
