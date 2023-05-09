using System;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;

namespace DevPace.Wpf.Validators
{
    internal class PhoneValidator : ValidationRule
    {
        private const string _validationError = "The phone number must consist of with numbers only";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int lengthValue = ((string)value).Length;
                int countDigits = ((string)value).Where(el => char.IsDigit(el)).Count();

                if (lengthValue == countDigits)
                    return new ValidationResult(true, value);
                else
                    return new ValidationResult(false, _validationError);
            }
            catch (Exception)
            {
                return new ValidationResult(false, _validationError);
            }
        }
    }
}
