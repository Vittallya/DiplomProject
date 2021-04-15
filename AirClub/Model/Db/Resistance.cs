using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AirClub.Pages;

namespace AirClub.Model.Db
{
    public class Reservation : IDbClass
    {
        public int Id { get; set; }

        public int ClientId { get; set; }
        public int ServiceId { get; set; }
        public int InsuranceId { get; set; }
        public DateTime DateCreation { get; set; }
        public DateTime DateReservation { get; set; }
        public virtual Client Client { get; set; }
        public virtual Service Service { get; set; }
        public virtual Insurance Insurance { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is Reservation res)
            {
                Id = res.Id;
                ClientId = res.ClientId;
                Client = res.Client;
                ServiceId = res.ServiceId;
                Service = res.Service;
                InsuranceId = res.InsuranceId;
                Insurance = res.Insurance;
                DateCreation = res.DateCreation;
                DateReservation = res.DateReservation;
            }
        }
    }
}
