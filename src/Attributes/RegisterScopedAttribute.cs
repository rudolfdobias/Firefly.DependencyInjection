using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Firefly.DependencyInjection
{
    /// <summary>
    /// Registers class to DI with request scoped lifetime
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RegisterScopedAttribute : DiRegistrationAttribute
    {
        /// <inheritdoc />
        public override Type Type { get; set; }

        /// <inheritdoc />
        public override void Register(IServiceCollection services, Type type)
        {
            Console.Out.WriteLine($"Registering {type} as {Type}");
            if (Type != null)
            {
                services.TryAdd(ServiceDescriptor.Scoped(Type, type));
            }
            services.TryAdd(ServiceDescriptor.Scoped(type, type));
        }
    }
}