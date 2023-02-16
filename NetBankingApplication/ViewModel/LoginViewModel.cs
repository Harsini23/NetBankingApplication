using Library;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.Login;

namespace NetBankingApplication.ViewModel
{

    internal class LoginViewModel : LoginBaseViewModel
    {

        Login login;
        DefaultAdmin defaultAdmin;
        ResetPassword resetPassword;
        public string userId, password, resetNewPassword;
        public static User user;
        public static bool IsAdmin;

        ILoginViewModel loginViewCallback;
        IMainPageNavigation mainPageNavigation;
        ICloseAllWindows closeAllWindowsCallback;


        public override void CallUseCase()
        {
            SetValueForCallback();
            login = new Login(new UserLoginRequest(userId, password, new CancellationTokenSource()), new PresenterLoginCallback(this));
            login.Execute();
        }
        public void CallResetUseCase()
        {
            resetPassword = new ResetPassword(new ResetPasswordRequest(userId, resetNewPassword,new CancellationTokenSource()), new PresenterLoginCallback(this));
            resetPassword.Execute();
           //call to display admin or user dashboard
        }

        private void SetValueForCallback()
        {
            loginViewCallback = LoginViewModelCallback;
            mainPageNavigation = MainPageNavigationCallback;
            closeAllWindowsCallback=CloseAllWindowsCallback;
        }

        public override void ValidateUserInput(string userId, string password)
        {
            this.userId = userId;
            this.password = password;
            //check password specification and UI logic is correct, or if needed - set binded value proceed to call use case or handle UI error
            CallUseCase();
        }

        public override void CreateAdminAccount()
        {
            defaultAdmin = new DefaultAdmin();
            defaultAdmin.Execute();
        }

        public override void ResetPassword(string newPassword)
        {
            this.resetNewPassword = newPassword;
            CallResetUseCase();
            
        }

        public override async void Logout()
        {
             LoggingOut();
            //call allAccountsPreview view to close all new windows
            this.closeAllWindowsCallback?.closeAllWindows();
        }

        private async Task LoggingOut()
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                       Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                       {
                           this.mainPageNavigation.NavigateToLoginPage();
                       });
        }


        public class PresenterLoginCallback : IPresenterLoginCallback, IPersenterResetPasswordCallback
        {
            private LoginViewModel loginViewModel;
            // private ILoginViewModel loginViewCallback;

            // private ILoginViewModel loginVMCallbackSwitch;
            public PresenterLoginCallback()
            {

            }
            public PresenterLoginCallback(LoginViewModel loginViewModel)
            {
                this.loginViewModel = loginViewModel;

            }
   
            public void OnFailure(ZResponse<bool> response)
            {
                loginViewModel.ResetPasswordResponseValue = response.Response.ToString();
            }

            public async void OnSuccess(ZResponse<bool> response)
            {
                loginViewModel.ResetPasswordResponseValue = response.Response.ToString();
                //
                if (IsAdmin)
                {
                   await handleAdminAccess();
                }
                else
                {
                  await  LoadDashBoard(LoginViewModel.user);
                }
            }
       
        
            private async Task LoadDashBoard(User user)
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                loginViewModel.mainPageNavigation.NavigateToDashBoard(LoginViewModel.user);
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
            private async Task handleAdminAccess()
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  loginViewModel.mainPageNavigation.NavigateToAdminDashBoard(LoginViewModel.user);
              });
            }

            //Presenter call back methods -----------------------------------------------------------------------------------------------------------
            public async void OnSuccess(ZResponse<LoginResponse> response)
            {
                LoginViewModel.IsAdmin = response.Data.IsAdmin;
                LoginViewModel.user = response.Data.currentUser;
                if (response.Data.IsAdmin)
                {
                    if (response.Data.NewUser)
                    {
                        //in vm base hv interface.. in abstract class hv the property of I and set it from view by passing this acces from VM by I so only interface functionalities are visibles
                      await  handleCallbackAsync();
                    }
                    else
                    {
                       await handleAdminAccess();
                    }
                }
                else
                {
                    // loginViewModel.LoginResponseValue = response.Response.ToString();
                    //redirect to next page with user details
                    if (response.Data.NewUser)
                    {
                        //in vm base hv interface.. in abstract class hv the property of I and set it from view by passing this acces from VM by I so only interface functionalities are visibles
                      await  handleCallbackAsync();
                    }
                    else
                    {
                        //then continue with user profile details display //pass user and id
                      
                        //Debug.WriteLine(LoginViewModel.user.EmailId);
                       await LoadDashBoard(LoginViewModel.user);
                    }

                }

            }

         
            public void OnFailure(ZResponse<LoginResponse> response)
            {
                loginViewModel.LoginResponseValue = response.Response.ToString();
            }

            public void OnError(BException errorMessage)
            {
                //Block account
                loginViewModel.LoginResponseValue = errorMessage.ToString();
            }
        }
    }
}