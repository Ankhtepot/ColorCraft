namespace Extensions
{
    public static class StringExtensions
    {
        public static string SanitizeFilePath(this string path)
        {
            return path.Replace(@"/", "\\")
                .Replace(@"\r", "")
                .Replace(@"\n", "");
        }
    }
}