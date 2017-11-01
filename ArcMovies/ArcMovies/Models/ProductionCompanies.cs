using Newtonsoft.Json;

namespace ArcMovies.Models
{
    public class ProductionCompanies
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
