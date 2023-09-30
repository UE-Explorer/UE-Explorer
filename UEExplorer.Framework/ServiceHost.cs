using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace UEExplorer.Framework
{
    public static class ServiceHost
    {
        private static IServiceProvider InstanceServiceProvider { get; set; }

        public static IServiceProvider Build(IHostBuilder builder)
        {
            InstanceServiceProvider = builder.Build().Services;
            return InstanceServiceProvider;
        }

        public static IServiceProvider Get()
        {
            if (InstanceServiceProvider == null)
            {
                throw new InvalidOperationException();
            }

            return InstanceServiceProvider;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Get<T>() where T : class
        {
            return (T)InstanceServiceProvider.GetService(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRequired<T>() where T : class
        {
            return (T)InstanceServiceProvider.GetRequiredService(typeof(T));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetAll<T>() where T : class
        {
            return InstanceServiceProvider.GetServices<T>();
        }
    }
}
