using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainOps
{
    /// <summary>
    /// domain utilities
    /// </summary>
    public static class DUtils
    {

        /// <summary>
        /// extention method to determine is the string is null or empty
        /// </summary>
        /// <param name="target">string to test</param>
        /// <returns>boolean</returns>
        public static bool IsEmpty(this string target)
        {
            return String.IsNullOrEmpty(target);
        }
    }
}
