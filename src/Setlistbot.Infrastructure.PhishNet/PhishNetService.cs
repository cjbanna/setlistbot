using Microsoft.Extensions.Logging;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.PhishNet.UnitTests
{
    public class PhishNetService : ISetlistProvider
    {
        private readonly ILogger<PhishNetService> _logger;
        private readonly IPhishNetClient _phishNetClient;

        public PhishNetService(ILogger<PhishNetService> logger, IPhishNetClient phishNetClient)
        {
            _logger = logger;
            _phishNetClient = phishNetClient;
        }

        public async Task<IEnumerable<Setlist>> GetSetlistsAsync(DateTime date)
        {
            var setlistResponse = await _phishNetClient.GetSetlistAsync(date);

            var setlists = new List<Setlist>();

            if (setlistResponse == null || setlistResponse.Data == null)
            {
                return setlists;
            }

            var phishSetlists = setlistResponse.Data
                .Where(s => s.ArtistName == "Phish")
                .GroupBy(
                    s =>
                        new
                        {
                            s.ShowDate,
                            s.Venue,
                            s.City,
                            s.State,
                            s.Country,
                            s.SetlistNotes
                        }
                )
                .OrderBy(s => s.Key.ShowDate)
                .ToArray();

            foreach (var showGrouping in phishSetlists)
            {
                var location = $"{showGrouping.Key.City}, {showGrouping.Key.State}";

                var setlist = Setlist.NewSetlist(
                    "phish",
                    "Phish",
                    DateTime.Parse(showGrouping.Key.ShowDate),
                    new Location(
                        showGrouping.Key.Venue,
                        showGrouping.Key.City,
                        showGrouping.Key.State,
                        showGrouping.Key.Country
                    ),
                    showGrouping.Key.SetlistNotes
                );

                foreach (var setGrouping in showGrouping.GroupBy(x => x.Set))
                {
                    var setName = setGrouping.Key switch
                    {
                        "1" => "Set 1",
                        "2" => "Set 2",
                        "3" => "Set 3",
                        "4" => "Set 4",
                        "e" => "Encore",
                        "e2" => "Encore 2",
                        "e3" => "Encore 3",
                        _ => string.Empty,
                    };

                    var set = new Set(setName);

                    var orderedSongResponses = setGrouping
                        .OrderBy(x => x.Position)
                        .Select(
                            (x, i) =>
                                new
                                {
                                    x.Song,
                                    x.Transition,
                                    Index = i + 1,
                                    x.Footnote
                                }
                        );

                    foreach (var songResponse in orderedSongResponses)
                    {
                        var song = new Song(
                            songResponse.Song,
                            songResponse.Index,
                            songResponse.Transition,
                            default,
                            songResponse.Footnote
                        );
                        set.AddSong(song);
                    }

                    setlist.AddSet(set);
                }

                setlists.Add(setlist);
            }

            return setlists;
        }
    }
}
