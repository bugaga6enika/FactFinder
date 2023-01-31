namespace FactFinder.Validators
{
    public static class NumberValidator
    {
        public const string Name = "number";

        public static bool CanBeParsed(string numberAsString)
           => double.TryParse(numberAsString, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.CurrentCulture, out var _) || long.TryParse(SanitizeHex(numberAsString.AsSpan(0)), System.Globalization.NumberStyles.AllowHexSpecifier, System.Globalization.CultureInfo.CurrentCulture, out var _);

        private static ReadOnlySpan<char> SanitizeHex(ReadOnlySpan<char> span) => span.StartsWith("0x") ? span.TrimStart("0x") : span;
    }
}
