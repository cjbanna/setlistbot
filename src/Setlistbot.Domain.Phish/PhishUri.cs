namespace Setlistbot.Domain.Phish
{
    public static class PhishUri
    {
        public static Uri PhishNet(DateOnly date) =>
            new Uri($"https://phish.net/setlists/?d={date:yyyy-MM-dd}");

        public static Uri PhishIn(DateOnly date) => new Uri($"https://phish.in/{date:yyyy-MM-dd}");

        public static Uri PhishTracks(DateOnly date) =>
            new Uri($"https://phishtracks.com/shows/{date:yyyy-MM-dd}");
    }
}
