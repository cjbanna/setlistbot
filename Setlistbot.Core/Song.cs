using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    [DebuggerDisplay("{Position} {Name}")]
    public class Song : ISong
    {
        public int Position { get; set; }
        public string Name { get; set; }


        // optional
        public int Duration { get; set; }
    }
}
