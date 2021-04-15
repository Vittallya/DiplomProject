using AirClub.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirClub.ViewModels.Validators
{
    internal class PasswordRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }

            string str = value.ToString();

            if(str.Length < 3)
            {
                return new ValidationResult(false, $"Длина пароля должна быть от 3х символов");
            }

            //if(Db.DbModule.GetTable("SELECT * FROM EMPLOYEES WHERE empPass = @pass", 
            //    ("pass", str)).Rows.Count > 0)
            //{
            //    return new ValidationResult(false, $"Такой пароль уже существует");
            //}
            return new ValidationResult(true, null);
        }
    }
}
