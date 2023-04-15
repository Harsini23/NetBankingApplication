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
    public class ResetpasswordViewModel : ResetPasswordBaseViewModel

    {
        ResetPassword resetPassword;
        public string userId, resetNewPassword;
        private bool redirectToLogin;

        public override void ResetPassword(string newPassword, string userId, bool redirectToLogin)
        {
            this.resetNewPassword = newPassword;
            this.redirectToLogin = redirectToLogin;
            resetPassword = new ResetPassword(new ResetPasswordRequest(userId, resetNewPassword, new CancellationTokenSource()), new PresenterResetLoginCallback(this));
            resetPassword.Execute();
        }

        public class PresenterResetLoginCallback : IPersenterResetPasswordCallback
        {
            ResetpasswordViewModel resetpassword;
            public PresenterResetLoginCallback(ResetpasswordViewModel resetpassword)
            {
                this.resetpassword = resetpassword;
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
                    resetpassword.ResetPasswordResponseValue = response.Response.ToString();
                    resetpassword.ResetRedirection?.NavigateAfterReset(response.Response.ToString());
                });
            }
        }
    }

    public abstract class ResetPasswordBaseViewModel : NotifyPropertyBase
    {
        public abstract void ResetPassword(string newPassword, string userId, bool redirectToLogin);

        public IResetRedirection ResetRedirection { get; set; }
        private string _response;
        public string ResetPasswordResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(ResetPasswordResponseValue));
            }
        }
    }
    public interface IResetRedirection
    {
        void NavigateAfterReset(string response);
    }

}
