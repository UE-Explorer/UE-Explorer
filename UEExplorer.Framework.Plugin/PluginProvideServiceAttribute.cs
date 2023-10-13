using System;

namespace UEExplorer.Framework.Plugin
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class PluginProvideServiceAttribute : Attribute
    {
        public readonly Type ServiceType;
        public readonly Type ImplementationType;

        public PluginProvideServiceAttribute(Type serviceType, Type implementationType)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
        }
    }
}
