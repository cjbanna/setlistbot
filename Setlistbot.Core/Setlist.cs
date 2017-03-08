using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public class Setlist : ISetlist
    {
        public Setlist()
        {
            Sets = new List<ISet>();
        }

        public IList<ISet> Sets { get; set; }
        public int ShowID { get; set; }
        public string ShowDate { get; set; }
        public string Url { get; set; }
        public string Venue { get; set; }
        public string Location { get; set; }

        // optional
        public string Notes { get; set; }
        public decimal Rating { get; set; }
        public int Duration { get; set; }
    }
}
