using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CarRescueSystem.DAL.Model
{
    public class LocationData
    {
        [JsonPropertyName("lat")]
        public string latString { get; set; }

        [JsonPropertyName("lon")]
        public string lonString { get; set; }

        [JsonIgnore]
        public double? latitude => double.TryParse(latString, out var lat) ? lat : null;

        [JsonIgnore]
        public double? longitude => double.TryParse(lonString, out var lon) ? lon : null;
    }


}
