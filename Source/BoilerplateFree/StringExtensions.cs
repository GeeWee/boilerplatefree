namespace BoilerplateFree
{
    internal static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            if (char.IsUpper(str[0]))
            {
                if (!string.IsNullOrEmpty(str) && str.Length > 1)
                {
                    return char.ToLowerInvariant(str[0]) + str.Substring(1);
                }

                return str;
            }
            if (str[0] == '_')
            {
                // Remove the _ in the field name
                return str.Substring(1);
            }

            // If none of the transformations above work, just return as-is
            return str;
        }
    }
}
