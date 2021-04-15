using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public class Insurance : IDbClass
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Cost { get; set; }

        public int PayementPeriod { get; set; }

        public string Compensation { get; set; }

        public string InsuranceType { get; set; }

        public int? Validity { get; set; }

        public int PartnerId { get; set; }

        public virtual Partner Partner { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is Insurance insurance)
            {
                Id = insurance.Id;
                Name = insurance.Name;
                Cost = insurance.Cost;
                PayementPeriod = insurance.PayementPeriod;
                Compensation = insurance.Compensation;
                InsuranceType = insurance.InsuranceType;
                Validity = insurance.Validity;
                PartnerId = insurance.PartnerId;
            }
        }
    }
}
