using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    public abstract class DiRegistrationAttribute : Attribute, IDiRegistrationAttribute
    {
        /// <inheritdoc />
        public abstract Type Type { get; set; }

        /// <inheritdoc />
        public abstract void Register(IServiceCollection services, Type type);
    }
}