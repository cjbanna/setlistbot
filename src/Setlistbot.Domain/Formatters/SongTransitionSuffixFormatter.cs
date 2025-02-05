﻿namespace Setlistbot.Domain.Formatters
{
    public sealed class SongTransitionSuffixFormatter(SongTransition songTransition) : IFormatter
    {
        public string Format() =>
            (
                songTransition switch
                {
                    SongTransition.Stop => new CombinedFormatter(
                        new SongTransitionFormatter(songTransition),
                        new SpaceFormatter()
                    ),
                    _ => new CombinedFormatter(
                        new SpaceFormatter(),
                        new SongTransitionFormatter(songTransition),
                        new SpaceFormatter()
                    ),
                }
            ).Format();
    }
}
