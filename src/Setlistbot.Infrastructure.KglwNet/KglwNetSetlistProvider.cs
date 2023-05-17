using Microsoft.Extensions.Logging;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.KglwNet
{
    public class KglwNetSetlistProvider : ISetlistProvider
    {
        private readonly ILogger<KglwNetSetlistProvider> _logger;
        private readonly IKglwNetClient _kglwNetClient;

        public KglwNetSetlistProvider(
            ILogger<KglwNetSetlistProvider> logger,
            IKglwNetClient kglwNetClient
        )
        {
            _logger = logger;
            _kglwNetClient = kglwNetClient;
        }

        public string ArtistId => "kglw";

        public async Task<IEnumerable<Setlist>> GetSetlists(DateTime date)
        {
            try
            {
                var setlistResponse = await _kglwNetClient.GetSetlistAsync(date);

                if (setlistResponse == null || setlistResponse.Data == null)
                {
                    return Enumerable.Empty<Setlist>();
                }

                return GetSetlistsFromResponse(setlistResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get setlists for {Date}", date);
                throw;
            }
        }

        private IEnumerable<Setlist> GetSetlistsFromResponse(SetlistResponse setlistResponse)
        {
            try
            {
                var setlists = new List<Setlist>();

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
                                s.ShowNotes,
                                s.Permalink
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

                    foreach (
                        var setGrouping in showGrouping.GroupBy(x => $"{x.SetType} {x.SetNumber}")
                    )
                    {
                        var setName = setGrouping.Key.Contains(
                            "One Set",
                            StringComparison.CurrentCultureIgnoreCase
                        )
                            ? "One Set"
                            : setGrouping.Key;

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
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to process KglwNet response {@SetlistResponse}",
                    setlistResponse
                );
                throw;
            }
        }
    }
}
