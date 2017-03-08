using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public class Set : ISet
    {
        public Set()
        {
            Songs = new List<ISong>();
        }

        public string Name { get; set; }
        public IList<ISong> Songs { get; set; }

        // optional
        public int Duration { get; set; }
    }
}
