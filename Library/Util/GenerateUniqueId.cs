using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util
{
    public class GenerateUniqueId
    {
        public static string GetUniqueId(string preFix)
        {
            return preFix + Guid.NewGuid().ToString().Substring(0,10);
        }
    }
}
