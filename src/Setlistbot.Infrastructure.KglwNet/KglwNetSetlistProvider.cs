using CSharpFunctionalExtensions;
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

        public async Task<IEnumerable<Setlist>> GetSetlists(DateOnly date)
        {
            try
            {
                var response = await _kglwNetClient.GetSetlistAsync(date);
                return response.Match(GetSetlistsFromResponse, () => []);
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

                var kglwSetlists = setlistResponse
                    .Data.Where(s => s.ArtistName == "King Gizzard & the Lizard Wizard")
                    .GroupBy(s => new
                    {
                        s.ShowDate,
                        s.Venue,
                        s.City,
                        s.State,
                        s.Country,
                        s.ShowNotes,
                        s.Permalink,
                    })
                    .OrderBy(s => s.Key.ShowDate)
                    .ToArray();

                foreach (var showGrouping in kglwSetlists)
                {
                    var location = $"{showGrouping.Key.City}, {showGrouping.Key.State}";

                    var setlist = Setlist.NewSetlist(
                        Domain.ArtistId.From("kglw"),
                        ArtistName.From("King Gizzard & the Lizard Wizard"),
                        DateOnly.Parse(showGrouping.Key.ShowDate),
                        new Location(
                            Venue.From(showGrouping.Key.Venue),
                            City.From(showGrouping.Key.City),
                            State.From(showGrouping.Key.State),
                            Country.From(showGrouping.Key.Country)
                        ),
                        showGrouping.Key.ShowNotes
                    );

                    var permalink = GetPermalink(showGrouping.Key.Permalink);
                    if (permalink != null)
                    {
                        setlist.AddPermalink(permalink);
                    }

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
                                TimeSpan.Zero,
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

        private Uri? GetPermalink(string permalink)
        {
            if (string.IsNullOrWhiteSpace(permalink))
            {
                return null;
            }

            return new Uri(permalink, UriKind.Relative);
        }
    }
}
