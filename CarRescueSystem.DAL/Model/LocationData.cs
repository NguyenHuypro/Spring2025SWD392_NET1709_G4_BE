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
        public string LatString { get; set; }

        [JsonPropertyName("lon")]
        public string LonString { get; set; }

        [JsonIgnore]
        public double? Latitude => double.TryParse(LatString, out var lat) ? lat : null;

        [JsonIgnore]
        public double? Longitude => double.TryParse(LonString, out var lon) ? lon : null;
    }


}
