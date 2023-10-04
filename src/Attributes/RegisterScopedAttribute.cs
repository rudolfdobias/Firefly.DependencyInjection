using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    /// <summary>
    /// Registers class to DI with request scoped lifetime
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RegisterScopedAttribute : DiRegistrationAttribute
    {
        internal override ServiceLifetime Lifetime => ServiceLifetime.Scoped;
    }
}