using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using AirClub.Model.Db;

public class Special: BindableBase, IDbClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string AccessCode { get; set; }

    public double Salary { get; set; }

    public virtual ICollection<Employee> Employees { get; set; }

    public override string ToString()
    {
        return $"{Name}";
    }

    public TDb Clone<TDb>() where TDb: class, IDbClass, new()
    {
        if (typeof(TDb) == typeof(Special))
        {
            var special = new Special
            {
                Name = Name,
                AccessCode = AccessCode,
                Salary = Salary,
                Id = Id
            };

            return special as TDb;
        }
        return null;
    }

    public object Clone()
    {
        return this.MemberwiseClone();
    }

    public void UpdateData(IDbClass dbClass)
    {

        if(dbClass is Special sp)
        {
            this.AccessCode = sp.AccessCode;
            this.Name = sp.Name;
            this.Salary = sp.Salary;
        }

    }
}