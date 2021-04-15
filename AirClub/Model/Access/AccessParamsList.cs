using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AirClub.Model.Access
{

 
    public interface IAccessSection
    {
        public int Index { get; set; }
        public string Name { get; set; }

        public AccessParam[] Params { get; }
    }

    public interface IAccessParam
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
    [XmlRoot]
    [Serializable]
    public class AccessSection: IAccessSection
    {
        [XmlAttribute]
        public int Index { get; set; }
        [XmlAttribute]
        public string Name { get; set; }

        //public AccessParam[] Params { get; set; }
        public AccessParam[] Params { get; set; }


        public AccessSection() { }

        public AccessSection(int index, string name)
        {
            Index = index;
            Name = name;
        }
    }

    public class AccessParam : IAccessParam
    {
        [XmlAttribute]
        public string Name { get; set; }
        [XmlAttribute]
        public int Value { get; set; }

        public AccessParam() { }

        public AccessParam(string name, int value)
        {
            Name = name;
            Value = value;
        }

    }

}
