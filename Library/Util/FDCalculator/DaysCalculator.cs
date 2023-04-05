using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util.FDCalculator
{
    public static class DaysCalculator
    {
        public static int ConvertIntoDays(int year,int month,int days)
        {
            var MaturityDate = DateTime.Now.AddYears(year).AddMonths(month).AddDays(days);
            TimeSpan timeDifference = MaturityDate - DateTime.Now;
            return timeDifference.Days;
        }
    }
}
