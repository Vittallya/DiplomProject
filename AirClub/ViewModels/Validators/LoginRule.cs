using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using AirClub.Model;

namespace AirClub.ViewModels.Validators
{
    internal class LoginRule : ValidationRule
    {
     
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }

            string str = value.ToString();

            if (str.Length < 3)
            {
                return new ValidationResult(false, $"Длина логина должна быть от 3х символов");
            }



            //if (_dbContext.Employees.FirstOrDefault(x => x.Login == str) != null)
            //{
            //    return new ValidationResult(false, $"Такой логин уже существует");
            //}
            return new ValidationResult(true, null);
        }
    }
}
