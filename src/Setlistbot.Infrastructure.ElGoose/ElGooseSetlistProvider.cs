using Microsoft.Extensions.Logging;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.ElGoose
{
    public class ElGooseSetlistProvider : ISetlistProvider
    {
        private readonly ILogger<ElGooseSetlistProvider> _logger;
        private readonly IElGooseClient _elGooseClient;

        public ElGooseSetlistProvider(
            ILogger<ElGooseSetlistProvider> logger,
            IElGooseClient elGooseClient
        )
        {
            _logger = logger;
            _elGooseClient = elGooseClient;
        }

        public string ArtistId => "goose";

        public async Task<IEnumerable<Setlist>> GetSetlists(DateTime date)
        {
            try
            {
                var setlistResponse = await _elGooseClient.GetSetlistAsync(date);

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

                var gooseSetlists = setlistResponse.Data
                    .Where(s => s.ArtistName == "Goose")
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

                foreach (var showGrouping in gooseSetlists)
                {
                    var location = $"{showGrouping.Key.City}, {showGrouping.Key.State}";

                    var setlist = Setlist.NewSetlist(
                        "goose",
                        "Goose",
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
                        var setName = setGrouping.Key == "Set e" ? "Encore" : setGrouping.Key;

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
                    "Failed to process ElGoose response {@SetlistResponse}",
                    setlistResponse
                );
                throw;
            }
        }
    }
}
