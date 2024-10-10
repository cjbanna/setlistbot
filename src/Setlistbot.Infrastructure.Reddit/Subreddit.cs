using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.Reddit
{
    public readonly record struct Subreddit
    {
        private readonly NonEmptyString _value;

        [Obsolete("Don't use the default constructor", true)]
        public Subreddit() => throw new NotImplementedException();

        public Subreddit(NonEmptyString value) => _value = value;

        public static implicit operator string(Subreddit value) => value._value;
    }
}
