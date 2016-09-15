using Newtonsoft.Json;
using System.Collections.Generic;

/// <summary>
/// Data class that is used to deserialize messages from Deckbrew's API
/// </summary>
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