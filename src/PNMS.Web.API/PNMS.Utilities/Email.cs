using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PNMS.Utilities
{
    /// <summary>
    /// For Email Validation
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Check the email
        /// </summary>
        public class Validation
        {
            public static bool IsValid(string emailaddress)
            {
                if (string.IsNullOrEmpty(emailaddress))
                    return false;
                try
                {
                    MailAddress m = new MailAddress(emailaddress);

                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }
        }
    }
}
