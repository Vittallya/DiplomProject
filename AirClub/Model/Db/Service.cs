using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public class Service: BindableBase, IDbClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int AgeFrom { get; set; }
        public int AgeBefore { get; set; }
        public string PhysReqs { get; set; }
        public double Cost { get; set; }



        public virtual object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual void UpdateData(IDbClass dbClass)
        {
            if(dbClass is Service serv)
            {
                Id = serv.Id;
                Name = serv.Name;
                AgeFrom = serv.AgeFrom;
                AgeBefore = serv.AgeBefore;
                PhysReqs = serv.PhysReqs;
                Cost = serv.Cost;
            }
        }
    }

    public class ServiceCourse: Service
    {
        public int CourseDuration { get; set; }

        public int ExersiceDuration { get; set; }

        public override void UpdateData(IDbClass dbClass)
        {
            base.UpdateData(dbClass);
            if(dbClass is ServiceCourse course)
            {
                CourseDuration = course.CourseDuration;
                ExersiceDuration = course.ExersiceDuration;
            }
        }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

    }
    public class ServiceCompetition: Service
    {
        public DateTime DateBegin { get; set; }
        public DateTime DateEnd { get; set; }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public override void UpdateData(IDbClass dbClass)
        {
            base.UpdateData(dbClass);

            if(dbClass is ServiceCompetition competition)
            {
                DateBegin = competition.DateBegin;
                DateEnd = competition.DateEnd;
            }
        }
    }

    public class ServiceActiveRest: Service
    {
        public int TourId { get; set; }

        public virtual Tour Tour { get; set; }

        public int MinCountPeople { get; set; }
        public int MaxCountPeople { get; set; }

        public override object Clone()
        {
            return this.MemberwiseClone();
        }

        public override void UpdateData(IDbClass dbClass)
        {
            base.UpdateData(dbClass);
            if(dbClass is ServiceActiveRest rest)
            {
                TourId = rest.TourId;
                MinCountPeople = rest.MinCountPeople;
                MaxCountPeople = rest.MaxCountPeople;
            }

        }
    }
}
