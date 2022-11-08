using Library.Data.DataManager;using Library.Domain;using Library.Domain.UseCase;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
using System;using System.Collections.Generic;using System.ComponentModel;using System.Diagnostics;using System.Linq;using System.Runtime.CompilerServices;
using System.Text;using System.Threading.Tasks;using static Library.Domain.Login;namespace NetBankingApplication.ViewModel{

    internal class LoginViewModel : LoginBaseViewModel    {

        Login login;
        ResetPassword resetPassword;
        public string userId, password, resetNewPassword;
        public static User user;

        ILoginViewModel loginViewCallback;
        IMainPageNavigation mainPageNavigation;


        public override void CallUseCase()        {
            SetValueForCallback();
            login = new Login(new UserLoginRequest(userId, password), new PresenterLoginCallback(this));
            login.Execute();        }        public void CallResetUseCase()
        {

            resetPassword = new ResetPassword(new ResetPasswordRequest(userId, resetNewPassword), new PresenterLoginCallback(this));
            resetPassword.Execute();
        }
        private void SetValueForCallback()
        {
            loginViewCallback = LoginViewModelCallback;
            mainPageNavigation = MainPageNavigationCallback;
        }

        public override void ValidateUserInput(string userId, string password)
        {
            this.userId = userId;            this.password = password;
            //check password specification and UI logic is correct, or if needed - set binded value proceed to call use case or handle UI error
            CallUseCase();
        }

        public override void ResetPassword(string newPassword)
        {
            this.resetNewPassword = newPassword;
            CallResetUseCase();
        }

        public class PresenterLoginCallback : IPresenterLoginCallback, IPersenterResetPasswordCallback        {            private LoginViewModel loginViewModel;
            // private ILoginViewModel loginViewCallback;

            // private ILoginViewModel loginVMCallbackSwitch;
            public PresenterLoginCallback()
            {

            }            public PresenterLoginCallback(LoginViewModel loginViewModel)            {                this.loginViewModel = loginViewModel;

            }
            public void BlockAccount(ZResponse<LoginResponse> response)            {                loginViewModel.LoginResponseValue = response.Response.ToString();            }            public void LoginFailed(ZResponse<LoginResponse> response)            {                loginViewModel.LoginResponseValue = response.Response.ToString();            }

            public void ResetPasswordFailure(ZResponse<bool> response)
            {
                loginViewModel.ResetPasswordResponseValue = response.Response.ToString();
            }

            public void ResetPasswordSuccess(ZResponse<bool> response)
            {
                loginViewModel.ResetPasswordResponseValue = response.Response.ToString();
            }
            void IPresenterLoginCallback.VerfiedUserAsync(ZResponse<LoginResponse> response)
            {
                loginViewModel.LoginResponseValue = response.Response.ToString();
                //redirect to next page with user details
                if (response.Data.NewUser)
                {
                    //in vm base hv interface.. in abstract class hv the property of I and set it from view by passing this acces from VM by I so only interface functionalities are visibles
                    handleCallbackAsync();
                }

                //then continue with user profile details display //pass user and id
                LoginViewModel.user = response.Data.currentUser;
                //Debug.WriteLine(LoginViewModel.user.EmailId);
                LoadDashBoard();


            }
            private async Task LoadDashBoard()
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                loginViewModel.mainPageNavigation.NavigateToDashBoard();
            });

            }

            private async Task handleCallbackAsync()
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  loginViewModel.loginViewCallback.SwitchToResetPasswordContainer();
              });
            }
        }
    }
}