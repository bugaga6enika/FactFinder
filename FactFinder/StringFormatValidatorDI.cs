namespace FactFinder
{
    public static class StringFormatValidatorDI
    {
        private static readonly IDictionary<string, Func<string, bool>> _allowedFormatValidators = new Dictionary<string, Func<string, bool>>()
        {
            [NumberFormat] = NumberFormatValidator,
            [DateFormat] = DateFormatValidator,
            [TimeSpanFormat] = TimeSpanFormatValidator
        };

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
        /// <exception cref="Exception"></exception>
        /// <exception cref="FormatNotAllowedException"></exception>
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

            try
            {
                if (_allowedFormatValidators.TryGetValue(sanitizedFormat, out var validator))
                {
                    return validator(stringToCheck);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Unknown format.", e);
            }

            throw new FormatNotAllowedException("Format not allowed.");
        }

        private static bool NumberFormatValidator(string numberAsString)
            => double.TryParse(numberAsString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out var _) || long.TryParse(SanitizeHex(numberAsString.AsSpan(0)), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out var _);

        private static bool DateFormatValidator(string dateAsString) => DateTime.TryParse(dateAsString, out var _);

        private static bool TimeSpanFormatValidator(string timespanAsString) => TimeSpan.TryParse(timespanAsString, out var _);

        private static ReadOnlySpan<char> SanitizeHex(ReadOnlySpan<char> span) => span.StartsWith("0x") ? span.TrimStart("0x") : span;
    }
}
