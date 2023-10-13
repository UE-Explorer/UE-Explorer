using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Properties;
using UELib;
using UELib.Core;

namespace UEExplorer.Tools.Commands
{
    // TODO: Deprecate when we have our own Model tools
    [CommandCategory(CommandCategories.External)]
    internal class ExternalViewTool : MenuCommand, IContextCommand
    {
        public bool CanExecute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);
            return resolvedTarget is IUnrealViewable && File.Exists(Program.Options.UEModelAppPath);
        }

        public Task Execute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);

            var target = (IUnrealViewable)resolvedTarget;
            Debug.Assert(target != null);

            var obj = resolvedTarget as UObject;
            Debug.Assert(obj != null, nameof(obj) + " != null");

            var linker = obj.Package;
            Process.Start
            (
                Program.Options.UEModelAppPath,
                $"-path=\"{linker.PackageDirectory}\" \"{linker.PackageName}\" \"{obj.Name}\""
            );

            return Task.CompletedTask;
        }

        [Localizable(true)] public override string Text => Resources.NodeItem_OpenInUEModelViewer;

        public override Image Icon => Resources.Open;
    }

    // TODO: Deprecate when we have our own Model tools
    [CommandCategory(CommandCategories.External)]
    internal class ExternalExportTool : MenuCommand, IContextCommand
    {
        public bool CanExecute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);
            return resolvedTarget is IUnrealViewable && File.Exists(Program.Options.UEModelAppPath);
        }

        public Task Execute(object subject)
        {
            object resolvedTarget = TargetResolver.Resolve(subject);

            var exportableObject = (IUnrealExportable)resolvedTarget;
            Debug.Assert(exportableObject != null);

            var obj = resolvedTarget as UObject;
            Debug.Assert(obj != null, nameof(obj) + " != null");

            var linker = obj.Package;

            string packagePath = Application.StartupPath
                                 + "\\Exported\\"
                                 + linker.PackageName;

            string contentDir = packagePath + "\\Content";
            Directory.CreateDirectory(contentDir);
            string appArguments =
                $"-path=\"{linker.PackageDirectory}\" -out=\"{contentDir}\" -export \"{linker.PackageName}\" \"{((TreeNode)resolvedTarget).Text}\"";
            var appInfo = new ProcessStartInfo(Program.Options.UEModelAppPath, appArguments)
            {
                UseShellExecute = false, RedirectStandardOutput = true, CreateNoWindow = false
            };
            string log = string.Empty;
            var app = Process.Start(appInfo);
            app.OutputDataReceived += (sender, e) => log += e.Data;

            if (Directory.GetFiles(contentDir).Length > 0)
            {
                if (MessageBox.Show(
                        Resources.UC_PackageExplorer_PerformNodeAction_QUESTIONEXPORTFOLDER,
                        Application.ProductName,
                        MessageBoxButtons.YesNo
                    ) == DialogResult.Yes)
                {
                    Process.Start(contentDir);
                }
            }
            else
            {
                MessageBox.Show
                (
                    $"The object was not exported.\r\n\r\nArguments:{appArguments}\r\n\r\nLog:{log}",
                    Application.ProductName
                );
            }

            return Task.CompletedTask;
        }

        [Localizable(true)] public override string Text => Resources.NodeItem_ExportWithUEModelViewer;

        public override Image Icon => Resources.Export;
    }
}
