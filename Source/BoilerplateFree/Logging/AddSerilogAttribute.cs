using System;

namespace BoilerplateFree
{
    /// <summary>
    /// Use this to add a Serilog logger on your class under the _logger scope. 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AddSerilogAttribute: Attribute
    {
    }
}
