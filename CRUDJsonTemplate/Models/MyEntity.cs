using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CRUDJsonTemplate.Models
{
    public class MyEntity
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
