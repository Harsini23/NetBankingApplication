using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Util.FDCalculator
{
    public interface IFDCalculator
    {
        FDCalculatedVobj calculate(double principle, double rate, int days);
    }
}
