﻿using EnsureThat;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Setlistbot.Infrastructure.KglwNet.Options;

namespace Setlistbot.Infrastructure.KglwNet
{
    public class KglwNetClient : IKglwNetClient
    {
        private readonly ILogger<KglwNetClient> _logger;
        private readonly KglwNetOptions _options;

        public KglwNetClient(ILogger<KglwNetClient> logger, IOptions<KglwNetOptions> options)
        {
            _logger = logger;
            _options = Ensure.Any.IsNotNull(options, nameof(options)).Value;
        }

        public async Task<SetlistResponse> GetSetlistAsync(DateTime date)
        {
            try
            {
                // https://kglw.songfishapp.com/api/v1/setlists/showdate/2022-10-10.json

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
