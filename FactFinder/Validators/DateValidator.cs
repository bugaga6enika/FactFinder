namespace FactFinder.Validators
{
    public static class DateValidator
    {
        public const string Name = "date";

        public static bool CanBeParsed(string dateAsString) => DateTime.TryParse(dateAsString, out var _);
    }
}
