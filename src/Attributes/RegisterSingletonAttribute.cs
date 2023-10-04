using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    /// <summary>
    /// Registers class to DI as a singleton
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RegisterSingletonAttribute : DiRegistrationAttribute
    {
        internal override ServiceLifetime Lifetime => ServiceLifetime.Singleton;
    }
}