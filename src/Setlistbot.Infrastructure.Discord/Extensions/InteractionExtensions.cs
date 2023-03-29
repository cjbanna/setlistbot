using Setlistbot.Infrastructure.Discord.Interactions;

namespace Setlistbot.Infrastructure.Discord.Extensions
{
    public static class InteractionExtensions
    {
        public static string GetOption(this Interaction interaction, string name)
        {
            var option = interaction.Data?.Options.FirstOrDefault(o => o.Name == name);
            if (option == null)
            {
                throw new ArgumentException($"Option {name} not found");
            }

            return option.Value?.ToString() ?? string.Empty;
        }
    }
}
