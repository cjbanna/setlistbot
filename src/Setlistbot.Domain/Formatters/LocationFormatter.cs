namespace Setlistbot.Domain.Formatters
{
    public sealed class LocationFormatter(Location location) : IFormatter
    {
        public string Format() =>
            new SeparatedFormatter(
                new CombinedFormatter(new CharacterFormatter(','), new SpaceFormatter()),
                [
                    new MaybeFormatter<Venue>(
                        location.Venue,
                        () => new LiteralFormatter(location.Venue.Value)
                    ),
                    new LiteralFormatter(location.City),
                    new MaybeFormatter<State>(
                        location.State,
                        () => new LiteralFormatter(location.State.Value)
                    ),
                    new LiteralFormatter(location.Country),
                ]
            ).Format();
    }
}
