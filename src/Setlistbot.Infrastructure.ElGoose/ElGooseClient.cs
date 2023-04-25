using EnsureThat;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.ElGoose.Options;

namespace Setlistbot.Infrastructure.ElGoose
{
    public class ElGooseClient : IElGooseClient
    {
        private readonly ILogger<ElGooseClient> _logger;
        private readonly ElGooseOptions _options;

        public ElGooseClient(ILogger<ElGooseClient> logger, IOptions<ElGooseOptions> options)
        {
            _logger = logger;
            _options = Ensure.Any.IsNotNull(options, nameof(options)).Value;
        }

        public async Task<SetlistResponse> GetSetlistAsync(DateTime date)
        {
            try
            {
                // https://elgoose.net/api/v1/setlists/showdate/2023-04-14.json

                var url = Url.Combine(
                    _options.BaseUrl,
                    "setlists",
                    "showdate",
                    date.ToString("yyyy-MM-dd") + ".json"
                );

                var response = await url.GetJsonAsync<SetlistResponse>();

                if (!string.IsNullOrEmpty(response.ErrorMessage))
                {
                    _logger.LogError(
                        "Error retrieving setlist: {ErrorMessage}",
                        response.ErrorMessage
                    );
                }

                return response;
            }
            catch (FlurlHttpException ex)
            {
                var body = await ex.GetResponseStringAsync();
                _logger.LogError(body);
                throw;
            }
        }
    }
}
