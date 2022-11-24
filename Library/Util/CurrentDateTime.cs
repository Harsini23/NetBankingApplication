using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util
{
    public class CurrentDateTime
    {
        //making it singleton?
        public static String GetCurrentDate()
        {
            var now = DateTime.Now.ToString("d-M-yyyy h:mm tt");
            return now.ToString();
        }
      
    }
}
