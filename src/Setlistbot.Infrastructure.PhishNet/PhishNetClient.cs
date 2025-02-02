using CSharpFunctionalExtensions;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.PhishNet.Options;

namespace Setlistbot.Infrastructure.PhishNet
{
    public sealed class PhishNetClient : IPhishNetClient
    {
        private readonly ILogger<PhishNetClient> _logger;
        private readonly IOptions<PhishNetOptions> _options;

        public PhishNetClient(ILogger<PhishNetClient> logger, IOptions<PhishNetOptions> options)
        {
            _logger = logger;
            _options = options;
        }

        public async Task<Result<SetlistResponse>> GetSetlistAsync(DateOnly date)
        {
            var url = "";

            try
            {
                // https://api.phish.net/v5/setlists/showdate/1997-11-22.json?apikey=YourApiKey

                url = Url.Combine(
                        _options.Value.BaseUrl,
                        "setlists",
                        "showdate",
                        date.ToString("yyyy-MM-dd") + ".json"
                    )
                    .SetQueryParam("apikey", _options.Value.ApiKey);

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
                _logger.LogError("Phish call failed. Url: {Url} {Body}", url, body);
                throw;
            }
        }
    }
}
