using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Setlistbot.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Setlistbot.Phish
{
	// TODO: work in progress

    public class PhishDotNetWebAgent : ISetlistAgent
    {
		private string _apiKey;

		public PhishDotNetWebAgent(string apiKey)
		{
			_apiKey = apiKey;
		}
        public IList<ISetlist> GetSetlists(int year, int month, int day)
        {
            IList<ISetlist> setlists = new List<ISetlist>();

            string formattedDate = new DateTime(year, month, day).ToString("yyyy-MM-dd");
            string url = string.Format("https://api.phish.net/v3/setlists/get?apikey={0}&showdate={1}", _apiKey, formattedDate);

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

            if (json.Value<int>("error_code") == 0)
            {
                var response = json["response"];
                var data = (JArray)response["data"];

                foreach (var show in data)
                {
                    // only parse Phish setlists
                    int artistID = show.Value<int>("artistid");
                    if (artistID != 1)
                        continue;


                    Setlist setlist = new Setlist();
                    setlist.ShowID = show.Value<int>("showid");
                    setlist.ShowDate = show.Value<string>("showdate");
                    setlist.Url = show.Value<string>("url");
                    setlist.Venue = show.Value<string>("venue");
                    setlist.Location = show.Value<string>("location");
                    setlist.Sets = ParseSetlistDataResponse(show.Value<string>("setlistdata"));

                    setlists.Add(setlist);
                }

                
            }

            return setlists;
        }

        private IList<ISet> ParseSetlistDataResponse(string setlistdata)
        {
            IList<ISet> sets = new List<ISet>();

            Regex setRegex = new Regex(@"\<span class\=\'set\-label\'\>");

            string snippet = string.Empty;

            MatchCollection matches = setRegex.Matches(setlistdata);
            
            int lastMatchIndex = matches.Count - 1;
            int startIndex = 0;
            Set set = null;
            for (int i = 0; i < matches.Count; i++)
            {
                startIndex = matches[i].Index;

                if (i == lastMatchIndex)
                {
                    // assume encore
                    snippet = setlistdata.Substring(startIndex);
                    set = new Set();
                    set.Name = string.Format("Encore {0}", i + 1);
                    set.Songs = ParseSongsForSet(snippet);
                    
                }
                else
                {
                    int length = matches[i + 1].Index - matches[i].Index;
                    snippet = setlistdata.Substring(startIndex, length);
                    set = new Set();
                    set.Name = string.Format("Set {0}", i + 1);
                    set.Songs = ParseSongsForSet(snippet);
                }

                sets.Add(set);
            }

            return sets;
        }

        private IList<ISong> ParseSongsForSet(string set)
        {
            List<string> patterns = new List<string>()
            {
                @"\<p\>",
                @"\<\/p\>",
                @"\<sup(.*)\<\/sup\>",
                @"\<span class\=\'set\-label\'\>",
                @"\<\/span\>",
                @"\<a title\=(.*?)\>",
                @"\<\/a\>",
                @"\<a href\=(.*?)\'\>"
            };

            string result = set;
            foreach (string pattern in patterns)
            {
                result = Regex.Replace(result, pattern, string.Empty);
            }
            

            //String result = Regex.Replace(set, @"<[^>]*>", string.Empty);

            //string result2 = Regex.Replace(result, @"\[\""\d+\]", string.Empty);
            //string result3 = Regex.Replace(result2, @"^.*\:", string.Empty);

            IList<ISong> songs = new List<ISong>();

            //Regex songRegex = new Regex(@"title\=\'[\w\s]+\'");
            Regex songRegex = new Regex(@"\<a[\w\s]+\<\/a\>");


            int position = 1;
            string name = string.Empty;
            foreach (Match match in songRegex.Matches(set))
            {
                name = match.Value.Replace("title='", string.Empty);
                songs.Add(new Song
                {
                    Name = name,
                    Position = position
                });
                position++;
            }

            return songs;
        }

    }
}
