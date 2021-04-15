using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace AirClub.ViewModels.Validators
{
    internal class PhoneValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }

            if (!value.ToString().All(char.IsDigit))
            {
                return new ValidationResult(false, $"Введите только цифры");
            }

            var newVal = Regex.Replace(value.ToString(), @"[^0-9]", "");

            if (newVal.Length != 11)
            {
                return new ValidationResult(false, $"Количество цифр должно быть 11");
            }

            return new ValidationResult(true, null);
        }
    }
}
