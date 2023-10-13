using System;
using System.Windows.Forms;
using UEExplorer.Framework;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Pages;
using UEExplorer.Framework.UI.Services;
using UEExplorer.UI.Pages;
using UELib;
using UELib.Decompiler;

namespace UEExplorer.Tools.Commands
{
    internal abstract class DecompilerFactoryLanguageCommand : MenuCommand
    {
        protected string PageUniqueName = "Decompiler";

        private static Type PageType => typeof(DecompilerContextPage);

        protected void SetupPage(object subject,
            IDecompiler<IOutputDecompiler<IAcceptable>, IAcceptable> decompiler)
        {
            var dockingService = ServiceHost.GetRequired<IDockingService>();
            if (dockingService.HasDocument(PageUniqueName, out DecompilerContextPage page))
            {
                page.Decompiler = decompiler;
            }
            else
            {
                page = PageFactory.CreateTrackingPage<DecompilerContextPage>(true, PageUniqueName, PageType);
                page.Decompiler = decompiler;
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
        }
    }
}
