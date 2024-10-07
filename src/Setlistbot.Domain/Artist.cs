using EnsureThat;

namespace Setlistbot.Domain
{
    public record ArtistId
    {
        private readonly string _artistId = string.Empty;

        public ArtistId(string artistId)
        {
            _artistId = EnsureArg.IsNotNullOrWhiteSpace(artistId, nameof(artistId)).ToLower();
        }

        public static implicit operator string(ArtistId artistId) => artistId._artistId;
    }

    public record ArtistName : StringNotNullOrWhiteSpace
    {
        public ArtistName(string value)
            : base(value) { }
    }

    public abstract record StringNotNullOrWhiteSpace
    {
        private readonly string _value = string.Empty;

        protected StringNotNullOrWhiteSpace(string value)
        {
            _value = EnsureArg.IsNotNullOrWhiteSpace(value, nameof(value));
        }

        public static implicit operator string(StringNotNullOrWhiteSpace value) => value._value;
    }
}
