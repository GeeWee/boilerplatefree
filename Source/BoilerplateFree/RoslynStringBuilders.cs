namespace BoilerplateFree
{
    using System.Collections.Generic;

    internal static class RoslynStringBuilders
    {
        internal static string BuildUsingStrings(List<string> usingTypes)
        {
            var str = "";
            foreach (var usingType in usingTypes)
            {
                str += $"using {usingType}; \n";
            }

            return str;
        }
    }
}
