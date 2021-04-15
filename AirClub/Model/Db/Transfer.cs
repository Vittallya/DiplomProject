using DevExpress.Mvvm;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AirClub.Model.Db
{
    public class Transfer: IDbClass
    {
        public int Id { get; set; }
        public string TransferAPoint { get; set; }
        public string TransferBPoint { get; set; }
        public string Transport { get; set; }
        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        
        public int? InvertedForId { get; set; } 
        public virtual ICollection<TransferTour> TransferTours { get; set; }
        public object Clone()
        {
            return this.MemberwiseClone();
        }
        public void UpdateData(IDbClass dbClass)
        {
            if(dbClass is Transfer transfer)
            {
                Id = transfer.Id;
                TransferAPoint = transfer.TransferAPoint;
                TransferBPoint = transfer.TransferBPoint;
                Transport = transfer.Transport;
                PartnerId = transfer.PartnerId;
                Partner = transfer.Partner;
            }
        }
    }


    //Заметка: в данном случае присутсвует только точка назначения, убрана значение откуда
    //Также можно в детальном окне тура
    //добавить кнопку "обратный трансфер", она создаст копию выбранного элемента с обратными точками А и Б

    //При добавлении или редакт. элемента нужно проследить, чтобы не добавились две одинаковые точки А и Б

    //элементы таблицы "Места" удалять нельзя
}
