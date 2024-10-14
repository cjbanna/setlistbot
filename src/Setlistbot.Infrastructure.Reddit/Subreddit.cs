using Setlistbot.Domain;
using StronglyTypedPrimitives.Attributes;

namespace Setlistbot.Infrastructure.Reddit
{
    [StronglyTyped(Template.String)]
    public readonly partial struct Subreddit
    {
        public Subreddit(NonEmptyString value) => _value = value;
    }
}
