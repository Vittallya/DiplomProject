using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AirClub.Model.Db
{
    public class PlaceOld : IDbClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Region { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is PlaceOld place)
            {
                Id = place.Id;
                Name = place.Name;
                Country = place.Country;
                City = place.City;
                Region = place.Region;
            }
        }
    }

    [Serializable]
    public class Country
    {
        [XmlAttribute]
        public string Name { get; set; }

        public City[] Cities { get; set; }
    }

    public class City
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public bool IsCapital { get; set; }

    }




    //места не будут фигурировать в бд, они будут в JSON или XML
}
