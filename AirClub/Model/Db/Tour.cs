using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public class Tour: IDbClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<TransferTour> TransferTours { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is Tour tour)
            {
                Id = tour.Id;
                Name = tour.Name;
            }
        }
        //public int ProgramId { get; set; }
        //Коллекция трансферов
    }

    public class TransferTour
    {
        public int TransferId { get; set; }
        public virtual Transfer Transfer { get; set; }

        public int TourId { get; set; }
        public virtual Tour Tour { get; set; }
    }
}
