﻿using Library;
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
        private GetFdRate _fdRate;
        private OpenFD _openFD;
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
            _fdRate = new GetFdRate(new FDRateRequest(FDBObj, new CancellationTokenSource()),new PresenterFDAccountRateCallback(this));
            _fdRate.Execute();
        }

        public override void CreateFD(FDAccountBObj fdAccount)
        {
            _openFD = new OpenFD(new OpenFDRequest(fdAccount,new CancellationTokenSource()), new PresenterOpenFDCallback(this));
            _openFD.Execute();
        }
    }

    public class PresenterFDAccountRateCallback : IPresenterGetFDRateCallback
    {
        private FDAccountViewModel _fDAccountViewModel;

        public PresenterFDAccountRateCallback(FDAccountViewModel fDAccountViewModel)
        {
            this._fDAccountViewModel = fDAccountViewModel;
        }

        public async void OnError(BException errorMessage)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _fDAccountViewModel.NotificationMessage = errorMessage.exceptionMessage;
                _fDAccountViewModel.NotificationAlert?.CallNotification();
            });
        }

        public void OnFailure(ZResponse<GetFDRateResponse> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<GetFDRateResponse> response)
        {
            if (_fDAccountViewModel.OpenAccount)
            {
                _fDAccountViewModel.CreateFD(new FDAccountBObj
                {
                    TenureDate=response.Data.FDDetails.MaturityDate,
                    MaturityAmount=response.Data.FDDetails.MaturityAmount,
                    FDType= _fDAccountViewModel.FDBObj.FDType,
                    CustomerType= _fDAccountViewModel.FDBObj.CustomerType,
                    FromAccount= _fDAccountViewModel.FDBObj.FromAccountNumber,
                    Principle= _fDAccountViewModel.FDBObj.PrincipalAmount,
                    Rate=response.Data.FDDetails.Rate,
                    InterestAmount=response.Data.FDDetails.InterestAmount,
                    UserID= _fDAccountViewModel.FDBObj.UserID
                });
            }
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _fDAccountViewModel.CalculatedFd = response.Data.FDDetails;
            });
        }
    }

    public class PresenterOpenFDCallback : IPresenterOpenFDCallback
    {
        private FDAccountViewModel _fDAccountViewModel;

        public PresenterOpenFDCallback(FDAccountViewModel fDAccountViewModel)
        {
            this._fDAccountViewModel = fDAccountViewModel;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _fDAccountViewModel.NotificationMessage = response.Response;
                _fDAccountViewModel.NotificationAlert?.CallNotification();
            });
        }
    }


    public abstract class FDAccountBaseViewModel : NotifyPropertyBase
    {
        public abstract void CalculateFD(double principle, int year, int month, int day, CustomerType customerType, FDType fDType, string userdId, string Account="");

        public bool OpenAccount;
        public abstract void CreateFD(FDAccountBObj fDAccountBObj);

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
        public INotificationAlert NotificationAlert { get; set; }

        private string _notificationMessage;
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set
            {
                _notificationMessage = value;
                OnPropertyChanged(nameof(NotificationMessage));
            }
        }

    }
}
