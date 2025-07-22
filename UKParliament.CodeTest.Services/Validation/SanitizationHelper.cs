using Ganss.Xss;

namespace UKParliament.CodeTest.Services.Validation
{
    public static class SanitizationHelper
    {
        private static readonly HtmlSanitizer sanitizer = new HtmlSanitizer();

        public static string Sanitize(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;
            return sanitizer.Sanitize(input);
        }
    }
}
