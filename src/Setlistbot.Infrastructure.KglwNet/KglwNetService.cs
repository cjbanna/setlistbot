using Microsoft.Extensions.Logging;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.KglwNet
{
    public class KglwNetService : ISetlistProvider
    {
        private readonly ILogger<KglwNetService> _logger;
        private readonly IKglwNetClient _kglwNetClient;

        public KglwNetService(ILogger<KglwNetService> logger, IKglwNetClient kglwNetClient)
        {
            _logger = logger;
            _kglwNetClient = kglwNetClient;
        }

        public async Task<IEnumerable<Setlist>> GetSetlistsAsync(DateTime date)
        {
            var setlistResponse = await _kglwNetClient.GetSetlistAsync(date);

            var setlists = new List<Setlist>();

            if (setlistResponse == null || setlistResponse.Data == null)
            {
                return setlists;
            }

            var kglwSetlists = setlistResponse.Data
                .Where(s => s.ArtistName == "King Gizzard & the Lizard Wizard")
                .GroupBy(
                    s =>
                        new
                        {
                            s.ShowDate,
                            s.Venue,
                            s.City,
                            s.State,
                            s.Country,
                            s.ShowNotes
                        }
                )
                .OrderBy(s => s.Key.ShowDate)
                .ToArray();

            foreach (var showGrouping in kglwSetlists)
            {
                var location = $"{showGrouping.Key.City}, {showGrouping.Key.State}";

                var setlist = Setlist.NewSetlist(
                    "kglw",
                    "King Gizzard & the Lizard Wizard",
                    DateTime.Parse(showGrouping.Key.ShowDate),
                    new Location(
                        showGrouping.Key.Venue,
                        showGrouping.Key.City,
                        showGrouping.Key.State,
                        showGrouping.Key.Country
                    ),
                    showGrouping.Key.ShowNotes
                );

                foreach (var setGrouping in showGrouping.GroupBy(x => x.SetType))
                {
                    var set = new Set(setGrouping.Key);

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
