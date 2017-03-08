using Microsoft.ApplicationInsights;
using Microsoft.Azure;
using RedditSharp;
using RedditSharp.Things;
using Setlistbot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Setlistbot
{
    public class Setlistbot
    {
        public void Run(
            IBotSettings settings, 
            ISetlistAgent setlistAgent, 
            ICommentBuilder commentBuilder,
            ICommentRepository commentRepository)
        {
            // todo arg null checks

            TelemetryClient telemetry = new TelemetryClient();
            try
            {
                string subredditName = string.Format("/r/{0}", settings.Subreddit);
                DateParser dateParser = new DateParser();
                List<ISetlist> setlists = new List<ISetlist>();

                BotWebAgent webAgent = new BotWebAgent(settings.Username, settings.Password, settings.Key, settings.Secret, "http://127.0.0.1");
                Reddit reddit = new Reddit(webAgent, true);
                reddit.RateLimit = WebAgent.RateLimitMode.Pace;

                Subreddit subreddit = reddit.GetSubreddit(subredditName);

                IEnumerable<Comment> comments = subreddit.Comments.Take(settings.MaxComments);

                foreach (Comment comment in comments)
                {
                    if (!comment.Body.ToLower().Contains(settings.Username))
                    {
                        continue;
                    }

                    if (commentRepository.CommentExists(comment.Id))
                    {
                        continue;
                    }

                    setlists.Clear();
                    string reply = string.Empty;

                    List<DateTime> dates = dateParser.ParseDates(comment.Body);
                    foreach (DateTime date in dates)
                    {
                        setlists.AddRange(setlistAgent.GetSetlists(date.Year, date.Month, date.Day));
                    }

                    if (setlists.Count > 1)
                    {
                        reply = commentBuilder.Build(setlists);
                    }
                    else if (setlists.Count == 1)
                    {
                        reply = commentBuilder.Build(setlists.First());
                    }

                    if (reply.Length > 0)
                    {
                        comment.Reply(reply);
                        commentRepository.SaveComment(comment.Id, comment.Body, reply);
                    }
                }
            }
            catch (RateLimitException rle)
            {
                // cool it down for 3 minutes
                telemetry.TrackException(rle);
                Thread.Sleep(180 * 1000);
            }
            catch (Exception ex)
            {
                telemetry.TrackException(ex);
            }
        }
    }
}
