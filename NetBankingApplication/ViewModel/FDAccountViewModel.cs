using Library;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using Library.Util.FDCalculator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class FDAccountViewModel : FDAccountBaseViewModel
    {
        GetFdRate FdRate;
        OpenFD OpenFD;
       public FDBObj FDBObj;
        public override void CalculateFD(double principle,int year,int month,int day, CustomerType customerType,FDType fDType, string userID,string FromAccountNumber)
        {
            //fetch rates and on call back perform FD calculation
            // CalculatedFd = FDCalculator.calculate(principle, 5.75, DaysCalculator.ConvertIntoDays(year,month,day));
            FDBObj = new FDBObj{
                UserID = userID,
                CustomerType = customerType,
                TenureDate = DaysCalculator.ConvertIntoDays(year, month, day),
                PrincipalAmount = principle,
                FDType = fDType,
                FromAccountNumber=FromAccountNumber
            };
            FdRate = new GetFdRate(new FDRateRequest(FDBObj, new CancellationTokenSource()),new PresenterFDAccountRateCallback(this));
            FdRate.Execute();
        }

        public override void CreateFD(FDCalculatedVobj fDCalculatedBobj, FDBObj fDBObj)
        {
            OpenFD = new OpenFD(new OpenFDRequest(fDCalculatedBobj, fDBObj,new CancellationTokenSource()), new PresenterOpenFDCallback(this));
            OpenFD.Execute();
        }
    }

    public class PresenterFDAccountRateCallback : IPresenterGetFDRateCallback
    {
        private FDAccountViewModel fDAccountViewModel;

        public PresenterFDAccountRateCallback(FDAccountViewModel fDAccountViewModel)
        {
            this.fDAccountViewModel = fDAccountViewModel;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<GetFDRateResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<GetFDRateResponse> response)
        {
            if (fDAccountViewModel.OpenAccount)
            {
                fDAccountViewModel.CreateFD(response.Data.FDDetails, fDAccountViewModel.FDBObj);
            }
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                fDAccountViewModel.CalculatedFd = response.Data.FDDetails;
            });
        }
    }

    public class PresenterOpenFDCallback : IPresenterOpenFDCallback
    {
        private FDAccountViewModel fDAccountViewModel;

        public PresenterOpenFDCallback(FDAccountViewModel fDAccountViewModel)
        {
            this.fDAccountViewModel = fDAccountViewModel;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<GetFDRateResponse> response)
        {
            throw new NotImplementedException();
        }

        public void OnSuccessAsync(ZResponse<GetFDRateResponse> response)
        {
          
        }
    }


    public abstract class FDAccountBaseViewModel : NotifyPropertyBase
    {
        public abstract void CalculateFD(double principle, int year, int month, int day, CustomerType customerType, FDType fDType, string userdId, string Account="");

        public bool OpenAccount;
        public abstract void CreateFD(FDCalculatedVobj fDCalculatedBobj,FDBObj fDBObj);

        private FDCalculatedVobj _calculatedFd =null;
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
