using DevPace.DbConnectionSQLite;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace DevPace.WebApi.Verifications
{
    internal class CustomerVerification
    {
        private readonly IDbContextFactory<DatabaseConnector> _databaseFactory;

        public CustomerVerification(IDbContextFactory<DatabaseConnector> databaseFactory,
                                    Models.Customer customer)
        {
            _databaseFactory = databaseFactory;
            Customer = customer;
        }

        internal Models.Customer Customer { get; init; }

        internal bool Verified { get => NameVerified && PhoneVerified && EmailVerified; }

        internal bool NameVerified { get; private set; }
        internal bool PhoneVerified { get; private set; }
        internal bool EmailVerified { get; private set; }

        internal void Verify()
        {
            CheckName();
            CheckPhone();
            CheckEmail();
        }

        private void CheckName()
        {
            using DatabaseConnector db = _databaseFactory.CreateDbContext();

            NameVerified = !db.Customers.Any(el => el.Name == Customer.Name && el != Customer);
        }

        private void CheckPhone()
        {
            PhoneVerified = false;

            if (string.IsNullOrEmpty(Customer.Phone))
            {
                PhoneVerified = true;
            }
            else
            {
                PhoneVerified = Customer.Phone.Length == Customer.Phone.Where(el => char.IsDigit(el)).Count();
            }
        }

        private void CheckEmail()
        {
            EmailVerified = false;

            if (string.IsNullOrEmpty(Customer.Email))
            {
                EmailVerified = true;
            }
            else
            {
                if (MailAddress.TryCreate(Customer.Email, out MailAddress? checkEmail))
                {
                    string pattern =
                              @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                            + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                            + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

                    Regex regex = new(pattern, RegexOptions.IgnoreCase);
                    if (regex.IsMatch(checkEmail.Address))
                    {
                        EmailVerified = true;
                    }
                }
            }
        }
    }
}
