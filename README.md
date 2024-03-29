# Attribute-driven Service Registration

[![NuGet](https://img.shields.io/nuget/v/Firefly.DependencyInjection.svg)](https://www.nuget.org/packages/Firefly.DependencyInjection)
[![NuGet](https://img.shields.io/nuget/dt/Firefly.DependencyInjection.svg)](https://www.nuget.org/packages/Firefly.DependencyInjection)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)]()

---

```csharp
// Declaration
[RegisterScoped(Type = typeof(IMyService))]
public class MyServiceImplementation : IMyService
{}

// Consumer
[RegisterScoped]
public class MyServiceConsumer(){
    public MyServiceConsumer(IMyService myService)
    {
        ...
    }
}
```

## Features

 - Simple and fast service registration with single attribute
 - All service lifetimes supported, of course
 - Binding interface to one or more implementations with option to expose concrete types as well
 - Select which assemblies will be scanned for auto-registration
 - Possibility to select a single i-face implementation of many based on your custom logic

## Installation

Linux/OSX
```shell
dotnet add package Firefly.DependencyInjection
```

Windows
```shell
Install-Package Firefly.DependencyInjection
```

.csproj
```xml
<PackageReference Include="Firefly.DependencyInjection" />
```

## Basic Setup

During application startup, find a `IServiceCollection` instance and call `AddFireflyServiceRegistration()`.

The location depends on your [Hosting Model](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis/webapplication?view=aspnetcore-7.0):

#### WebApplicationBuilder
```cs
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddFireflyServiceRegistration();
```
#### Older Startup.cs
```csharp
public virtual void ConfigureServices(IServiceCollection services)
{
    services.AddFireflyServiceRegistration();
}
```
#### Manually created ServiceCollction 
```csharp
var sc = new ServiceCollection()
sc.AddFireflyServiceRegistration();
```
---

## Usage

### Service Lifetimes

```cs
// All three lifetimes are expressed by attributes

[RegisterTransient]
public class MyTransientService {}

[RegisterSingleton]
public class MyScopedService {}

[RegisterScoped]
public class MyScopedService {}
```

### Using interfaces

```csharp
// Declaration
[RegisterScoped(Type = typeof(IMyService))]
public class MyServiceImplementation : IMyService
{}

// Consumer
[RegisterScoped]
public class MyServiceConsumer(){
    public MyServiceConsumer(IMyService myService)
    {
        ...
    }
}
```

### Multiple implementations of an interface

```cs
[RegisterScoped(Type = typeof(IMyService))] 
public class MyServiceA : IMyService {} // Variant A

[RegisterScoped(Type = typeof(IMyService))] 
public class MyServiceB : IMyService {} // Variant B

[RegisterScoped]
public class Consumer 
{
    // Services will be injected the same way as you're used to.
    public Consumer(ICollection<IMyService> myInstances){}
}
```

### Registering concrete types along with its interface

By default, if a service is registered with an interface, the concrete implementation is not registered
into the container. With `RegisterAllImplementations(true)`, not only the interface but even the derived type(s) will be registered.

```csharp
// Setup
services.AddFireflyServiceRegistration(builder => {
    builder.RegisterAllImplementations()    
});

// Declaration
[RegisterScoped(Type = typeof(IMyService))]
public class MyServiceImplementation : IMyService {}

// Consumer
[RegisterScoped]
public class MyServiceConsumer(){
    public MyServiceConsumer(IMyService ifaceImpl, MyServiceImplementation concreteImpl)
    {
        // Both dependencies are resolved
        Asert.True(ifaceImpl.GetType() == concreteImpl.GetType());
    }
}
```

> [!NOTE]  
> If the service lifetime is Transient, both object will have different instances.


### Picking single implementation of an interface from many

There can be a situation where you need to choose an implementation at the runtime. 
This is an example of choosing an filesystem provider based on a string during the application startup.

Let's have two different impl. of a `IFileProvider` interface.
```csharp
// Implementation A
[RegisterScoped(Type = typeof(IFileProvider))]
public class BlobFileProvider : IFileProvider {} 

// Implementation B
[RegisterScoped(Type = typeof(IFileProvider))]
public class LocalFileProvider : IFileProvider {}
```

Application startup:
```csharp
var useLocalFiles = true;

services.AddFireflyServiceRegistration(builder => {
    if (useLocalFiles)
        builder.PickSingleImplementation<IFileProvider>(typeof(LocalFileProvider));
    else
        builder.PickSingleImplementation<IFileProvider>(typeof(BlobFileProvider));
});
```

The consuming service:
```csharp
public class FilesystemConsumer {
    public FilesystemConsumer(IFileProvider provider){
        // provider will LocalFileProvider
    }
}
```

You may also use another two overrides that allow you to pass the Types via Type Parameters or via Type function argument.
```csharp
// From DiRegistrationBuilder: 
public DiRegistrationBuilder PickSingleImplementation(Type interfaceType, Type concreteType);
public DiRegistrationBuilder PickSingleImplementation<TInterface>(Type concreteType);
public DiRegistrationBuilder PickSingleImplementation<TInterface, TConcrete>()
```

---

### Registering services from other Assemblies

It's fully possible to include another assembly. All these assemblies will be scanned for `[Register*]` attributes.
> [!IMPORTANT]  
> Referencing an assembly is needed if you want to register services from another project in your solution.

```csharp
services.AddFireflyServiceRegistration(builder => {
    builder.UseAssembly("Example.Assembly.Name"); // Locate assembly by string
    builder.UseAssembly(Assembly.GetEntryAssembly()); // Specify assembly by the Assembly type and pass anything you need.
});
```


