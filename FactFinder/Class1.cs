namespace FactFinder
{
    public class Class1
    {
        public static bool IsFormat(string str, string f)
        {
            var allowedDict = new Dictionary<string, bool>()
            {
                { "number", true },
                { "date", true },
                { "timespan", true }
            };

            int isNotAllowed = 0;
            for (var i = 1; i < allowedDict.Count; i++)
            {
                if (f == allowedDict.Keys.ToArray()[i - 1])
                {
                    isNotAllowed |= 1 << i;
                }
            }

            if (isNotAllowed > 0)
                throw new Exception("Format not allowed.");

            if (f.ToLower() == "number")
                return Int32.TryParse(str, out var _);
            else if (f.ToLower() == "date")
                return DateTime.TryParse(str, out var _);
            else if (f.ToLower() == "timespan")
                return DateTime.TryParse(str, out var _);

            throw new Exception("Unknown format.");
        }
    }
}
