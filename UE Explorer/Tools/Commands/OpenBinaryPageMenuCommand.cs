using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Pages;
using UEExplorer.Framework.UI.Services;
using UEExplorer.Properties;
using UEExplorer.UI.Pages;
using UELib;

namespace UEExplorer.Tools.Commands
{
    internal class OpenBinaryPageMenuCommand : MenuCommand, IContextCommand
    {
        public bool CanExecute(object subject) => TargetResolver.Resolve(subject) is IBinaryData binaryDataObject &&
                                                  binaryDataObject.BinaryMetaData != null;

        public Task Execute(object subject)
        {
            var dockingService = ServiceHost.GetRequired<IDockingService>();
            if (dockingService.HasDocument("BinaryTool", out BinaryContextPage page))
            {
            }
            else
            {
                page = PageFactory.CreateTrackingPage<BinaryContextPage>(true, "BinaryTool", typeof(BinaryContextPage));
                dockingService.AddDocument(page);
            }

            page.Invoke((MethodInvoker)(() =>
            {
                var contextService = ServiceHost.GetRequired<ContextService>();
                contextService.OnContextChanged(this,
                    new ContextChangedEventArgs(
                        new ContextInfo(ContextActionKind.Target, subject)
                        {
                            ResolvedTarget = TargetResolver.Resolve(subject)
                        }));
            }));

            return Task.CompletedTask;
        }

        public override string Text => Resources.NodeItem_ViewBinaryFields;
        public override Image Icon => Resources.Binary;
    }
}
