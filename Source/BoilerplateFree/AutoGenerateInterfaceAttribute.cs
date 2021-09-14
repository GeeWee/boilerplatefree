namespace BoilerplateFree
{
    using System;

    /// <summary>
    /// Put this attribute on a class to make it generate an interface that's called I{CLASS_NAME}
    /// and has all the same public methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AutoGenerateInterfaceAttribute: Attribute
    {

    }
}
