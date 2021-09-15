# BoilerPlateFree
[Install on Nuget](https://www.nuget.org/packages/BoilerplateFree/)

**This project is still in alpha stages. Use at your own risk**

Remove boilerplate via C# 9 [source generators](https://devblogs.microsoft.com/dotnet/introducing-c-source-generators/) and attributes. The attributes currently supported are:

# AutoGenerateConstructor
This attribute takes all private fields and generates a constructor based on them. Never write another boilerplate constructor again!

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
Are you also tired of implementing a nonsensical `IYourService` just do you can replace a dependency in your tests?
Well you don't have to take it anymore. Let source generators do it for you!
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
