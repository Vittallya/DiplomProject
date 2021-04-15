using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public interface IDbClass: ICloneable
    {
        public int Id { get; set; }

        public void UpdateData(IDbClass dbClass);

        //public TDbClass Clone<TDbClass>() where TDbClass : class, IDbClass, new();
    }
}
