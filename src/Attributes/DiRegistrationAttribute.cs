using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    public abstract class DiRegistrationAttribute : Attribute
    {
        /// <summary>
        /// Type used in DI Registration (Interface type) 
        /// </summary>
        public virtual Type? Type { get; set; }

        /// <summary>
        /// Lifetime of a service
        /// </summary>
        internal abstract ServiceLifetime Lifetime { get; }
    }
}