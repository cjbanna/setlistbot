using System.Text.Json;

namespace Setlistbot.Infrastructure.KglwNet.UnitTests
{
    public static class TestData
    {
        public static SetlistResponse GetSetlistResponseTestData()
        {
            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "KglwNetResponses/2022-10-10-setlist-response.json"
            );
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<SetlistResponse>(json)!;
        }
    }
}
