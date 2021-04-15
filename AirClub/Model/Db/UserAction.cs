using System;
using System.Collections.Generic;
using System.Text;

namespace AirClub.Model.Db
{
    public class UserAction : IDbClass
    {
        public int Id { get; set; }

        public int EmloyeeId { get; set; }
        public DateTime DateAction { get; set; }

        public virtual Employee Employee { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public virtual void UpdateData(IDbClass dbClass)
        {
            if(dbClass is IDbClass cl)
            {
                Id = cl.Id;
            }
        }
    }


    public class UserEnterAction: UserAction
    {
        //Происх. когда польз авторизуется
        public int NumberOfRertyes { get; set; }

        public override void UpdateData(IDbClass dbClass)
        {
            base.UpdateData(dbClass);
            if(dbClass is UserEnterAction action)
            {
                NumberOfRertyes = action.NumberOfRertyes;
            }
        }
    }

    public class UserTableOpenAction: UserAction
    {
        //Происх. когда польз. открывает таблицу бд
        public string VmType { get; set; }
        public string DbClassType { get; set; }

        public override void UpdateData(IDbClass dbClass)
        {
            base.UpdateData(dbClass);
            if (dbClass is UserTableOpenAction action)
            {
                VmType = action.VmType;
            }
        }
    }

    public class UserDbEditAction: UserAction
    {
        //Происходит, когда польз. редактир. элемент какой-либо
        public int ItemId { get; set; }
        public string ItemType { get; set; }
        public UserDbActionType ActionType { get; set; }

        public override void UpdateData(IDbClass dbClass)
        {
            base.UpdateData(dbClass);
            if (dbClass is UserDbEditAction action)
            {
                ItemId = action.ItemId;
                ItemType = action.ItemType;
                ActionType = action.ActionType;
            }
        }
    }

    public enum UserDbActionType
    {
        View, Add, Edit, Remove,
    }
}
