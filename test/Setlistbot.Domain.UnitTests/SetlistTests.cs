namespace Setlistbot.Domain.UnitTests
{
    public sealed class SetlistTests
    {
        [Fact]
        public void NewSetlist_WhenCalled_ExpectSetlist()
        {
            // Arrange
            var artistId = ArtistId.From("phish");
            var artistName = ArtistName.From("Phish");
            var date = new DateOnly(2021, 1, 1);
            var location = new Location(
                Venue.From("Venue"),
                City.From("City"),
                State.From("State"),
                Country.From("Country")
            );
            const string notes = "Notes";

            // Act
            var setlist = Setlist.NewSetlist(artistId, artistName, date, location, notes);

            // Assert
            Assert.Equal(artistId, setlist.ArtistId);
            Assert.Equal(artistName, setlist.ArtistName);
            Assert.Equal(date, setlist.Date);
            Assert.Equal(location, setlist.Location);
            Assert.Equal(notes, setlist.Notes);
        }

        [Fact]
        public void AddSet_WhenCalled_ExpectSetAdded()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(2021, 1, 1),
                new Location(
                    Venue.From("Venue"),
                    City.From("City"),
                    State.From("State"),
                    Country.From("Country")
                ),
                "Notes"
            );
            var set = new Set(SetName.From("Set"));

            // Act
            setlist.AddSet(set);

            // Assert
            Assert.Contains(set, setlist.Sets);
        }

        [Fact]
        public void AddSets_WhenCalled_ExpectAdded()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(2021, 1, 1),
                new Location(
                    Venue.From("Venue"),
                    City.From("City"),
                    State.From("State"),
                    Country.From("Country")
                ),
                "Notes"
            );

            var set1 = new Set(SetName.From("Set 1"));
            var set2 = new Set(SetName.From("Set 2"));

            // Act
            setlist.AddSets(new[] { set1, set2 });

            // Assert
            Assert.Contains(set1, setlist.Sets);
        }

        [Fact]
        public void Add_WhenDuplicateSet_ExpectException()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(2021, 1, 1),
                new Location(
                    Venue.From("Venue"),
                    City.From("City"),
                    State.From("State"),
                    Country.From("Country")
                ),
                "Notes"
            );
            var set = new Set(SetName.From("Set"));
            setlist.AddSet(set);

            var duplicateSet = new Set(SetName.From("Set"));

            // Act
            var result = setlist.AddSet(duplicateSet);

            // Assert
            Assert.True(result.IsFailure);
        }

        [Fact]
        public void AddSpotifyUrl_WhenCalled_ExpectSpotifyUrlAdded()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(2021, 1, 1),
                new Location(
                    Venue.From("Venue"),
                    City.From("City"),
                    State.From("State"),
                    Country.From("Country")
                ),
                "Notes"
            );
            var url = new Uri("https://open.spotify.com/playlist/37i9dQZF1DZ06evO3fq5Og");

            // Act
            setlist.AddSpotifyUrl(url);

            // Assert
            Assert.Equal(url.ToString(), setlist.SpotifyUrl.Value);
        }

        [Fact]
        public void AddPermalink_WhenCalled_ExpectPermalinkAdded()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(2021, 1, 1),
                new Location(
                    Venue.From("Venue"),
                    City.From("City"),
                    State.From("State"),
                    Country.From("Country")
                ),
                "Notes"
            );
            var url = new Uri("https://phish.in/2021-01-01");

            // Act
            setlist.AddPermalink(url);

            // Assert
            Assert.Equal(url.ToString(), setlist.Permalink.Value);
        }

        [Fact]
        public void Duration_WhenSetsHaveDuration_ExpectTotalDuration()
        {
            // Arrange
            var setlist = Setlist.NewSetlist(
                ArtistId.From("phish"),
                ArtistName.From("Phish"),
                new DateOnly(2021, 1, 1),
                new Location(
                    Venue.From("Venue"),
                    City.From("City"),
                    State.From("State"),
                    Country.From("Country")
                ),
                "Notes"
            );
            var set1 = new Set(SetName.From("Set 1"));
            set1.AddSong(
                new Song(
                    SongName.From("Song 1"),
                    SongPosition.From(1),
                    SongTransition.Stop,
                    TimeSpan.FromMinutes(5),
                    string.Empty
                )
            );
            set1.AddSong(
                new Song(
                    SongName.From("Song 2"),
                    SongPosition.From(2),
                    SongTransition.Stop,
                    TimeSpan.FromMinutes(5),
                    string.Empty
                )
            );
            var set2 = new Set(SetName.From("Set 2"));
            set2.AddSong(
                new Song(
                    SongName.From("Song 3"),
                    SongPosition.From(3),
                    SongTransition.Stop,
                    TimeSpan.FromMinutes(5),
                    string.Empty
                )
            );
            set2.AddSong(
                new Song(
                    SongName.From("Song 4"),
                    SongPosition.From(4),
                    SongTransition.Stop,
                    TimeSpan.FromMinutes(5),
                    string.Empty
                )
            );
            setlist.AddSets(new[] { set1, set2 });

            // Act
            var duration = setlist.Duration;

            // Assert
            Assert.Equal(20, duration.TotalMinutes);
        }
    }
}
