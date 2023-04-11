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
        private GetFDDetails _getFDDetail;
        private CloseFD _closeFd;

        public override void CloseFD(FDAccount account, string userId)
        {
            _closeFd = new CloseFD(new CloseFDRequest { UserId=userId,FDAccount= account,CtsSource= new CancellationTokenSource() },new CloseFDCallback(this));
            _closeFd.Execute();
        }

        public override void GetFDDetails(string account)
        {
            _getFDDetail = new GetFDDetails(new GetFDDetailsRequest(account, new CancellationTokenSource()), new GetFDDetailsCallback(this));
            _getFDDetail.Execute();
        }

        
    }

    public class GetFDDetailsCallback : IPresenterGetFDDetailsCallback
    {
        private FDAccountDetailsViewModel _fDAccountDetailsViewModel;

        public GetFDDetailsCallback(FDAccountDetailsViewModel fDAccountDetailsViewModel)
        {
            this._fDAccountDetailsViewModel = fDAccountDetailsViewModel;
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
                _fDAccountDetailsViewModel.CurrentFDAccount = response.Data;
            });
        }
    }

    public class CloseFDCallback : IPresenterCloseFDCallback
    {
        private FDAccountDetailsViewModel _fDAccountDetailsViewModel;

        public CloseFDCallback(FDAccountDetailsViewModel fDAccountDetailsViewModel)
        {
            this._fDAccountDetailsViewModel = fDAccountDetailsViewModel;
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
            //notification(control notification and to remove account) and close the new window manually
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _fDAccountDetailsViewModel.CloseWindow?.CloseWindow();
            });
        }
    }
    public abstract class FDAccountDetailsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetFDDetails(String account);
        public ICloseWindow CloseWindow { get; set; }
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

    public interface ICloseWindow
    {
        void CloseWindow();
    }
}
