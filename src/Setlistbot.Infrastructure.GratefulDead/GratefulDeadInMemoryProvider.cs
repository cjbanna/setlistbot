using Newtonsoft.Json;
using Setlistbot.Infrastructure.GratefulDead.Extensions;
using domain = Setlistbot.Domain;

namespace Setlistbot.Infrastructure.GratefulDead
{
    public class GratefulDeadInMemoryProvider : domain.ISetlistProvider
    {
        private static readonly Lazy<IEnumerable<Setlist>> Setlists = new Lazy<
            IEnumerable<Setlist>
        >(LoadSetlists);

        private static IEnumerable<Setlist> LoadSetlists()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "gd-shows.json");
            return JsonConvert.DeserializeObject<IEnumerable<Setlist>>(File.ReadAllText(filePath))
                ?? Enumerable.Empty<Setlist>();
        }

        public string ArtistId => "gd";

        public async Task<IEnumerable<domain.Setlist>> GetSetlists(DateOnly date)
        {
            return await Task.FromResult(
                Setlists
                    .Value.Where(s => DateOnly.FromDateTime(s.ShowDate) == date)
                    .Select(s => s.ToSetlist())
            );
        }
    }

    public record Setlist
    {
        public DateTime ShowDate { get; init; }
        public string Venue { get; init; } = string.Empty;
        public string Location { get; init; } = string.Empty;
        public string SpotifyUrl { get; set; } = string.Empty;
        public IEnumerable<Set> Sets { get; init; } = Enumerable.Empty<Set>();
    }

    public record Set
    {
        public string Name { get; init; } = string.Empty;
        public IEnumerable<Song> Songs { get; init; } = Enumerable.Empty<Song>();
    }

    public record Song
    {
        public string Name { get; init; } = string.Empty;
        public bool Segue { get; init; }
    }
}
