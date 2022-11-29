using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util
{
    public static class GenerateUniqueId
    {
        public static string GetUniqueId(string preFix)
        {
            return preFix + Guid.NewGuid().ToString().Substring(0,10);
        }

        private static Random rng = new Random(Environment.TickCount);
        private static string GetUniquieNumber(object objlength)
        {
            String number = "";
            for (int index = 0; index < 20; index++)
            {
                int length = Convert.ToInt32(objlength);
                number = rng.NextDouble().ToString("0.000000000000").Substring(2, length);
            }
          //  Debug.WriteLine(number.ToString());
            return number.ToString();
        }



    }
}
