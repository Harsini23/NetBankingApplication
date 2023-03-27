using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util
{
    internal static class EmailValidation
    {
        public static bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                // Use the MailAddress class to validate the email address (syntax validation)
                MailAddress addr = new MailAddress(email);

                // Check if the domain name is valid ( split into various sub and top level domain)
                string[] parts = addr.Host.Split('.');
                if (parts.Length >= 2)
                {
                    //validating top level domain
                    string tld = parts[parts.Length - 1];
                    if (tld.Length > 1 && tld.Length <= 4)
                    {
                        return true;
                    }
                }
            }
            catch (FormatException)
            {
                // The email address is not valid
            }

            return false;
        }
    }
}
