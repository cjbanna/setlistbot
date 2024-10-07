using System.Text;
using Microsoft.Azure.Functions.Worker.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Setlistbot.Function.Discord.Extensions
{
    public static class HttpRequestDataExtensions
    {
        private static readonly CamelCasePropertyNamesContractResolver ContractResolver =
            new CamelCasePropertyNamesContractResolver();

        private static readonly JsonSerializerSettings SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = ContractResolver,
                NullValueHandling = NullValueHandling.Ignore,
            };

        /// <summary>
        /// Deserializes the HTTP request body as JSON using Newtonsoft.Json.
        /// </summary>
        /// <returns>The deserialized object from the JSON string.</returns>
        public static async Task<T?> DeserializeJsonBodyAsync<T>(this HttpRequestData request)
        {
            var body = await request.ReadAsStringAsync();
            if (string.IsNullOrEmpty(body))
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(body, SerializerSettings);
        }

        /// <summary>
        /// Serializes the object as JSON using Newtonsoft.Json and writes it to the HTTP response body.
        /// </summary>
        public static async Task SerializeJsonBodyAsync(
            this HttpResponseData httpResponse,
            object value
        )
        {
            var responseBody = JsonConvert.SerializeObject(value, SerializerSettings);
            httpResponse.Headers.Add("Content-Type", "application/json");

            httpResponse.Body.Seek(0, SeekOrigin.Begin);
            await httpResponse.Body.WriteAsync(Encoding.UTF8.GetBytes(responseBody));
        }
    }
}
