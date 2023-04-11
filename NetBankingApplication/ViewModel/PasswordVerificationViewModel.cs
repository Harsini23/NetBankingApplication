using Library;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NetBankingApplication.ViewModel
{
    public class PasswordVerificationViewModel : PasswordVerificationBaseViewModel
    {
      private  CheckPassword _checkPassword;
        public override void CheckPassword(string userId, string password)
        {
            UserPasswordBObj cred = new UserPasswordBObj(userId, password);
            _checkPassword = new CheckPassword(new CheckPasswordRequest(cred), new PresenterCheckPasswordCallback(this));
            _checkPassword.Execute();
        }
    }
    public class PresenterCheckPasswordCallback : IPresenterCheckPasswordCallback
    {
        private PasswordVerificationViewModel _passwordVerificationViewModel;

        public PresenterCheckPasswordCallback(PasswordVerificationViewModel passwordVerificationViewModel)
        {
            this._passwordVerificationViewModel = passwordVerificationViewModel;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<bool> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data)
                {
                    //validated
                    _passwordVerificationViewModel.ResponseValue = response.Response;
                    _passwordVerificationViewModel.settingsView?.TriggerResetPasswordPopup();
                    _passwordVerificationViewModel.settingsView?.RemoveErrors();
                  
                }
                else
                {
                    //wrong password
                    _passwordVerificationViewModel.TextBoxVisibility = Visibility.Visible;
                    _passwordVerificationViewModel.ResponseValue = "Oops! incorrect password,try again";
                }
            });
        }
    }

    public abstract class PasswordVerificationBaseViewModel : NotifyPropertyBase
    {
        public abstract void CheckPassword(string userId,string password);
        public ISettingsView settingsView { get; set; }
       // public IPasswordConfirmationView 

        private string _response = String.Empty;
        public string ResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(ResponseValue));
            }
        }



        private Visibility _textBoxVisibility = Visibility.Collapsed;
        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            set
            {
                _textBoxVisibility = value;
                OnPropertyChanged(nameof(TextBoxVisibility));

            }
        }

    }
    public interface ISettingsView
    {
         void TriggerResetPasswordPopup();
        void RemoveErrors();
      
    }

   
}
