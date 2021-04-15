using System.Globalization;
using System.Windows.Controls;

namespace AirClub.ViewModels.Validators
{
    public class AgeValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {

            if(value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }

            if (int.TryParse(value?.ToString(), out int res))
            {
                if(res < 0)
                {
                    return new ValidationResult(false, $"Возраст не может быть отрицаительным");
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
