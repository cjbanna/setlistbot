using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public interface ISetlistAgent
    {
        IList<ISetlist> GetSetlists(int year, int month, int day);
    }
}
