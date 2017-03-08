using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public interface ICommentBuilder
    {
        string Build(IList<ISetlist> setlists);

        string Build(ISetlist setlist);
    }
}
