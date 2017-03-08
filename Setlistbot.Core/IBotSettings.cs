using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public interface IBotSettings
    {
        string Username { get; }
        string Password { get; }
        string Key { get; }
        string Secret { get; }
        string Subreddit { get; }
        int MaxSetlists { get; }
        int MaxComments { get; }
    }
}
