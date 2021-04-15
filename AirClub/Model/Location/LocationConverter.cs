using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.Mvvm.Native;
using Newtonsoft.Json;

namespace AirClub.Model.Location
{
    public static class LocationConverter
    {
        public static async Task ConvertToFile()
        {
            for (int i = 1; i <= NumberOfRetries; ++i)
            {
                try
                {                    
                    using (var writer = new StreamWriter(filePath, false))
                    {
                        var str = JsonConvert.SerializeObject(_original.ToList());
                        await writer.WriteAsync(str);
                    }
                    break; // When done we can break loop
                }
                catch (IOException) when (i <= NumberOfRetries)
                {
                    // You may check error code to filter some exceptions, not every error
                    // can be recovered.
                    Thread.Sleep(DelayOnRetry);
                }
            }
        }

        private static readonly string filePath = "places.json";

        private static IList<Place> _original;

        private const int NumberOfRetries = 3;
        private const int DelayOnRetry = 1000;

        public static async Task<IEnumerable<Place>> ConverFromFile()
        {            
            JsonSerializer serializer = new JsonSerializer();


            using (var stram = File.Open(filePath, FileMode.Open))
            {
                using (var reader = new StreamReader(stram))
                {
                    using (JsonReader jsonR = new JsonTextReader(reader))
                    {
                        while (await jsonR.ReadAsync())
                        {
                            _original = serializer.Deserialize<IList<Place>>(jsonR);
                        }
                    }
                }
            }
            DefineStructure(_original);

            return _original;
        }

        public static async Task<IEnumerable<Place>> Filter(string value)
        {
            if (_original == null)
            {
                _original = (await ConverFromFile()).ToList();
            }

            if (string.IsNullOrEmpty(value))
            {
                return _original;
            }

            IList<Place> childrens = _original.ToList();

            IList<Place> res = new List<Place>();

            while (childrens != null && childrens.Count > 0)
            {
                res = res.Union(FilterList(childrens), new PlaceComparer()).ToList();
                childrens = childrens.Where(x => x.Places != null).SelectMany(x => x.Places).ToArray();
            }

            return res;

            IList<Place> FilterList(IEnumerable<Place> places)
            {
                var res = places.Where(x => x.FullName.ToLower().StartsWith(value.ToLower()) 
                || x.Name.ToLower().StartsWith(value.ToLower())).ToList();

                return res;
            }
        }

        public static IEnumerable<Place> Places => _original;

        private static void DefineStructure(IEnumerable<Place> places, Place parent = null)
        {
            foreach(var place in places)
            {
                if(parent != null)
                {
                    place.Parent = parent;
                }

                if(place.Places != null && place.Places.Length > 0)
                {
                    DefineStructure(place.Places, place);
                }
            }
        }

        public async static void Add(Place place)
        {
            _original.Add(place);
            await ConvertToFile();
        }

        public async static void AddChildTo(Place child, Place parent)
        {
            var list = parent.Places?.ToList() ?? new List<Place>();
            list.Add(child);
            parent.Places = list.ToArray();
            await ConvertToFile();
        }
        public async static void Edit(Place newPlace, Place oldPlace)
        {
            oldPlace.Name = newPlace.Name;
            oldPlace.PlaceType = newPlace.PlaceType;
            await ConvertToFile();
        }
        public async static void Remove(Place place)
        {
            if (place.Parent == null)
            {
                _original.Remove(place);
            }
            else
            {
                var parent = place.Parent;
                var list = parent.Places.ToList();
                list.Remove(place);
                parent.Places = list.ToArray();
            }
            await ConvertToFile();
        }
    }

    public class PlaceComparer : IEqualityComparer<Place>
    {
        public bool Equals([AllowNull] Place x, [AllowNull] Place y)
        {
            return x.Name.Equals(y.Name);
        }

        public int GetHashCode([DisallowNull] Place obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}
