using UEExplorer.Framework.Plugin;
using UEExplorer.Framework.UI.Commands;
using UEExplorer.Tools.Commands;

namespace UEExplorer.Tools
{
    [PluginProvideService(typeof(IContextCommand), typeof(DecompilerFactoryMenuCommand))]
    [PluginProvideService(typeof(IDecompilerFactoryCommand),
        typeof(UnrealScriptLegacyDecompilerFactoryLanguageCommand))]
    
#if DEBUG
    [PluginProvideService(typeof(IDecompilerFactoryCommand), typeof(UnrealScriptDecompilerFactoryLanguageCommand))]
    [PluginProvideService(typeof(IDecompilerFactoryCommand), typeof(UnrealT3DDecompilerFactoryLanguageCommand))]
#endif
    
#if DEBUG
    [PluginProvideService(typeof(IContextCommand), typeof(OpenBinaryPageMenuCommand))]
#endif
    [PluginProvideService(typeof(IContextCommand), typeof(OpenBufferPageMenuCommand))]
    
    [PluginProvideService(typeof(IContextCommand), typeof(ExportFactoryMenuCommand))]
    [PluginProvideService(typeof(IExportFactoryCommand), typeof(LegacyExportAsFactoryCommand))]
    [PluginProvideService(typeof(IExportFactoryCommand), typeof(ExportPackageReferenceClassesMenuCommand))]
    [PluginProvideService(typeof(IExportFactoryCommand), typeof(ExportPackageReferenceScriptsMenuCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(ExternalViewTool))]
    [PluginProvideService(typeof(IContextCommand), typeof(ExternalExportTool))]
    [PluginProvideService(typeof(IContextCommand), typeof(LoadPackageReferenceCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(UnloadPackageReferenceCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(RemovePackageReferenceCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(OpenPackageReferenceInExplorerCommand))]
    [PluginProvideService(typeof(IContextCommand), typeof(OpenPackageReferenceSettingsCommand))]
    internal class InternalToolsPlugin : IPluginModule
    {
        public void Activate()
        {
        }

        public void Deactivate()
        {
        }
    }
}
