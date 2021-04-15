using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;


namespace AirClub.ViewModels.Validators
{
    internal class EducationDocumentSerialValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }
            var newVal = Regex.Replace(value.ToString(), @"[^0-9]", "");

            if (!newVal.All(Char.IsDigit))
            {
                return new ValidationResult(false, $"Введите только цифры");
            }

            if (newVal.Length != 6)
            {
                return new ValidationResult(false, $"Количество цифр должно быть 6");
            }

            return new ValidationResult(true, null);
        }
    }

    internal class EducationDocumentNumberValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }
            var newVal = Regex.Replace(value.ToString(), @"[^0-9]", "");

            if (!newVal.All(Char.IsDigit))
            {
                return new ValidationResult(false, $"Введите только цифры");
            }

            if (newVal.Length != 7)
            {
                return new ValidationResult(false, $"Количество цифр должно быть 7");
            }

            return new ValidationResult(true, null);
        }
    }
}
