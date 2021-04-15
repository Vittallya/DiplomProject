using AirClub.Model.Db;
using DevExpress.Mvvm;
using System;
using System.Collections;
using System.Security;
using System.Windows.Controls;


public class Human : BindableBase, IDbClass
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }

    public string Phone { get; set; }

    public bool Gender { get; set; }

    public DateTime DateBirth { get; set; }

    public virtual TDbClass Clone<TDbClass>() where TDbClass: class, IDbClass, new()
    {
        if (typeof(TDbClass).BaseType == typeof(Human))
        {
            var dbClass = new TDbClass();

            var human = dbClass as Human;
            human.Name = Name;
            human.Surname = Surname;
            human.Id = Id;
            human.Phone = Phone;
            human.Gender = Gender;
            human.DateBirth = DateBirth;


            return human as TDbClass;
        }
        return null;
    }

    public virtual object Clone()
    {
        return this.MemberwiseClone();
    }

    public virtual void UpdateData(IDbClass dbClass)
    {
        if(dbClass is Human human)
        {
            this.Id = human.Id;
            this.Name = human.Name;
            this.Surname = human.Surname;
            this.DateBirth = human.DateBirth;
            this.Phone = human.Phone;
            this.Gender = human.Gender;
            
        }
    }
}



public class Employee : Human
{
    public int SpecialId { get; set; }

    public string AccessCode { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public virtual Special Special { get; set; }

    public string EducationDoc { get; set; }

    public DateTime EdDocGetDate { get; set; }

    public override TEmployee Clone<TEmployee>()
    {
        var human = base.Clone<Employee>();

        var emp = human as Employee;

        emp.SpecialId = SpecialId;
        emp.AccessCode = AccessCode;
        emp.Login = Login;
        emp.Password = Password;
        emp.Special = new Special
        {
            AccessCode = Special.AccessCode,
            Name = Special.Name,
            Id = Special.Id
        };
        emp.EducationDoc = EducationDoc;
        emp.EdDocGetDate = EdDocGetDate;

        return emp as TEmployee;
    }

    public override void UpdateData(IDbClass dbClass)
    {
        base.UpdateData(dbClass);

        if(dbClass is Employee emp)
        {
            this.Password = emp.Password;
            this.Login = emp.Login;
            this.Special = emp.Special;
            this.SpecialId = emp.SpecialId;
            this.AccessCode = emp.AccessCode;
            this.EdDocGetDate = emp.EdDocGetDate;
            this.EducationDoc = emp.EducationDoc;
        }
    }
}