namespace FactFinder.Converters
{
    public class StringConverterValidatorExt
    {
        private readonly IDictionary<Format, Func<string, bool>> _allowedFormatValidators;

        public StringConverterValidatorExt(IDictionary<Format, Func<string, bool>> allowedFormatValidators)
        {
            _allowedFormatValidators = allowedFormatValidators ?? throw new ArgumentNullException(nameof(allowedFormatValidators));
        }

        /// <summary>
        /// Checks if given string can be converted to the given format
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="FormatNotAllowedException"></exception>
        public bool CanBeConverted(string stringToCheck, Format format)
        {
            if (string.IsNullOrWhiteSpace(stringToCheck))
            {
                throw new ArgumentException($"'{nameof(stringToCheck)}' cannot be null or whitespace.", nameof(stringToCheck));
            }

            if (_allowedFormatValidators.TryGetValue(format, out var validator))
            {
                return validator(stringToCheck);
            }

            throw new FormatNotAllowedException("Format not allowed.");
        }
    }
}
