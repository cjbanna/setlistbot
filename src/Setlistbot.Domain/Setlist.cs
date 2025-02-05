﻿using CSharpFunctionalExtensions;

namespace Setlistbot.Domain
{
    public sealed class Setlist
    {
        private readonly Dictionary<SetName, Set> _sets = [];

        public IReadOnlyList<Set> Sets => _sets.Values.ToList();
        public ArtistId ArtistId { get; private set; }
        public ArtistName ArtistName { get; private set; }
        public DateOnly Date { get; private set; }
        public Location Location { get; private set; } = default!;
        public TimeSpan Duration =>
            _sets.Aggregate(TimeSpan.Zero, (acc, s) => acc + s.Value.Duration);
        public string Notes { get; private set; } = string.Empty;
        public Maybe<string> SpotifyUrl { get; private set; }
        public Maybe<string> Permalink { get; private set; }

        public static Setlist NewSetlist(
            ArtistId artistId,
            ArtistName artistName,
            DateOnly showDate,
            Location location,
            string notes
        )
        {
            return new Setlist
            {
                ArtistId = artistId,
                ArtistName = artistName,
                Date = showDate,
                Location = location,
                Notes = notes,
            };
        }

        public Result AddSet(Set set) =>
            !_sets.TryAdd(set.Name, set) ? Result.Failure("Set already exists.") : Result.Success();

        public void AddSets(IEnumerable<Set> sets)
        {
            foreach (var set in sets)
            {
                AddSet(set);
            }
        }

        public void AddSpotifyUrl(Uri url)
        {
            SpotifyUrl = url.ToString();
        }

        public void AddPermalink(Uri url)
        {
            Permalink = url.ToString();
        }
    }
}
