using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Firefly.DependencyInjection;

internal class Binder
{
	private readonly IServiceCollection _serviceCollection;
	internal readonly HashSet<RegistrationEntry> RegistrationEntries = new ();

	public Binder(IServiceCollection serviceCollection)
	{
		_serviceCollection = serviceCollection;
	}

	internal void LoadServices(DiRegistrationBuilder builder)
	{
		// 1. Load
		foreach (var ass in builder.Assemblies)
		{
			foreach (var type in ass.GetTypes())
			{
				var typeInfo = type.GetTypeInfo();
				foreach (var a in typeInfo.GetCustomAttributes<DiRegistrationAttribute>())
				{
					if (a.Type != null && builder.Selections.TryGetValue(a.Type, out var implType))
					{
						// Given type is marked for specific interface only
						if (type != implType)
							continue;
					}
					RegistrationEntries.Add(new RegistrationEntry(type, a.Lifetime, a.Type));
				}	
			}
		}
		
		// 2. Check
		// 2.1. Check if all single implementations have been met
		foreach (var (interfaceType, concreteType) in builder.Selections)
		{
			if (false == RegistrationEntries.Any(x => x.ObjectType == concreteType && x.OuterType == interfaceType))
				throw new ArgumentException($"No implementation of {concreteType} : {interfaceType} has been found.\n" +
				                            $" This binding had been declared by `PickSingleImplementation` method in SetupFireflyServiceRegistration.\n" +
				                            $" Note that the {interfaceType} must be set as Type parameter in the attribute, ie.:\n" +
				                            $" [RegisterScoped(Type = typeof({interfaceType.Name})]");
		}
	}

	internal void RegisterWithServiceCollection()
	{
		foreach (var binding in RegistrationEntries)
		{
			// If interface type (outer type) has been specified, use it for registration
			if (binding.OuterType != null)
				_serviceCollection.Add(new ServiceDescriptor(binding.OuterType, binding.ObjectType, binding.Lifetime));
			// If not, the service will be registered by its own type
			else
				_serviceCollection.Add(new ServiceDescriptor(binding.ObjectType, binding.ObjectType, binding.Lifetime));
		}	
	}
}