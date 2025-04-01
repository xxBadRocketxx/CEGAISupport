using System;

namespace CEGAISupport.Utils
{
    public static class StringExtensions
    {
        public static bool ContainsCaseInsensitive(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}