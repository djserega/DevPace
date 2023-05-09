using System;
using System.Globalization;
using System.Windows.Controls;

namespace DevPace.Wpf.Validators
{
    internal class CompanyNameValidator : ValidationRule
    {
        public int MaxCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string _validationError = $"The maximum length of the company name is {MaxCharacters} characters";

            try
            {
                if (((string)value).Length <= MaxCharacters)
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
