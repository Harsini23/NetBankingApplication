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
    public abstract class FDAccountDetailsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetFDDetails(String account);
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
