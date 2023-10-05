using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Firefly.DependencyInjection;

public class DiRegistrationBuilder
{
	/// <summary>
	/// List of assemblies to traverse
	/// </summary>
	internal readonly HashSet<Assembly> Assemblies = new();
	
	/// <summary>
	/// List of single implementations.
	/// Key is interface type, value is the concrete type 
	/// </summary>
	internal readonly Dictionary<Type, Type> Selections = new();
	
	/// <summary>
	/// Internal holder
	/// </summary>
	internal bool RegisterConcreteTypes { get; private set; }
	
	/// <summary>
	/// Registers all services from given assembly
	/// </summary>
	/// <param name="name"></param>
	/// <returns></returns>
	public DiRegistrationBuilder UseAssembly(string name)
		=> UseAssembly(Assembly.Load(new AssemblyName(name)));
	
	/// <summary>
	/// Registers all services from given assembly
	/// </summary>
	/// <param name="assembly"></param>
	/// <returns></returns>
	public DiRegistrationBuilder UseAssembly(Assembly assembly)
	{
		Assemblies.Add(assembly);
		return this;
	}

	/// <summary>
	/// Normally, if a service is registered with an interface, the concrete implementation is not registered
	/// to the DI. With RegisterAllImplementations(true), not only the interface but even the derived type(s) will be registered. 
	/// </summary>
	/// <param name="register"></param>
	/// <returns></returns>
	public DiRegistrationBuilder RegisterAllImplementations(bool register = true)
	{
		RegisterConcreteTypes = register;
		return this;
	}

	/// <summary>
	/// Instructs the binder to register single implementation of the interface
	/// </summary>
	/// <param name="interfaceType"></param>
	/// <param name="concreteType"></param>
	/// <returns></returns>
	public DiRegistrationBuilder PickSingleImplementation(Type interfaceType, Type concreteType)
	{
		if (Selections.ContainsKey(interfaceType))
			throw new ArgumentException($"Implementation of {interfaceType} has already been used.");
		
		Selections.Add(interfaceType, concreteType);
		return this;
	}

	/// <summary>
	/// Instructs the binder to register single implementation of the interface
	/// </summary>
	/// <param name="concreteType"></param>
	/// <typeparam name="TInterface"></typeparam>
	/// <returns></returns>
	public DiRegistrationBuilder PickSingleImplementation<TInterface>(Type concreteType)
		=> PickSingleImplementation(typeof(TInterface), concreteType);
	
	/// <summary>
	/// Instructs the binder to register single implementation of the interface
	/// </summary>
	/// <typeparam name="TInterface"></typeparam>
	/// <typeparam name="TConcrete"></typeparam>
	/// <returns></returns>
	public DiRegistrationBuilder PickSingleImplementation<TInterface, TConcrete>()
		=> PickSingleImplementation(typeof(TInterface), typeof(TConcrete));

	/// <summary>
	/// Get list of loaded assemblies for debug purposes
	/// </summary>
	/// <returns></returns>
	public Assembly[] GetUsedAssemblies() => Assemblies.ToArray();
	
	/// <summary>
	/// Get list of single implementations for debug purposes
	/// </summary>
	public KeyValuePair<Type, Type>[] GetSingleImplementations() => Selections.ToArray();
}