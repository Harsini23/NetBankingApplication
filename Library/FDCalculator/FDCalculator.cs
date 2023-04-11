using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util.FDCalculator
{
   
    public class StandardFDCalculator : IFDCalculator
    {
        public FDCalculatedVobj Calculate(double principle, double rate, int days)
        {
            var interestAmount = principle * (rate/100) * days / 365;
            return new FDCalculatedVobj
            {
                InterestAmount = Math.Round(interestAmount,2),
                MaturityDate = DateTime.Now.AddDays(days).ToString("d-M-yyyy"),
                MaturityAmount = Math.Round(interestAmount+principle,2),
                Rate = rate
            };
        }
    }


    public class SeniorCitizenFDCalculator : IFDCalculator
    {
        public FDCalculatedVobj Calculate(double principle, double rate, int days)
        {
            //varied FD logic based on type of account or type of FD maturity plan
            rate += 1.5;
            var interestAmount = principle * (rate / 100) * days / 366;
            return new FDCalculatedVobj
            {
                InterestAmount = Math.Round(interestAmount, 2),
                MaturityDate = DateTime.Now.AddDays(days).ToString("d-M-yyyy"),
                MaturityAmount = Math.Round(interestAmount + principle, 2),
                Rate = rate
            };
        }
    }
}
