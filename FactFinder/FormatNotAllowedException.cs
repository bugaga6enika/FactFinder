namespace FactFinder
{
    public class FormatNotAllowedException : Exception
    {
        public FormatNotAllowedException(string? message) : base(message)
        {
        }
    }
}
