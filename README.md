# BoilerPlateFree

[Install on Nuget](https://www.nuget.org/packages/BoilerplateFree/)

**This project in alpha stages, however it is not currently maintained. Please feel free to fork it if you want to develop further on the concept.**

**You need to use at least .NET 5.0.4 for this library to work. Earlier versions of .NET 5 will not work**

Remove boilerplate via C# 9 [source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/)
and attributes. The attributes currently supported are:

# AutoGenerateConstructor

This attribute takes all private fields and generates a constructor based on them. Never write another boilerplate
constructor again!

### Before:

```csharp
public class YourService
{
    private YourOtherService _otherService;
    private ILogger<YourService> _logger;
    private YourThirdService _thirdService;

    public YourService(YourOtherService otherService, ILogger<YourService> logger, YourThirdService thirdService)
    {
        _otherService = otherService;
        _logger = logger;
        _thirdService = thirdService;
    }
}
```

### After:

- Add the `[AutoGenerateConstructor]` attribute
- Mark the class as `partial`
- Remove the constructor

```csharp
[AutoGenerateConstructor]
public partial class YourService
{
    private YourOtherService _otherService;
    private ILogger<YourService> _logger;
    private YourThirdService _thirdService;
}
```

(And the generated code looks like this - but you don't need to look at that)

```csharp
public partial class YourService
{
    public YourService(YourOtherService otherService, ILogger<YourService> logger,
        YourThirdService thirdService)
    {
        this._otherService = otherService;
        this._logger = logger;
        this._thirdService = thirdService;
    }
}
```

# AutoGenerateInterface

Are you also tired of implementing a nonsensical `IYourService` just do you can replace a dependency in your tests? Well
you don't have to take it anymore. Let source generators do it for you!

### Before

```csharp
public interface IGenerateAutoInterfaceClass
{
    void Foo();
    int Bar(int param1);
}

public class GenerateAutoInterfaceClass : IGenerateAutoInterfaceClass
{
    public void Foo()
    {
        Console.WriteLine("Foo");
    }

    public int Bar(int param1) => 1 + param1;
}
```

### After

- Add the `[AutoGenerateInterface]` attribute to your class
- Have your class inherit from `I{YOUR_CLASS_NAME}`.
- Enjoy an interface that exposes all public methods without having to write a line of code.

```csharp
[AutoGenerateInterface]
public class GenerateAutoInterfaceClass : IGenerateAutoInterfaceClass
{
    public void Foo()
    {
        Console.WriteLine("Foo");
    }

    public int Bar(int param1) => 1 + param1;
}
```

And the generated interface looks like this:

```csharp
public interface IGenerateAutoInterfaceClass
{
    public void Foo();

    public int Bar(int param1);
}
```

# Logger Attributes

If you're not a fan of getting your loggers through Dependency Injection and would prefer it as a static field on your
class - it's only a attribute away. Simply mark your class as `partial` use the `[AddSerilog]` or `[AddNLog]` attribute
to your class to get a logger from the framework with the class context set properly.

```csharp
[AddNLog]
public partial class MyClass
{
    public void SomeMethod()
    {
        _logger.Info("Look! Access to loggers without having them clutter up your class and look at them in your constructors!");
    }
}
```

If you prefer relying on dependency injection and the Microsoft `ILogger<>` interface, you can of course define that as
a field, and have `[AutoGenerateConstructor]` to create the constructor automatically.
