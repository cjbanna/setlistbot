using EnsureThat;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.Phish.Options;

namespace Setlistbot.Infrastructure.PhishNet
{
    public class PhishNetClient : IPhishNetClient
    {
        private readonly ILogger<PhishNetClient> _logger;
        private readonly PhishNetOptions _options;

        public PhishNetClient(ILogger<PhishNetClient> logger, IOptions<PhishNetOptions> options)
        {
            _logger = logger;
            _options = Ensure.Any.IsNotNull(options, nameof(options)).Value;
        }

        public async Task<SetlistResponse> GetSetlistAsync(DateTime date)
        {
            try
            {
                // https://api.phish.net/v5/setlists/showdate/1997-11-22.json?apikey=YourApiKey

                var url = Url.Combine(
                        _options.BaseUrl,
                        "setlists",
                        "showdate",
                        date.ToString("yyyy-MM-dd") + ".json"
                    )
                    .SetQueryParam("apikey", _options.ApiKey);

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
