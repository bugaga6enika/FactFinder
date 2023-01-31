namespace FactFinder
{
    public class StringFormatValidatorExt
    {
        private readonly IDictionary<string, Func<string, bool>> _allowedFormatValidators;

        public StringFormatValidatorExt(IDictionary<string, Func<string, bool>> validators)
        {
            _allowedFormatValidators = validators ?? throw new ArgumentNullException(nameof(validators));
        }

        /// <summary>
        /// Checks if given string can be converted to the given format
        /// </summary>
        /// <param name="stringToCheck"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="FormatNotAllowedException"></exception>
        public bool IsFormat(string stringToCheck, string format)
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

            if (_allowedFormatValidators.TryGetValue(sanitizedFormat, out var validator))
            {
                return validator(stringToCheck);
            }

            throw new FormatNotAllowedException("Format not allowed.");
        }
    }
}
