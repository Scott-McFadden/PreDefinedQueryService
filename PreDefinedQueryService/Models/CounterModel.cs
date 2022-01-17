
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreDefinedQueryService.Models
{
    public static class Counters
    {
        public static int calls = 0;
        public static int errors = 0;


        public static void Inc() => Counters.calls++;
        public static void err() => Counters.errors++;
        public static readonly DateTime started = DateTime.UtcNow;
        public static TimeSpan UpTime { get =>    DateTime.UtcNow - started; }
      
        public static Dictionary<string,string> Results(){
            var ret = new Dictionary <string, string>();

            ret.Add("calls", Counters.calls.ToString());
            
             
            ret.Add( "errors", Counters.errors.ToString() );
            ret.Add( "UpTime", Counters.UpTime.ToString() );
            ret.Add("started", Counters.started.ToString());
            return ret;

        }
    }
}
