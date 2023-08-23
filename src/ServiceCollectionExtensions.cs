using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register all classes with Register* attributes in DI (<see cref="IDiRegistrationAttribute"/>)
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

        /// <summary>
        /// Registers all classes with Register* attributes in DI. (<see cref="IDiRegistrationAttribute"/>)
        /// Goes through all namespaces specified by "namespaceName". Useful when using with multiple project.
        /// Recommended usage:
        /// services.UseInlineDiRegistrationForNamespace(nameof(MyApplicationRoot));
        /// </summary>
        /// <param name="services"></param>
        /// <param name="namespaceName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection UseInlineDiRegistrationForNamespace(this IServiceCollection services,
            string namespaceName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => a.FullName.StartsWith(namespaceName))
                .ToList();

            if (assemblies.Any() == false)
            {
                throw new ArgumentException($"No namespaces found with prefix \"{namespaceName}\".");
            }

            foreach (var assembly in assemblies)
            {
                Console.Out.WriteLine(assembly);
                services.UseInlineDiRegistration(assembly);
            }

            return services;
        }
    }
}