using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all classes with Registration attributes to DI (<see cref="IDiRegistrationAttribute"/>)
        /// </summary>
        /// <param name="services"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static IServiceCollection UseInlineDiRegistration(this IServiceCollection services,
            Assembly assembly = null)
        {
            if (assembly == null)
            {
                assembly = Assembly.GetEntryAssembly();
            }
            foreach (var type in assembly.GetTypes())
            {
                var typeInfo = type.GetTypeInfo();

                foreach (var a in typeInfo.GetCustomAttributes<DiRegistrationAttribute>())
                {
                    a.Register(services, type);
                }
            }

            return services;
        }
    }
}