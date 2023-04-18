using Newtonsoft.Json;
using Setlistbot.Infrastructure.Data;
using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Infrastructure.Extensions
{
    public static class DiscordUsageExtensions
    {
        public static DiscordUsageEntity ToTableEntity(this Interaction interaction)
        {
            return new DiscordUsageEntity
            {
                PartitionKey = Guid.NewGuid().ToString(),
                RowKey = Guid.NewGuid().ToString(),
                Id = interaction.Id,
                ApplicationId = interaction.ApplicationId,
                GuildId = interaction.GuildId,
                IteractionType = (int)interaction.InteractionType,
                Data = JsonConvert.SerializeObject(interaction.Data)
            };
        }
    }
}
