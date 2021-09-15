namespace BoilerplateFree
{
    using System;

    /// <summary>
    /// Use this to add an NLogger on your class under the _logger scope.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public class AddNLogAttribute: Attribute
    {
    }
}
