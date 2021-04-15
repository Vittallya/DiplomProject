using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirClub.ViewModels.Validators
{
    class PositiveValueRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }

            if (long.TryParse(value?.ToString(), out long resInt))
            {
                if (resInt < 0)
                {
                    return new ValidationResult(false, $"Значение должно быть положительным");
                }
                return new ValidationResult(true, null);

            }

            if(double.TryParse(value.ToString(), out double resDbl))
            {
                if (resDbl < 0)
                {
                    return new ValidationResult(false, $"Значение должно быть положительным");
                }
                return new ValidationResult(true, null);
            }

            else
            {
                return new ValidationResult(false, $"Введите числовое значение");
            }

        }
    }
}
