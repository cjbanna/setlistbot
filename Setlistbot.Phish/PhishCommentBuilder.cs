using Setlistbot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Phish
{
    public class CommentBuilder : ICommentBuilder
    {
        private int _maxSetlists;

        public CommentBuilder(int maxSetlists)
        {
            _maxSetlists = maxSetlists;
        }

        public string Build(IList<ISetlist> setlists)
        {
            StringBuilder reply = new StringBuilder();

            foreach (ISetlist setlist in setlists.Take(_maxSetlists))
            {
                reply.AppendFormat("[{0}]({1}) {2} @ {3}", setlist.ShowDate, GetPhishNetUrl(setlist), setlist.Location, setlist.Venue);
                reply.AppendLine();
                reply.AppendFormat("([phish.in]({1}) | [phishtracks]({2}))", GetPhishNetUrl(setlist), GetPhishinUrl(setlist), GetPhishTracksUrl(setlist));
                reply.AppendLine();
                reply.AppendLine();
            }

            reply.Append("> _data provided by [phish.in](http://phish.in)_");

            return reply.ToString();
        }

        public string Build(ISetlist setlist)
        {
            StringBuilder reply = new StringBuilder();
            reply.AppendFormat("# {0} {1} @ {2}", setlist.ShowDate, setlist.Location, setlist.Venue);
            reply.AppendLine();
            reply.AppendLine();

            foreach (ISet set in setlist.Sets)
            {
                TimeSpan span = TimeSpan.FromMilliseconds(set.Duration);
                string hours = span.TotalHours >= 1 ? span.ToString("%h") + "h " : string.Empty;
                string minutes = span.ToString("mm");
                reply.AppendFormat(@"**{0}**: ({1}{2}m)  ", set.Name, hours, minutes);
                foreach (ISong song in set.Songs.OrderBy(s => s.Position))
                {
                    reply.AppendFormat("{0}, ", song.Name);
                }
                reply.Remove(reply.Length - 2, 1);
                reply.AppendLine();
                reply.AppendLine();
            }

            reply.AppendFormat("[phish.net]({0}) | [phish.in]({1}) | [phishtracks]({2})", GetPhishNetUrl(setlist), GetPhishinUrl(setlist), GetPhishTracksUrl(setlist));
            reply.AppendLine();
            reply.AppendLine();
            reply.Append("> _setlist data provided by [phish.in](http://phish.in)_");

            return reply.ToString();
        }

        public string GetPhishTracksUrl(ISetlist setlist)
        {
            return string.Format("http://phishtracks.com/shows/{0}", setlist.ShowDate);
        }

        public string GetPhishinUrl(ISetlist setlist)
        {
            return string.Format("http://phish.in/{0}", setlist.ShowDate);
        }

        public string GetPhishNetUrl(ISetlist setlist)
        {
            return string.Format("http://phish.net/setlists/?d={0}", setlist.ShowDate);
        }
    }
}
