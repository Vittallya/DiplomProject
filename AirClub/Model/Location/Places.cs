using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Location
{

    public class Place
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        //[JsonProperty("type", ItemConverterType =(typeof(int)))]
        [JsonProperty("type")]
        public PlaceType PlaceType { get; set; }

        [JsonProperty("places")]
        public Place[] Places { get; set; }

        [JsonIgnore()]
        public Place Parent { get; set; }

        [JsonIgnore()]
        public string FullName 
        { 
            get
            {
                if(PlaceType == PlaceType.Capital || PlaceType == PlaceType.City)
                {
                    return $"г. {Name}";
                }
                return Name;
            } 
        }

        [JsonIgnore()]
        public static Dictionary<PlaceType, string> PlaceNames { get; } = new Dictionary<PlaceType, string>
        {
            [PlaceType.Country] = "Страна",
            [PlaceType.Capital] = "Столица",
            [PlaceType.City] = "Город",
            [PlaceType.Place] = "Абстрактная местность",
        };
    }

    public enum PlaceType
    {
        Country = 1, Capital, City, Place
    }
}
