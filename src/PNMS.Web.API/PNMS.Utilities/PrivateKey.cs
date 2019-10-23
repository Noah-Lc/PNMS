using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNMS.Utilities
{
    /// <summary>
    /// Helps with generating a private key
    /// </summary>
    public class PrivateKey
    {
        /// <summary>
        /// Generate a password
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string Generate(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
    }
}
