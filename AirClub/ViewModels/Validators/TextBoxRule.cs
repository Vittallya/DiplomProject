using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AirClub.ViewModels.Validators
{
    public class TextBoxRule : ValidationRule
    {

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string valueStr;

            try
            {

                valueStr = Convert.ToString(value);
            }
            catch
            {

                return new ValidationResult(false, "Недопустимые символы.");
            }


            if (string.IsNullOrEmpty(valueStr))
            {
                return new ValidationResult(false, $"Поле должно быть заполнено");
            }

            else
            {
                return new ValidationResult(true, null);
            }
        }
    }
}
