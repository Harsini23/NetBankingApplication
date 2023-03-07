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
    public class ResetPasswordViewModel: ResetPasswordBaseViewModel
    {
        ResetPassword resetPassword;
        public string userId, resetNewPassword;
        private bool redirectToLogin;
       
        public override void ResetPassword(string newPassword,User user,bool redirectToLogin)
        {
            this.resetNewPassword = newPassword;
            this.redirectToLogin= redirectToLogin;
            resetPassword = new ResetPassword(new ResetPasswordRequest(userId, resetNewPassword, new CancellationTokenSource()), new PresenterResetLoginCallback(this));
            resetPassword.Execute();

        }


        public class PresenterResetLoginCallback : IPersenterResetPasswordCallback
        {
            ResetPasswordViewModel resetpassword;
            public PresenterResetLoginCallback(ResetPasswordViewModel resetpassword)
            {
                this.resetpassword = resetpassword;
            }

            public void OnError(BException errorMessage)
            {
               // throw new NotImplementedException();
            }

            public async void OnFailure(ZResponse<bool> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                 //   resetpassword.ResetPasswordResponseValue = response.Response.ToString();
                });
            }

            public async void OnSuccessAsync(ZResponse<bool> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    resetpassword.ResetPasswordResponseValue = response.Response.ToString();
                    if (IsAdmin)
                    {
                        handleAdminAccess();
                    }
                    else
                    {
                        LoadDashBoard(LoginViewModel.user);
                    }
                }); 
            }
        }
    }


    public abstract class ResetPasswordBaseViewModel: NotifyPropertyBase
    {
        public abstract void ResetPassword(string newPassword,User userId,bool redirectToLogin);
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
}
