namespace FactFinder.Validators
{
    public static class TimeSpanValidator
    {
        public const string Name = "timespan";
        
        public static bool CanBeParsed(string timespanAsString) => TimeSpan.TryParse(timespanAsString, out var _);
    }
}
