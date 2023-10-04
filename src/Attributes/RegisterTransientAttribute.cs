using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection
{
    /// <summary>
    /// Registers class to DI with transient lifetime
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RegisterTransientAttribute : DiRegistrationAttribute
    {
        internal override ServiceLifetime Lifetime => ServiceLifetime.Transient;
    }
}