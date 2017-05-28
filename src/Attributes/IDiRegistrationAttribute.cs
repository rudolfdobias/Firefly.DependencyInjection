using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    public interface IDiRegistrationAttribute
    {
        /// <summary>
        /// The type under the class will be registered (usually interface)
        /// </summary>
        Type Type { get; set; }

        /// <summary>
        /// Registers class to DI
        /// </summary>
        /// <param name="services"></param>
        /// <param name="type"></param>
        void Register(IServiceCollection services, Type type);
    }
}