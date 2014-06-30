using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FelicaMail
{
    public static class MailAddress
    {
        public static bool IsValidEmail(string mail)
        {
            if (string.IsNullOrWhiteSpace(mail))
            {
                return false;
            }

            try
            {
                new System.Net.Mail.MailAddress(mail);
            }
            catch (FormatException)
            {
                return false;
            }

            return true;
        }
    }
}
