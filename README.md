# Firefly.DependencyInjection

[![NuGet](https://img.shields.io/nuget/v/Firefly.DependencyInjection.svg)](https://www.nuget.org/packages/Firefly.DependencyInjection)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)]()

Simple attribute driven class DI registration for .NET Core applications.

## Installation

Linux/OSX
```
bash$ dotnet add package Firefly.DependencyInjection
```

Windows
```
PM> Install-Package Firefly.DependencyInjection
```

### How to make it work

At Startup.cs
```cs
public void ConfigureServices(IServiceCollection services)
{
    ...
    services.UseInlineDiRegistration();
    ...
}

```

### Basic usage

```cs

namespace Pub.Services {

    [RegisterScoped]
    public class BartenderService {
        
        public Beer GiveMeBeer() {...}
    } 
}


namespace Pub.Controllers {

    public class OrdersController {
        
        public OrdersController(BartenderService bartenderService){
            var beer = bartenderService.GiveMeBeer(); // here we go
        }
    } 
}

```

### Using interfaces

```cs

namespace Pub.Services {

    [RegisterScoped(Type = typeof(IBartenderService))]
    public class BartenderService : IBartenderService{
        
        public Beer GiveMeBeer() {...}
    } 
}


namespace Pub.Controllers {

    public class OrdersController {
        
        public OrdersController(IBartenderService bartenderService){
            var beer = bartenderService.GiveMeBeer(); // here we go
        }
    } 
}

```

### Possibilities

```cs

// Registers class as a singleton (one instance forever)
[RegisterSingleton]

// Registers class with request scoped lifetime (one instance per request)
[RegisterScoped]

// Registers class with transient lifetime (creates instance with every single call)
[RegisterTransient]

// Type assignment works for all of them
[RegisterScoped(Type = typeof(IMyInterface))]
[RegisterScoped(Type = typeof(MyParentBaseType))]

// Allows multiple identifiers
[RegisterScoped]
[RegisterScoped(Type = typeof(IMyService))]
public class MyService : IService {}

```

### Advanced

 Enabling registration for custom assembly:
 ```cs
 services.UseInlineDiRegistration(Assembly.Load(new AssemblyName("My.Custom.Assembly")));

 ```