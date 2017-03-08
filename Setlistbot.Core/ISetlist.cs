using System.Collections.Generic;

namespace Setlistbot.Core
{
    public interface ISetlist
    {
        IList<ISet> Sets { get; set; }
        int ShowID { get; set; }
        string ShowDate { get; set; }
        string Url { get; set; }
        string Venue { get; set; }
        string Location { get; set; }

        // optional
        string Notes { get; set; }
        decimal Rating { get; set; }
        int Duration { get; set; }
    }
}