using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PNMS.Utilities
{
    public class URL
    {
        public class Validator
        {
            public static bool HasSpecialChars(string url)
            {
                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                return regexItem.IsMatch(url);
            }
        }
    }
}
