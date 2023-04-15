using CommunityToolkit.Mvvm.ComponentModel;
using Library.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

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

        private User _currentUser;
        public User CurrentUser
        {
            get { return this._currentUser; }
            set
            {
               _currentUser = value;
                OnPropertyChanged(nameof(_currentUser));
            }
        }

        private string _resetPasswordResponseValue;
        public string ResetPasswordResponseValue
        {
            get { return this._resetPasswordResponseValue; }
            set
            {
                _resetPasswordResponseValue = value;
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
        //public IMainPageNavigation MainPageNavigationCallback { get; set; }
        public ICloseAllWindows CloseAllWindowsCallback { get; set; }
        public IChangePasswordNotification settingsNotification { get; set; }
        public IClosePopUp ClosePopUp { get; set; }
        public abstract void ValidateUserInput(string userId, string password);
       // public abstract void ResetPassword(string newPassword, string userId);
        public abstract void CallUseCase();
        public abstract void Logout();

        public abstract void CreateAdminAccount();
    }

    public interface ILoginViewModel
    {
        void SwitchToResetPasswordContainer();
        void SetUser(User user);
        void NavigateToAdmin(Admin admin);
        //set iloginview instance to service provider
    }
    //public interface IMainPageNavigation
    //{
    //    void NavigateToDashBoard(User user);
    //    void NavigateToAdminDashBoard();
    //    void NavigateToLoginPage();
    //    //future logout change frame
    //}

    public interface ICloseAllWindows
    {
        void closeAllWindows();
    }

    public interface IClosePopUp
    {
        void closePopup();
    }

    public interface IChangePasswordNotification
    {
        void ChangePasswordNotification();
    }

}
