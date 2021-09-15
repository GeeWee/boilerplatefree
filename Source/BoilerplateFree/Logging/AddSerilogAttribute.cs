using System;

namespace BoilerplateFree
{
    /// <summary>
    /// Use this to add a static Serilog logger on your class with the _logger name. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AddSerilogAttribute: Attribute
    {
    }
}
