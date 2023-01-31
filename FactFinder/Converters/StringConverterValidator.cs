using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FactFinder.Converters
{
    public static class StringConverterValidator
    {
        /// <summary>
        /// Checks if given string can be converted to the given format
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatNotAllowedException"></exception>
        public static bool CanBeConverted(string stringToCheck, Format format)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentException($"'{nameof(stringToCheck)}' cannot be null or whitespace.", nameof(stringToCheck));
            }

            return format switch
            {
                Format.Number => double.TryParse(stringToCheck, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out var _) || long.TryParse(SanitizeHex(stringToCheck.AsSpan(0)), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out var _),
                Format.Date => DateTime.TryParse(stringToCheck, out var _),
                Format.TimeSpan => TimeSpan.TryParse(stringToCheck, out var _),
                _ => throw new FormatNotAllowedException("Format not allowed.")
            };
        }

        private static ReadOnlySpan<char> SanitizeHex(ReadOnlySpan<char> span) => span.StartsWith("0x") ? span.TrimStart("0x") : span;
    }
}
