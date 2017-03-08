using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public interface ISet
    {
        string Name { get; set; }
        IList<ISong> Songs { get; set; }

        // optional
        int Duration { get; set; }
    }
}
