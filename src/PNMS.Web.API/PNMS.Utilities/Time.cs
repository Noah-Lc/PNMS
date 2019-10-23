using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PNMS.Utilities
{
    public class Time
    {
        /// <summary>
        /// Get Time Stamp from datetime 
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long UnixTimeStamp(DateTime date)
        {
            return ((DateTimeOffset)date).ToUnixTimeSeconds();
        }
    }
}
