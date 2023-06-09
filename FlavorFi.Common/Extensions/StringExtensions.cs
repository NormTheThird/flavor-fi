using System;

namespace FlavorFi.Common.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comparison)
        {
            return source != null && toCheck != null && source.IndexOf(toCheck, comparison) >= 0;
        }

    }
}