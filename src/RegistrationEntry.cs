using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection;

internal record RegistrationEntry(Type ObjectType, ServiceLifetime Lifetime, Type? OuterType = null)
{
	public Type ObjectType { get; } = ObjectType;
	public Type? OuterType { get; } = OuterType;
	public ServiceLifetime Lifetime { get; } = Lifetime;
}