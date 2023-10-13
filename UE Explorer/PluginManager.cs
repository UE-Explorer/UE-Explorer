using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using UEExplorer.Framework.Plugin;

namespace UEExplorer
{
    internal static class PluginManager
    {
        public static IEnumerable<Type> LoadModules(string pluginsPath)
        {
            if (!Directory.Exists(pluginsPath))
            {
                yield break;
            }

            string[] pluginDirectories = Directory.GetDirectories(pluginsPath);
            // TODO: Limit search to the first level directories.
            foreach (string pluginDirectory in pluginDirectories)
            {
                string pluginPackageFile = Path.Combine(pluginDirectory, "plugin.xml");
                if (!File.Exists(pluginPackageFile))
                {
                    Console.Error.Write($"Missing \"plugin.xml\" file in plugin directory \"{pluginDirectory}\"");
                    continue;
                }

                PluginPackage package;
                using (var reader = new XmlTextReader(pluginPackageFile))
                {
                    var serializer = new XmlSerializer(typeof(PluginPackage));
                    package = (PluginPackage)serializer.Deserialize(reader);
                }

                string pluginDllPath = Path.Combine(pluginDirectory, package.PackageDllFileName);
                if (!File.Exists(pluginDllPath))
                {
                    Console.Error.Write(
                        $"Missing DLL \"{pluginDllPath}\" file in plugin directory \"{pluginDirectory}\"");
                    continue;
                }

                var pluginAssembly = Assembly.LoadFrom(pluginDllPath);
                yield return pluginAssembly.ExportedTypes.First(t => typeof(IPluginModule).IsAssignableFrom(t));
            }
        }
    }
}
