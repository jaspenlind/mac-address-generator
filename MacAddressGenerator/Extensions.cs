using System;
using System.IO;
using System.Linq;
using System.Reflection;

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
                throw new ArgumentException("Cannot be a negative number", nameof(length));
            }

            return str.Substring(0, Math.Min(length, str.Length));
        }

        public static Stream GetEmbeddedResource(this Assembly assembly, string resourceName)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (string.IsNullOrWhiteSpace(resourceName))
            {
                throw new ArgumentException("Cannot be null or whitespace", nameof(resourceName));
            }
            var embeddedResourceName = assembly.GetManifestResourceNames().FirstOrDefault(_ => _.EndsWith(resourceName));

            if (embeddedResourceName == null)
            {
                throw new ArgumentException($"A resource with name {resourceName} could not be found", nameof(resourceName));
            }

            return assembly.GetManifestResourceStream(embeddedResourceName) ?? throw new ArgumentException($"Unable to read manifest resource stream for {resourceName}");
        }
    }
}