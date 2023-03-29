using Microsoft.Extensions.Logging;
using Moq;
using Setlistbot.Application.Discord;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Application.UnitTests
{
    public class DiscordInteractionServiceTestFixture
    {
        public Mock<ILogger<DiscordInteractionService>> Logger { get; }
        public Mock<ISetlistProviderFactory> SetlistProviderFactory { get; }
        public Mock<IReplyBuilderFactory> ReplyBuilderFactory { get; }
        public DiscordInteractionService DiscordInteractionService { get; }

        public DiscordInteractionServiceTestFixture()
        {
            Logger = new Mock<ILogger<DiscordInteractionService>>();
            SetlistProviderFactory = new Mock<ISetlistProviderFactory>();
            ReplyBuilderFactory = new Mock<IReplyBuilderFactory>();

            DiscordInteractionService = new DiscordInteractionService(
                Logger.Object,
                SetlistProviderFactory.Object,
                ReplyBuilderFactory.Object
            );
        }
    }

    public class SetlistCommandHandlerTestFixture : DiscordInteractionServiceTestFixture
    {
        public SetlistCommandHandlerTestFixture()
            : base()
        {
            Setup();
        }

        private void Setup()
        {
            var date = new DateTime(1995, 12, 31);
            var location = new Location("Madison Square Garden", "New York", "NY", "USA");
            var setlists = new List<Setlist>
            {
                Setlist.NewSetlist("phish", "Phish", date, location, string.Empty)
            };

            var setlistProvider = new Mock<ISetlistProvider>();
            setlistProvider.Setup(p => p.GetSetlists(date)).ReturnsAsync(setlists);

            var replyBuilder = new Mock<IReplyBuilder>();
            replyBuilder.Setup(b => b.Build(setlists.First())).Returns("Some setlist");

            SetlistProviderFactory.Setup(f => f.Get("phish")).Returns(setlistProvider.Object);

            ReplyBuilderFactory.Setup(f => f.Get("phish")).Returns(replyBuilder.Object);
        }
    }

    public class DiscordInteractionServiceTests
    {
        [Fact]
        public async Task GetResponse_WhenPing_ExpectPong()
        {
            // Arrange
            var fixture = new DiscordInteractionServiceTestFixture();
            var interaction = new Interaction { InteractionType = InteractionType.Ping };

            // Act
            var response = await fixture.DiscordInteractionService.GetResponse(interaction);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(InteractionCallbackType.Pong, response.Type);
        }

        [Fact]
        public async Task GetResponse_WhenSetlistCommandAndSetlistFound_ExpectSetlist()
        {
            // Arrange
            var fixture = new SetlistCommandHandlerTestFixture();

            var interaction = new Interaction
            {
                InteractionType = InteractionType.ApplicationCommand,
                Data = new InteractionData
                {
                    Name = "setlist",
                    Options = new InteractionOption[]
                    {
                        new InteractionOption { Name = "artist", Value = "phish" },
                        new InteractionOption { Name = "date", Value = "1995-12-31" }
                    }
                }
            };

            // Act
            var response = await fixture.DiscordInteractionService.GetResponse(interaction);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(InteractionCallbackType.ChannelMessageWithSource, response.Type);
            Assert.NotNull(response.Data);
            Assert.Equal("Some setlist", response.Data.Content);
        }

        [Fact]
        public async Task GetResponse_WhenSetlistCommandAndDateFormatInvalid_ExpectErrorMessage()
        {
            // Arrange
            var fixture = new SetlistCommandHandlerTestFixture();

            var interaction = new Interaction
            {
                InteractionType = InteractionType.ApplicationCommand,
                Data = new InteractionData
                {
                    Name = "setlist",
                    Options = new InteractionOption[]
                    {
                        new InteractionOption { Name = "artist", Value = "phish" },
                        new InteractionOption { Name = "date", Value = "not a date" }
                    }
                }
            };

            // Act
            var response = await fixture.DiscordInteractionService.GetResponse(interaction);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(InteractionCallbackType.ChannelMessageWithSource, response.Type);
            Assert.NotNull(response.Data);
            Assert.Equal("Failed to parse date", response.Data.Content);
        }
    }
}
