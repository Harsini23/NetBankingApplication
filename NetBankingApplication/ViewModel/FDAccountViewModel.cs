using Library.Model;
using Library.Util.FDCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class FDAccountViewModel : FDAccountBaseViewModel
    {
        public override void CalculateFD(double principle,int year,int month,int day)
        {
            CalculatedFd = FDCalculator.calculate(principle, 5.75, DaysCalculator.ConvertIntoDays(year,month,day));
        }
        public override void CreateFD()
        {
        }
    }
    public abstract class FDAccountBaseViewModel : NotifyPropertyBase
    {
        public abstract void CalculateFD(double principle, int year, int month, int day);
        public abstract void CreateFD();

        private FDCalculatedVobj _calculatedFd ;
        public FDCalculatedVobj CalculatedFd
        {
            get
            {
                return this._calculatedFd;
            }
            set
            {
                _calculatedFd = value;
                OnPropertyChanged(nameof(CalculatedFd));
            }
        }

    }
}
