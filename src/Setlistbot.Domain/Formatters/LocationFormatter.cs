namespace Setlistbot.Domain.Formatters
{
    public sealed class LocationFormatter(Location location) : IFormatter
    {
        public string Format() =>
            new SeparatedFormatter(
                new CombinedFormatter(new CharacterFormatter(','), new SpaceFormatter()),
                new IFormatter[]
                {
                    new RemoveEmptyFormatter(
                        new IFormatter[]
                        {
                            new MaybeFormatter<Venue>(location.Venue),
                            new LiteralFormatter(location.City),
                            new MaybeFormatter<State>(location.State),
                            new LiteralFormatter(location.Country),
                        }
                    ),
                }
            ).Format();
    }
}
