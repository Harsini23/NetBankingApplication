using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util
{
    public static class GenerateUniqueId
    {
        public static string GetUniqueId(string preFix="")
        {
            var id = preFix + Guid.NewGuid().ToString().Substring(0, 9);
            return id.Replace(@"-", string.Empty);
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

        public static string GeneratePassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(3, true));
            builder.Append(RandomNumber(100, 999));
            builder.Append(RandomString(2, false));
            builder.Append(SpecialCharacter());
            return builder.ToString();
          
        }
        public static string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        public static int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string SpecialCharacter()
        {
            string spl = "%!@#$%^&*()?/>.<,:;'}]{[_~`+=-";
            Random random = new Random();
            string res="";
          
            for (int i = 0; i < 3; i++)
            {
                res+= spl[random.Next(0, spl.Length)];
            }
            return res;
        }


    }
}
