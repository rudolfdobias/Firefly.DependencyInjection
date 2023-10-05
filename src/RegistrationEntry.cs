using System;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection;

internal record RegistrationEntry(Type ObjectType, ServiceLifetime Lifetime, Type? OuterType = null, bool RegisterConcreteTypes = false)
{
	public Type ObjectType { get; } = ObjectType;
	public Type? OuterType { get; } = OuterType;
	public ServiceLifetime Lifetime { get; } = Lifetime;
	public bool RegisterConcreteTypes { get; } = RegisterConcreteTypes;
}