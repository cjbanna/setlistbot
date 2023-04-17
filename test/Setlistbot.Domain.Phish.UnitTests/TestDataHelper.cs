namespace Setlistbot.Domain.Phish.UnitTests
{
    public class TestDataHelper
    {
        public static string GetTestData(string relativePath)
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            // Replace LF with environment specific line endings to make tests pass on Windows
            return File.ReadAllText(filePath).Replace("\n", Environment.NewLine);
        }
    }
}
