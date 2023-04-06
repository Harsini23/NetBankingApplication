using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util.FDCalculator
{
    public class FDFactory
    {
        public IFDCalculator CreateFDCalculation(CustomerType customerType)
        {
            switch (customerType)
            {
                case CustomerType.Normal:
                    return new StandardFDCalculator();
                case CustomerType.SeniorCitizen:
                    return new SeniorCitizenFDCalculator();
                default:
                    throw new ArgumentException("Invalid customer type.");
            }
        }
    }
}
