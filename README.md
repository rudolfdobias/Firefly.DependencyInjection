# Firefly.DependencyInjection

[![NuGet](https://img.shields.io/nuget/v/Firefly.DependencyInjection.svg)](https://www.nuget.org/packages/Firefly.DependencyInjection)
[![license](https://img.shields.io/github/license/mashape/apistatus.svg)]()

Attribute driven class DI registration for C# .NET Core applications.

## Installation

Linux/OSX
```
bash$ dotnet add package Firefly.DependencyInjection
```

Windows
```
Install-Package Firefly.DependencyInjection
```

### Basic usage

```cs

namespace Pub.Services {

    [RegisterScoped]
    public class BartenderService {
        
        public Beer GiveMeBeer() {...}
    } 
}

```

```cs

namespace Pub.Controllers {

    public class OrdersController {
        
        public OrdersController(BartenderService bartenderService){
            var beer = bartenderService.GiveMeBeer();
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

```

```cs

namespace Pub.Controllers {

    public class OrdersController {
        
        public OrdersController(IBartenderService bartenderService){
            var beer = bartenderService.GiveMeBeer();
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