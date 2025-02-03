using System.Text.Json;
using System.Text.Json.Serialization;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public static class TestData
    {
        public static SetlistResponse GetSetlistResponseTestData()
        {
            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "PhishNetResponses/1997-11-22-setlist-response.json"
            );
            var json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<SetlistResponse>(
                json,
                new JsonSerializerOptions
                {
                    NumberHandling = JsonNumberHandling.AllowReadingFromString,
                }
            )!;
        }
    }
}
