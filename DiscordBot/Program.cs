using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Discord;
using Newtonsoft.Json;
using RestSharp.Extensions.MonoHttp;

namespace DiscordBot
{
    class Program
    {
        private static DiscordClient _client = new DiscordClient();

        static void Main(string[] args)
        {
            _client.MessageReceived += async (s, e) =>
            {
                if (Regex.IsMatch(e.Message.Text, @"\[\[.*?\]\]", RegexOptions.Compiled))
                {
                    var matches =
                        Regex.Matches(e.Message.Text, @"\[\[(.*?)\]\]", RegexOptions.Compiled).Cast<Match>().ToList();
                    if (matches.Count > 6)
                    {
                        await e.Channel.SendMessage("Too many cards to list!");
                        return;
                    }
                    foreach (
                        var match in
                            matches.Select(x => x.Groups[1].Value).Distinct(StringComparer.InvariantCultureIgnoreCase))
                    {
                        using (var webClient = new WebClient())
                        {
                            try
                            {
                                var response =
                                    webClient.DownloadString($"http://api.deckbrew.com/mtg/cards?name={match}");
                                var items = JsonConvert.DeserializeObject<List<Card>>(response);
                                if (items.Count > 0)
                                {

                                    var name = items[0].Name;
                                    var cost = items[0].Cost;
                                    var text = items[0].CardText;
                                    await
                                        e.Channel.SendMessage(
                                            $"{name} - {cost}\r\nTypes: {String.Join(" ", items[0].Types)}\r\nOracle Text:\r\n{text}\r\nGatherer: <http://gatherer.wizards.com/Pages/Card/Details.aspx?name={HttpUtility.UrlEncode(name)}>\r\n");
                                }
                            }
                            catch
                            {

                            }


                        }

                    }
                }
            };

            _client.ExecuteAndWait(async () =>
            {
                await
                    _client.Connect("MjIzOTM5ODIwNTc2MTc4MTg3.CrTWkw.WbY3p0tBXBtqWdQGNPIiG9WE9tQ", TokenType.Bot);
            });

        }
    }

    public class Edition
    {
        [JsonProperty("set")]
        public string Set { get; set; }
        [JsonProperty("rarity")]
        public string Rarity { get; set; }
        [JsonProperty("artist")]
        public string Artist { get; set; }
        [JsonProperty("multiverse_id")]
        public int MultiverseId { get; set; }
        [JsonProperty("flavor")]
        public string Flavor { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("set_url")]
        public string SetUrl { get; set; }
        [JsonProperty("number")]
        public string Number { get; set; }
        [JsonProperty("layout")]
        public string Layout { get; set; }
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }
        [JsonProperty("store_url")]
        public string StoreUrl { get; set; }
    }

    public class Card
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("store_url")]
        public string StoreUrl { get; set; }
        [JsonProperty("types")]
        public List<string> Types { get; set; }
        [JsonProperty("colors")]
        public List<string> Colors { get; set; }
        [JsonProperty("editions")]
        public List<Edition> Editions { get; set; }
        [JsonProperty("cmc")]
        public int CMC { get; set; }
        [JsonProperty("cost")]
        public string Cost { get; set; }
        [JsonProperty("text")]
        public string CardText { get; set; }
    }
}
