using Library;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class FDAccountDetailsViewModel : FDAccountDetailsBaseViewModel
    {
        GetFDDetails GetFDDetail;
        CloseFD CloseFd;

        public override void CloseFD(FDAccount account, string userId)
        {
            CloseFd = new CloseFD(new CloseFDRequest { UserId=userId,FDAccount= account,CtsSource= new CancellationTokenSource() },new CloseFDCallback(this));
            CloseFd.Execute();
        }

        public override void GetFDDetails(string account)
        {
            GetFDDetail = new GetFDDetails(new GetFDDetailsRequest(account, new CancellationTokenSource()), new GetFDDetailsCallback(this));
            GetFDDetail.Execute();
        }

        
    }

    public class GetFDDetailsCallback : IPresenterGetFDDetailsCallback
    {
        private FDAccountDetailsViewModel fDAccountDetailsViewModel;

        public GetFDDetailsCallback(FDAccountDetailsViewModel fDAccountDetailsViewModel)
        {
            this.fDAccountDetailsViewModel = fDAccountDetailsViewModel;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<FDAccount> response)
        {
            throw new NotImplementedException();
        }

        public async void OnSuccessAsync(ZResponse<FDAccount> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                fDAccountDetailsViewModel.CurrentFDAccount = response.Data;
            });
        }
    }

    public class CloseFDCallback : IPresenterCloseFDCallback
    {
        private FDAccountDetailsViewModel fDAccountDetailsViewModel;

        public CloseFDCallback(FDAccountDetailsViewModel fDAccountDetailsViewModel)
        {
            this.fDAccountDetailsViewModel = fDAccountDetailsViewModel;
        }

        public void OnError(BException errorMessage)
        {
            throw new NotImplementedException();
        }

        public void OnFailure(ZResponse<bool> response)
        {
            throw new NotImplementedException();
        }

        public void OnSuccessAsync(ZResponse<bool> response)
        {
           //notification(control notification and to remove account) and close the new window manually
        }
    }
    public abstract class FDAccountDetailsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetFDDetails(String account);
        public abstract void CloseFD(FDAccount account,string userId);
        private FDAccount _fDAccount ;
        public FDAccount CurrentFDAccount
        {
            get { return this._fDAccount; }
            set
            {
                _fDAccount = value;
                OnPropertyChanged(nameof(CurrentFDAccount));
            }
        }

    }
}
