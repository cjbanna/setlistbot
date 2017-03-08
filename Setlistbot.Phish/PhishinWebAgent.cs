using Newtonsoft.Json.Linq;
using RestSharp;
using Setlistbot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Setlistbot.Phish
{
    public class PhishinWebAgent : ISetlistAgent
    {
        public IList<ISetlist> GetSetlists(int year, int month, int day)
        {
            IList<ISetlist> setlists = new List<ISetlist>();

            string formattedDate = new DateTime(year, month, day).ToString("yyyy-MM-dd");
            string url = string.Format("http://phish.in/api/v1/shows/{0}", formattedDate);

            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = JObject.Parse(response.Content);
                setlists = ParseSetlistResponse(json);
            }

            return setlists;
        }

        private IList<ISetlist> ParseSetlistResponse(JObject json)
        {
            IList<ISetlist> setlists = new List<ISetlist>();

            if (json.Value<bool>("success"))
            {
                var data = json["data"];
                var venue = data["venue"];

                Setlist setlist = new Setlist();

                setlist.ShowID = data.Value<int>("id");
                setlist.ShowDate = data.Value<string>("date");
                setlist.Duration = data.Value<int>("duration");
                setlist.Venue = venue.Value<string>("name");
                setlist.Location = venue.Value<string>("location");
                setlist.Url = string.Format("http://phish.in/{0}", setlist.ShowDate);
                setlist.Sets = ParseTracks((JArray)data["tracks"]);

                foreach (Set set in setlist.Sets)
                {
                    set.Duration = set.Songs.Sum(s => s.Duration);
                }

                setlists.Add(setlist);
            }

            return setlists;
        }

        private IList<ISet> ParseTracks(JArray tracks)
        {
            IList<ISet> sets = new List<ISet>();

            ISet set = null;
            ISong song = null;
            foreach (var track in tracks)
            {
                string setName = track.Value<string>("set_name");

                if (!sets.Any(s => s.Name == setName))
                {
                    set = new Set();
                    set.Name = setName;
                    sets.Add(set);
                }

                song = new Song();
                song.Name = track.Value<string>("title");
                song.Position = track.Value<int>("position");
                song.Duration = track.Value<int>("duration");
                set.Songs.Add(song);
            }

            return sets;
        }
    }
}
