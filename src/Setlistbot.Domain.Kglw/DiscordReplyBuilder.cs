﻿using System.Text;
using Setlistbot.Domain.Formatters;

namespace Setlistbot.Domain.Kglw
{
    public sealed class DiscordReplyBuilder : IReplyBuilder
    {
        public string ArtistId => "kglw";

        public string Build(IEnumerable<Setlist> setlists)
        {
            var enumerable = setlists as Setlist[] ?? setlists.ToArray();
            IFormatter formatter = enumerable switch
            {
                { Length: 1 } => new DiscordFormatter(enumerable[0]),
                _ => new EmptyFormatter(),
            };
            return formatter.Format();
        }
    }
}
