using Microsoft.Extensions.DependencyInjection;
using Setlistbot.Domain;

namespace Setlistbot.Application
{
    public sealed class ReplyBuilderFactory : IReplyBuilderFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public ReplyBuilderFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IReplyBuilder Get(string artistId)
        {
            var replyBuilders = _serviceProvider.GetServices<IReplyBuilder>();
            return replyBuilders.FirstOrDefault(p => p.ArtistId == artistId)
                ?? throw new Exception($"Could not resolve IReplyBuilder for {artistId}");
        }
    }
}
