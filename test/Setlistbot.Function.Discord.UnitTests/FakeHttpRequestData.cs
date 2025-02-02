using System.Security.Claims;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace Setlistbot.Function.Discord.UnitTests
{
    public sealed class FakeHttpRequestData : HttpRequestData
    {
        public FakeHttpRequestData(
            FunctionContext functionContext,
            Stream? body = null,
            Uri? url = null
        )
            : base(functionContext)
        {
            Url = url ?? new Uri("https://localhost");
            Body = body ?? new MemoryStream();
        }

        public override Stream Body { get; } = new MemoryStream();

        public override HttpHeadersCollection Headers { get; } = new HttpHeadersCollection();

        public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = new List<IHttpCookie>();

        public override Uri Url { get; }

        public override IEnumerable<ClaimsIdentity> Identities { get; } =
            Enumerable.Empty<ClaimsIdentity>();

        public override string Method { get; } = string.Empty;

        public override HttpResponseData CreateResponse()
        {
            return new FakeHttpResponseData(FunctionContext);
        }
    }
}
