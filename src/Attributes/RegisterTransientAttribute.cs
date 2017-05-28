using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Firefly.DependencyInjection
{
    /// <summary>
    /// Registers class to DI with transient lifetime
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RegisterTransientAttribute : DiRegistrationAttribute
    {
        /// <inheritdoc />
        public override Type Type { get; set; }

        /// <inheritdoc />
        public override void Register(IServiceCollection services, Type type)
        {
            if (Type != null)
            {
                services.TryAdd(ServiceDescriptor.Transient(Type, type));
            }
            services.TryAdd(ServiceDescriptor.Transient(type, type));
        }
    }
}