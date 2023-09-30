using Microsoft.Extensions.DependencyInjection;

namespace UEExplorer.Framework.Plugin
{
    public interface IPluginModule
    {
        void Build(IServiceCollection services);
        void Activate(PluginService service);
        void Deactivate();
    }
}
