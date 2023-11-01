using System.Threading;
using System.Threading.Tasks;
using UEExplorer.Framework;
using UEExplorer.Framework.Commands;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Services;
using UEExplorer.Plugin.Media.Audio;
using UEExplorer.Plugin.Media.Controls;
using UELib.Core;
using UELib.Engine;

namespace UEExplorer.Plugin.Media.Commands
{
    [CommandCategory(CommandCategories.View)]
    public class WavePlayerViewCommand : MenuCommand, ICommand<object>
    {
        private readonly IDockingService _DockingService;
        private readonly ContextService _ContextService;

        public WavePlayerViewCommand(IDockingService dockingService, ContextService contextService)
        {
            _DockingService = dockingService;
            _ContextService = contextService;
        }

        public override string Text => "WavePlayer";

        public bool CanExecute(object subject) => true;

        public Task Execute(object subject)
        {
            var control = _DockingService.AddDocumentUnique<WavePlayerControl>("WavePlayer", Text);
            if (control == null)
            {
                return Task.FromCanceled(CancellationToken.None);
            }

            if (_ContextService.ActiveContext != null && control.CanAccept(_ContextService.ActiveContext))
            {
                return control.Accept(_ContextService.ActiveContext);
            }

            return Task.CompletedTask;
        }
    }

    [CommandCategory(CommandCategories.Open)]
    public class OpenInWavePlayer : MenuCommand, IContextCommand
    {
        private readonly IDockingService _DockingService;
        private readonly ContextService _ContextService;

        public OpenInWavePlayer(IDockingService dockingService, ContextService contextService)
        {
            _DockingService = dockingService;
            _ContextService = contextService;
        }

        public bool CanExecute(object subject)
        {
            return TargetResolver.Resolve(subject) is USound;
        }

        public Task Execute(object subject)
        {
            var control = _DockingService.AddDocumentUnique<WavePlayerControl>("WavePlayer" + subject.GetHashCode(),
                $"{ObjectPathBuilder.GetPath(subject)}[Audio]");
            if (control == null)
            {
                return Task.FromCanceled(CancellationToken.None);
            }

            var contextInfo =
                new ContextInfo(ContextActionKind.Target, subject) { ResolvedTarget = TargetResolver.Resolve(subject) };
            return control.CanAccept(contextInfo) ? control.Accept(contextInfo) : Task.CompletedTask;
        }

        public override string Text => "Play &Sound";
    }

    [CommandCategory(CommandCategories.Open)]
    public class OpenInScene : MenuCommand, IContextCommand
    {
        private readonly IDockingService _DockingService;
        private readonly ContextService _ContextService;

        public OpenInScene(IDockingService dockingService, ContextService contextService)
        {
            _DockingService = dockingService;
            _ContextService = contextService;
        }

        public bool CanExecute(object subject)
        {
            return TargetResolver.Resolve(subject) is UPolys;
        }

        public Task Execute(object subject)
        {
            var control = _DockingService.AddDocumentUnique<ViewportPanel>("Scene" + subject.GetHashCode(),
                $"{ObjectPathBuilder.GetPath(subject)}[Model]");
            if (control == null)
            {
                return Task.FromCanceled(CancellationToken.None);
            }

            var contextInfo =
                new ContextInfo(ContextActionKind.Target, subject) { ResolvedTarget = TargetResolver.Resolve(subject) };

            control.DesiredTarget = contextInfo.ResolvedTarget;
            
            return Task.CompletedTask;
        }

        public override string Text => "View in &Scene";
    }
}
