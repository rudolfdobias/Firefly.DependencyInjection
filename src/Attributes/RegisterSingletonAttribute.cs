using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Firefly.DependencyInjection
{
    /// <summary>
    /// Registers class to DI as a singleton
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RegisterSingletonAttribute : DiRegistrationAttribute
    {
        /// <inheritdoc />
        public override Type Type { get; set; }

        /// <inheritdoc />
        public override void Register(IServiceCollection services, Type type)
        {
            Console.Out.WriteLine($"Registering {type} as {Type}");
            if (Type != null)
            {
                services.TryAdd(ServiceDescriptor.Singleton(Type, type));
            }
            services.TryAdd(ServiceDescriptor.Singleton(type, type));
        }
    }
}