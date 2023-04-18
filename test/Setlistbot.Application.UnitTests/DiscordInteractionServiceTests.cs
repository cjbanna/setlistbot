using Microsoft.Extensions.Logging;
using Moq;
using Setlistbot.Application.Discord;
using Setlistbot.Domain;
using Setlistbot.Infrastructure.Discord.Interactions;
using Setlistbot.Infrastructure.Repositories;

namespace Setlistbot.Application.UnitTests
{
    public class DiscordInteractionServiceTestFixture
    {
        public Mock<ILogger<DiscordInteractionService>> Logger { get; }
        public Mock<ISetlistProviderFactory> SetlistProviderFactory { get; }
        public Mock<IReplyBuilderFactory> ReplyBuilderFactory { get; }
        public Mock<IDiscordUsageRepository> DiscordUsageRepository { get; }
        public DiscordInteractionService DiscordInteractionService { get; }

        public DiscordInteractionServiceTestFixture()
        {
            Logger = new Mock<ILogger<DiscordInteractionService>>();
            SetlistProviderFactory = new Mock<ISetlistProviderFactory>();
            ReplyBuilderFactory = new Mock<IReplyBuilderFactory>();
            DiscordUsageRepository = new Mock<IDiscordUsageRepository>();

            DiscordInteractionService = new DiscordInteractionService(
                Logger.Object,
                SetlistProviderFactory.Object,
                ReplyBuilderFactory.Object,
                DiscordUsageRepository.Object
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
            replyBuilder.Setup(b => b.Build(setlists)).Returns("Some setlist");

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
            Assert.Equal(
                $"Failed to parse date: 'not a date'. Try using mm/dd/yy or yyyy-mm-dd.",
                response.Data.Content
            );
        }

        [Fact]
        public async Task GetResponse_WhenSetlistNotFound_ExpectErrorMessage()
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
                        new InteractionOption { Name = "date", Value = "1980-01-01" }
                    }
                }
            };

            // Act
            var response = await fixture.DiscordInteractionService.GetResponse(interaction);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(InteractionCallbackType.ChannelMessageWithSource, response.Type);
            Assert.NotNull(response.Data);
            Assert.Equal(
                $"No setlist found for phish on 1980-01-01. Please try again with a different date or artist.",
                response.Data.Content
            );
        }
    }
}
