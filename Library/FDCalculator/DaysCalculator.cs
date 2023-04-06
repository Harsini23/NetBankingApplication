using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util.FDCalculator
{
    public static class DaysCalculator
    {
        public static string ConvertIntoDays(int year,int month,int days)
        {
            var MaturityDate = DateTime.Now.AddYears(year).AddMonths(month).AddDays(days);
            
            return MaturityDate.ToString("d-M-yyyy");
        }
    }
}
