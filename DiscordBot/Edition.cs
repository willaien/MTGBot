using Newtonsoft.Json;

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