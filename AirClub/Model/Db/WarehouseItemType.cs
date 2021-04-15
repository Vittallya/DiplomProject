using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    internal class WarehouseItemType : BindableBase, IDbClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is WarehouseItemType itemType)
            {
                Id = itemType.Id;
                Name = itemType.Name;
            }
        }
    }
}
