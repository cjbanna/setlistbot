using EnsureThat;

namespace Setlistbot.Domain
{
    public sealed record ArtistId
    {
        private readonly string _artistId = string.Empty;

        public ArtistId(string artistId)
        {
            _artistId = EnsureArg.IsNotNullOrWhiteSpace(artistId, nameof(artistId)).ToLower();
        }

        public static implicit operator string(ArtistId artistId) => artistId._artistId;
    }

    public record ArtistName(NonEmptyString Name)
    {
        public static implicit operator string(ArtistName artistName) => artistName.Name;
    }
}
