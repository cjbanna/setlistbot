using Newtonsoft.Json;

namespace Setlistbot.Infrastructure.ElGoose.UnitTests
{
    public class TestData
    {
        public static SetlistResponse GetSetlistResponseTestData()
        {
            var filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "ElGooseResponses/2023-04-14-setlist-response.json"
            );
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<SetlistResponse>(json)!;
        }
    }
}
