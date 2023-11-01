using System.ComponentModel;
using UEExplorer.Framework.Plugin;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Framework.UI.Services;
using UEExplorer.Plugin.Media.Commands;
using UEExplorer.Plugin.Media.Controls;

namespace UEExplorer.Plugin.Media
{
    [DisplayName("Media")]
    [PluginProvideService(typeof(IMenuCommand), typeof(WavePlayerViewCommand))]
    [PluginProvideService(typeof(IMenuCommand), typeof(ImageEditorViewCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(OpenInWavePlayer))]
    [PluginProvideService(typeof(IContextCommand), typeof(OpenInImageEditorCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(OpenInScene))]
    public class MediaPluginModule : IPluginModule
    {
        private readonly IDockingService _DockingService;

        public MediaPluginModule(IDockingService dockingService)
        {
            _DockingService = dockingService;
        }

        public void Activate()
        {
            var control = _DockingService.AddDocument<ViewportPanel>("test", "test");
            control.DesiredTarget = null;
        }

        public void Deactivate()
        {
        }
    }
}
