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

    public record ArtistName(NonEmptyString Name);
}
