using System;

namespace HelpersCommon
{
    public static class StringHelpers
    {
        public static bool EqualsOrdinalIgnoreCase(this string left, string right) =>
            string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
    }
}
