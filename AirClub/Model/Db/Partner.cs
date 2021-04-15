using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public class Partner : IDbClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string INN { get; set; }

        public string Address { get; set; }

        public string FieldOfActivity { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is Partner partner)
            {
                Id = partner.Id;
                Name = partner.Name;
                INN = partner.INN;
                Address = partner.Address;
                FieldOfActivity = partner.FieldOfActivity;
            }
        }
    }
}
