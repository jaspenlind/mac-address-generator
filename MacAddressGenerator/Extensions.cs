using System;

namespace MacAddressGenerator
{
    public static class Extensions
    {
        public static string Truncate(this string str, int length)
        {
            if (str == null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            if (length < 1)
            {
                throw new ArgumentException("Cannot be negative", nameof(length));
            }

            return str.Substring(0, Math.Min(length, str.Length));
        }
    }
}