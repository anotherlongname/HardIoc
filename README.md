# WIP!!! - This project is very much a work in progress - WIP!!!

# HardIoc
C# Compile-time IoC container generator made possible using Roslyn Code Generators

## Progress
- [x] Transient Registration
- [x] Singleton Registration
- [x] Delegate Registration
- [x] Work with Generic types correctly
- [x] Support for multi-constructor dependencies
- [x] Parameterless Factory Registration
- [ ] Parameterized Factory Registration
- [ ] Validation of dependency graph
- [ ] Correct error handling and code highlighting
- ~~[ ] Change `ConstructorFor` entrypoints with `Resolve<T>` instances~~ Now doing both methods instead
- [x] Usage documentation
- [ ] Think of a better name

## Overview
TODO

## Usage
To create a container class, first inherit the `Container` class. Then add your registrations as interfaces to it. Be sure to declare your class as partial so the generated code can be created correctly:

```csharp
public partial class MyContainer : Container,
    Register.Transient<IMyService, MyImplimentation>,
    Register.Singleton<MySingletonService, MySingletonService>
{
}
```

The following registrations are supported:

### Transient

Register a service that will be instantiated each time the service is injected.
```csharp
Register.Transient<TService>
Register.Transient<TService, TImplimentation>
```
### Singleton

Register a service that will be instantiated only on first time it is injected, and will be shared throughout the contianer
```csharp
Register.Singleton<TService>
Register.Singleton<TService, TImplimentation>
```

### Delegate
Register a delegate that will be used to create the specified service. The interface will require a method to be created on the nongenerated side that will return the specified service, given a set of dependencies
```csharp
Register.Delegate<TService>
Register.Delegate<TService, TDependency>
Register.Delegate<TService, TDependency1, TDependency2>
Register.Delegate<TService, TDependency1, TDependency2, TDependency3>
Register.Delegate<TService, TDependency1, TDependency2, TDependency3, TDependency4>
Register.Delegate<TService, TDependency1, TDependency2, TDependency3, TDependency4, TDependency5>
```
The corresponding required method will look similar to this:
```csharp
TService Register.Delegate<TService, TDependency>.Create(TDependency dependency)
{
    ...
}
```

### Factory
Register an interface as a factory that can be used to resolve a service when required
```csharp
Register.Factory<TFactory>
```
*NOTE: Currently only parameterless service creation is possible within factories*

### `ConstructorFor` Attribute

You can add the `ConstructorFor` attribute on your container in order to have it generate a method to create a specified service type

```csharp
[ConstructorFor(typeof(MyService))]
public partial class MyContianer : Container,
    Register.Transient<MyService>
{
}
```


### Using a Generated Container

To use the container you can simply construct it and call any creat methods that you have included by adding `ConstructorFor` attributes, or by calling the `Resolve` or `TryResolve` methods.

```csharp
var contianer = new MyContainer();
var myService = container.CreateMyService();
var myOtherService = container.Resolve<MyOtherService>(); // or Resolve(typeof(MyOtherService))
```

## ASP.Net Core Support
Hard IoC supports replacing the default ASP.Net Core service provider. This is technically not a full "replacement" of the service provider, but it easily integrates into the default provider and allows resolving dependencies from both Hard IoC and the ASP.Net service provider.

Instead of using the `Container` type, use the `AspNetCoreContainer` type. This will add an `IServiceProvider` that can be used for delegate registrations to allow access to dependencies in the default Service Provider. Because the container is generated at compile-time you normally can't have direct access to the Service Provider services, but delegate registrations allow you to reference the Service Provider after it has been created.

```csharp
public partial class MyContainer : AspNetCoreContainer,
    Register.Delegate<IConfiguration>
{
    IConfiguration Register.Delegate<IConfiguration>.Create()
        => (IConfiguration)ServiceProvider.GetService(typeof(IConfiguration));
}
```

This will also add a `ServiceProviderFactory` for the container to easily replace the Service Provider during startup. During the host builder call the `.UseServiceProviderFactory()` method and pass the generated factory method `{ContainerName}.ServiceProviderFactory.Create()`

```csharp
// Program.cs

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseServiceProviderFactory(MyContainer.ServiceProviderFactory.Create());

```