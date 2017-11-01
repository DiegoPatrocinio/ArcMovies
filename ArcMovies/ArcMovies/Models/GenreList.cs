using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcMovies.Models
{
    public class GenreList
    {
        [JsonProperty(PropertyName = "genres")]
        public List<Genre> Genres { get; set; }
    }
}
