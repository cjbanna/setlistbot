using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public interface ISong
    {
        int Position { get; set; }
        string Name { get; set; }

        // optional
        int Duration { get; set; }
    }
}
