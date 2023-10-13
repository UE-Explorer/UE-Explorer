using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace UEExplorer.Framework.Plugin
{
    public class PluginService
    {
        private readonly IServiceProvider _ServiceProvider;

        public PluginService(IServiceProvider serviceProvider)
        {
            _ServiceProvider = serviceProvider;
        }

        public IEnumerable<IPluginModule> GetLoadedModules()
        {
            return _ServiceProvider.GetServices<IPluginModule>();
        }
    }
}
