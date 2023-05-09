using System;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace DevPace.Wpf.Validators
{
    internal class EmailValidator : ValidationRule
    {
        private const string _validationError = "The email must match the template: username@domain.com";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (string.IsNullOrEmpty((string)value))
                {
                    return new ValidationResult(true, value);
                }
                else
                {
                    if (MailAddress.TryCreate((string)value, out MailAddress? checkEmail))
                    {
                        string pattern =
                                  @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                        Regex regex = new(pattern, RegexOptions.IgnoreCase);
                        if (regex.IsMatch(checkEmail.Address))
                        {
                            return new ValidationResult(true, value);
                        }
                    }
                }

                return new ValidationResult(false, _validationError);
            }
            catch (Exception)
            {
                return new ValidationResult(false, _validationError);
            }
        }
    }
}
