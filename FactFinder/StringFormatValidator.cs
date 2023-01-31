namespace FactFinder
{
    public static class StringFormatValidator
    {
        private static readonly string[] _allowedFormats = new string[3] { NumberFormat, DateFormat, TimeSpanFormat };

        public const string NumberFormat = "number";
        public const string DateFormat = "date";
        public const string TimeSpanFormat = "timespan";

        /// <summary>
        /// Checks if given string can be converted to the given format
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatNotAllowedException"></exception>
        /// <exception cref="Exception"></exception>
        public static bool IsFormat(string stringToCheck, string format)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentException($"'{nameof(stringToCheck)}' cannot be null or whitespace.", nameof(stringToCheck));
            }

            if (string.IsNullOrWhiteSpace(format))
            {
                throw new ArgumentException($"'{nameof(format)}' cannot be null or whitespace.", nameof(format));
            }

            var sanitizedFormat = format.ToLower();

            if (!_allowedFormats.Contains(sanitizedFormat))
            {
                throw new FormatNotAllowedException("Format not allowed.");
            }

            return sanitizedFormat switch
            {
                NumberFormat => double.TryParse(stringToCheck, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out var _) || long.TryParse(SanitizeHex(stringToCheck.AsSpan(0)), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out var _),
                DateFormat => DateTime.TryParse(stringToCheck, out var _),
                TimeSpanFormat => TimeSpan.TryParse(stringToCheck, out var _),
                _ => throw new Exception("Unknown format.") // should never been reached
            };
        }

        private static ReadOnlySpan<char> SanitizeHex(ReadOnlySpan<char> span) => span.StartsWith("0x") ? span.TrimStart("0x") : span;
    }
}
