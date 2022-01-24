using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

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
        /// <summary>
        /// This searches the source for any replacement values (${xxx}) and extracts the replacement value name.
        /// </summary>
        /// <param name="source">string to search</param>
        /// <param name="pattern">regex pattern - only necessary if it is different than the default</param>
        /// <returns>List&lt;string&gt; of results.</string></returns>
        public static IList<string> GetReplacementParameters(this string source, string pattern = @"\${(.*?)\}")
        {

            List<string> ret = new List<string>();

            foreach(Match  m in Regex.Matches(source, pattern))
             {
             
                ret.Add(m.Value.Replace("${","").Replace("}",""));
            }
            return ret;
        }

        /// <summary>
        /// resolves all word replacements (${xxx}) in the string with corresponding values in the parameters list provided.
        /// </summary>
        /// <param name="criteria">the string containing work replacement requirements</param>
        /// <param name="parameters">key value pair of replacement values</param>
        /// <returns>resolved version of the criteria string</returns>
        public static string ResolveCriteria(this string criteria, Dictionary<string, string> parameters)
        {
            IList<string> parms = criteria.GetReplacementParameters();

            if (parms.Count() > 0)
            {
                if (parameters == null)
                    return "{}";
                if (parameters.Count() != parms.Count())
                    throw new Exception("the parameter requirements of the querydef getQuery do not match the number of parameters provided");
                for (int a = 0; a < parms.Count(); a++)
                {
                    var source = "${" + parms[a] + "}";
                    if (!parameters.ContainsKey(parms[a]))
                        throw new Exception("the required criteria key (" + parms[a] + ") was not found in the parameters provided");
                    criteria = criteria.Replace(source, parameters[parms[a]]);
                }
            }
            return criteria;
        }

        /// <summary>
        /// resolves all word replacements (${xxx}) in the string with corresponding values in the parameters list provided.
        /// </summary>
        /// <param name="criteria">the string containing work replacement requirements</param>
        /// <param name="parameters">key value pair of replacement values</param>
        /// <returns>resolved version of the criteria string</returns>
        public static string ResolveCriteria(this string criteria, JObject parameters)
        {
            IList<string> parms = criteria.GetReplacementParameters();

            if (parms.Count() > 0)
            {
                if (parameters == null)
                    return "{}";
                if (parameters.Properties().Count() != parms.Count())
                    throw new Exception("the parameter requirements of the querydef getQuery do not match the number of parameters provided");
                for (int a = 0; a < parms.Count(); a++)
                {
                    var source = "${" + parms[a] + "}";
                    if (!parameters.ContainsKey(parms[a]))
                        throw new Exception("the required criteria key (" + parms[a] + ") was not found in the parameters provided");
                    criteria = criteria.Replace(source, (string)parameters[parms[a]]);
                }

            }
            return criteria;
        }

        
    }
}
