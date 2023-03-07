using CommunityToolkit.Mvvm.ComponentModel;
using Library.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public abstract class LoginBaseViewModel: NotifyPropertyBase
    {
     //   public string LoginResponse { get; set; }

        private string _response = String.Empty;
        public string LoginResponseValue
        {
            get { return this._response; }
            set
            {
                 _response = value;
                OnPropertyChanged(nameof(LoginResponseValue));
            }
        }

        public string ResetPasswordResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(ResetPasswordResponseValue));
            }
        }



        private bool _redirect = true;
        public bool Redirect
        {
            get { return this._redirect; }
            set
            {
                _redirect = value;
                OnPropertyChanged(nameof(Redirect));
            }
        }

        public ILoginViewModel LoginViewModelCallback { get; set; }
        public IMainPageNavigation MainPageNavigationCallback { get; set; }
        public ICloseAllWindows CloseAllWindowsCallback { get; set; }
        public abstract void ValidateUserInput(string userId, string password);
        public abstract void ResetPassword(string newPassword);
        public abstract void CallUseCase();
        public abstract void Logout();

        public abstract void CreateAdminAccount();
    }

    public interface ILoginViewModel
    {
        void SwitchToResetPasswordContainer();
        //set iloginview instance to service provider
    }
    public interface IMainPageNavigation
    {
        void NavigateToDashBoard(User user);
        void NavigateToAdminDashBoard(User user);
        void NavigateToLoginPage();
        //future logout change frame
    }

    public interface ICloseAllWindows
    {
        void closeAllWindows();
    }

   
}
