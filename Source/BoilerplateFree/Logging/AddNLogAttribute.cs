namespace BoilerplateFree
{
    using System;

    /// <summary>
    /// Use this to add a static NLogger logger on your class with the _logger name.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AddNLogAttribute: Attribute
    {
    }
}
