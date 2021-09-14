namespace BoilerplateFree
{
    using System;

    /// <summary>
    /// Put this attribute on something to make it autogenerate the constructor
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoGenerateConstructorAttribute: Attribute
    {

    }
}
