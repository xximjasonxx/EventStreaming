using Newtonsoft.Json;

namespace ConnectorApp.Models
{
    public class Company
    {
        public string id { get => symbol; }
        public string type => "company";

        [JsonProperty("Name")]
        public string name { get; init; }

        [JsonProperty("Symbol")]
        public string symbol { get; init; }

        [JsonProperty("Size")]
        public int size { get; init; }
    }
}