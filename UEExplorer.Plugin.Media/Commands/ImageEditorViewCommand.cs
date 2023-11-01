using System.Threading;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Services;
using UEExplorer.Plugin.Media.Image;
using UELib.Engine;

namespace UEExplorer.Plugin.Media.Commands
{
    [CommandCategory(CommandCategories.View)]
    public class ImageEditorViewCommand : MenuCommand, ICommand<object>
    {
        private readonly IDockingService _DockingService;

        public ImageEditorViewCommand(IDockingService dockingService)
        {
            _DockingService = dockingService;
        }

        public override string Text => "ImageEditor";

        public bool CanExecute(object subject) => true;

        public Task Execute(object subject)
        {
            _DockingService.AddDocumentUnique<ImageEditorControl>("ImageEditor", Text);

            return Task.CompletedTask;
        }
    }

    [CommandCategory(CommandCategories.Open)]
    public class OpenInImageEditorCommand : MenuCommand, IContextCommand
    {
        private readonly IDockingService _DockingService;
        private readonly ContextService _ContextService;

        public OpenInImageEditorCommand(IDockingService dockingService, ContextService contextService)
        {
            _DockingService = dockingService;
            _ContextService = contextService;
        }

        public bool CanExecute(object subject)
        {
            // TODO: Proper detection of any texture object
            return TargetResolver.Resolve(subject) is UTexture;
        }

        public Task Execute(object subject)
        {
            var control = _DockingService.AddDocumentUnique<ImageEditorControl>("ImageEditor" + subject.GetHashCode(),
                $"{ObjectPathBuilder.GetPath(subject)}[Image]");
            if (control == null)
            {
                return Task.FromCanceled(CancellationToken.None);
            }

            var contextInfo =
                new ContextInfo(ContextActionKind.Target, subject) { ResolvedTarget = TargetResolver.Resolve(subject) };
            return control.CanAccept(contextInfo) ? control.Accept(contextInfo) : Task.CompletedTask;
        }

        public override string Text => "View &Image";
    }
}
