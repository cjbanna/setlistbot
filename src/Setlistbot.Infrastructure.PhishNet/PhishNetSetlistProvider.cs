using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using Setlistbot.Domain;

namespace Setlistbot.Infrastructure.PhishNet
{
    public sealed class PhishNetSetlistProvider : ISetlistProvider
    {
        private readonly ILogger<PhishNetSetlistProvider> _logger;
        private readonly IPhishNetClient _phishNetClient;

        public PhishNetSetlistProvider(
            ILogger<PhishNetSetlistProvider> logger,
            IPhishNetClient phishNetClient
        )
        {
            _logger = logger;
            _phishNetClient = phishNetClient;
        }

        public string ArtistId => "phish";

        public async Task<IEnumerable<Setlist>> GetSetlists(DateOnly date)
        {
            try
            {
                var setlistResponse = await _phishNetClient.GetSetlistAsync(date);
                return setlistResponse.Match(GetSetlistsFromResponse, _ => []);
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

                var phishSetlists = setlistResponse
                    .Data.Where(s => s.ArtistName == "Phish")
                    .GroupBy(s => new
                    {
                        s.ShowDate,
                        s.Venue,
                        s.City,
                        s.State,
                        s.Country,
                        s.SetlistNotes,
                    })
                    .OrderBy(s => s.Key.ShowDate)
                    .ToArray();

                foreach (var showGrouping in phishSetlists)
                {
                    var setlist = Setlist.NewSetlist(
                        Domain.ArtistId.From("phish"),
                        ArtistName.From("Phish"),
                        DateOnly.Parse(showGrouping.Key.ShowDate),
                        new Location(
                            string.IsNullOrWhiteSpace(showGrouping.Key.Venue)
                                ? Maybe.None
                                : Maybe.From(Venue.From(showGrouping.Key.Venue)),
                            City.From(showGrouping.Key.City),
                            string.IsNullOrWhiteSpace(showGrouping.Key.State)
                                ? Maybe.None
                                : Maybe.From(State.From(showGrouping.Key.State)),
                            Country.From(showGrouping.Key.Country)
                        ),
                        showGrouping.Key.SetlistNotes
                    );

                    foreach (var setGrouping in showGrouping.GroupBy(x => x.Set))
                    {
                        var setName = GetSetName(setGrouping.Key);
                        var set = new Set(SetName.From(setName));

                        var orderedSongResponses = setGrouping
                            .OrderBy(x => x.Position)
                            .Select(
                                (x, i) =>
                                    new
                                    {
                                        x.Song,
                                        x.Transition,
                                        Index = i + 1,
                                        x.Footnote,
                                    }
                            );

                        foreach (var songResponse in orderedSongResponses)
                        {
                            var song = new Song(
                                SongName.From(songResponse.Song),
                                SongPosition.From(songResponse.Index),
                                songResponse.Transition.ToSongTransition(),
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
                    "Failed to process PhishNet response {@SetlistResponse}",
                    setlistResponse
                );
                throw;
            }
        }

        private static string GetSetName(string value)
        {
            return value switch
            {
                "1" => "Set 1",
                "2" => "Set 2",
                "3" => "Set 3",
                "4" => "Set 4",
                "e" => "Encore",
                "e2" => "Encore 2",
                "e3" => "Encore 3",
                _ => "Set",
            };
        }
    }
}
