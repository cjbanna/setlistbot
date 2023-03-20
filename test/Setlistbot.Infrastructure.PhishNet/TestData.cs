using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public class TestData
    {
        public static SetlistResponse GetSetlistResponseTestData()
        {
            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "PhishNetResponses/1997-11-22-setlist-response.json"
            );
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SetlistResponse>(json);
        }
    }
}
