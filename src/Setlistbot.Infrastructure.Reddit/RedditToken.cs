using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.Reddit
{
    public readonly record struct RedditToken
    {
        private readonly NonEmptyString _value;

        [Obsolete("Don't use the default constructor", true)]
        public RedditToken() => throw new NotImplementedException();

        public RedditToken(NonEmptyString value) => _value = value;

        public static implicit operator string(RedditToken value) => value._value;
    }
}
