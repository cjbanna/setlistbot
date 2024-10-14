using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Domain
{
    [StronglyTyped(Template.String)]
    public readonly partial struct ArtistId
    {
        public ArtistId(NonEmptyString value) => _value = value;
    }

    [StronglyTyped(Template.String)]
    public readonly partial struct ArtistName
    {
        public ArtistName(NonEmptyString value) => _value = value;
    }
}
