using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Core
{
    public class BotSettings : IBotSettings
    {
        public string Key { get; set; }
        public string Password { get; set; }
        public string Secret { get; set; }
        public string Subreddit { get; set; }
        public string Username { get; set; }
        public int MaxSetlists { get; set; }
        public int MaxComments { get; set; }
    }
}
