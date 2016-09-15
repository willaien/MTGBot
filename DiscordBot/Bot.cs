using Discord;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using RestSharp.Extensions.MonoHttp;

namespace DiscordBot
{
    public sealed class Bot
    {
        private DiscordClient _client = new DiscordClient();

        // Extract all instances of [[CardName]], capturing (CardName)
        private const string CardTextRegex = @"\[\[(.*?)\]\]";

        /// <summary>
        /// Processes a single message received by this bot.
        /// </summary>
        /// <param name="s">Empty</param>
        /// <param name="e">The <see cref="MessageEventArgs"/> associated with this <see cref="Message"/>.</param>
        private static async void ProcessMessage(object s, MessageEventArgs e)
        {
            // If there are no cards in this message, we bail.
            if (!Regex.IsMatch(e.Message.Text, CardTextRegex, RegexOptions.Compiled))
                return;

            // Let's extract all of the card names from the message. We cast to Match and project into a list
            // So that we can run LINQ and enumerate multiple times.
            var matches = Regex.Matches(e.Message.Text, CardTextRegex, RegexOptions.Compiled).Cast<Match>().ToList();

            // Arbitrarily limits the number of cards to list to 6.
            if (matches.Count > 6)
            {
                await e.Channel.SendMessage("Too many cards to list!");
                return;
            }

            // Distinct out the card names, using a case-insensitive comparer.
            var cardNames = matches.Select(x => x.Groups[1].Value).Distinct(StringComparer.InvariantCultureIgnoreCase);


            // Loop through the names, look up against Deckbrew's API and return a response for each mentioned card.
            foreach (var cardName in cardNames)
            {
                using (var webClient = new WebClient())
                {
                    try
                    {
                        // For more information about Deckbrew's API, see: http://deckbrew.com/api
                        var response = webClient.DownloadString($"http://api.deckbrew.com/mtg/cards?name={cardName}");
                        var cards = JsonConvert.DeserializeObject<List<Card>>(response);
                        if (cards.Count > 0)
                        {
                            var card = cards.First();
                            
                            await
                                e.Channel.SendMessage(
                                    $"{card.Name} - {card.Cost}\r\nTypes: {String.Join(" ", card.Types)}\r\nOracle Text:\r\n{card.CardText}\r\nGatherer: <http://gatherer.wizards.com/Pages/Card/Details.aspx?name={HttpUtility.UrlEncode(card.Name)}>\r\n");
                        }
                    }
                    catch
                    {
                        // Just send no response if the server fails to respond, or returns invalid data for now.
                    }


                }

            }

        }

        /// <summary>
        /// Starts the bot by connecting to Discord, then waits for commands.
        /// </summary>
        public void Start()
        {
            //async void isn't a good place to be in, but, the people who wrote
            //Discord.NET use events, and have an async heavy API.
            _client.MessageReceived += ProcessMessage; 
            _client.ExecuteAndWait(async () =>
            {
                await _client.Connect(Properties.Settings.Default.ClientToken, TokenType.Bot);
            });
        }
    }
}
