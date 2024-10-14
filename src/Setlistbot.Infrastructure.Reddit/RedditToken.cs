using Setlistbot.Domain;
using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Infrastructure.Reddit
{
    [StronglyTyped(Template.String)]
    public readonly partial struct RedditToken
    {
        public RedditToken(NonEmptyString value) => _value = value;
    }
}
