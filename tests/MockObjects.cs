// NOTE: Deliberately using wrong namespace to demonstrate whether the registrar can traverse 
// a default namespace.

namespace Firefly.DependencyInjection.Tests;

[RegisterTransient]
internal class MyService
{
	public int Counter { get; set; }
}

[RegisterSingleton]
internal class MyServiceSingleton
{
	public int Counter { get; set; }
}

[RegisterTransient(Type = typeof(MyBaseService))]
internal class MyDerivedService : MyBaseService
{
}

[RegisterTransient(Type = typeof(IMyService))]
internal class MyImplementingService : IMyService
{
}

internal interface IMyService
{
}

internal abstract class MyBaseService
{
}

[RegisterScoped]
internal class MyScopedService
{
}

[RegisterTransient]
internal class MyTransientService
{
}

internal interface IMultipleServiceInterface
{
}

[RegisterScoped(Type = typeof(IMultipleServiceInterface))]
internal class MultipleInstanceClass1 : IMultipleServiceInterface
{
}

[RegisterScoped(Type = typeof(IMultipleServiceInterface))]
internal class MultipleInstanceClass2 : IMultipleServiceInterface
{
}

[RegisterScoped(Type = typeof(IMultipleServiceInterface))]
internal class MultipleInstanceClass3 : IMultipleServiceInterface
{
}

[RegisterScoped]
internal class BadlyDefinedMultipleInstanceClass4 : IMultipleServiceInterface
{
}