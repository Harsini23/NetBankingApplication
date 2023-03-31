﻿using Library;
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
       // public static User user;
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
   
            public async void OnFailure(ZResponse<bool> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    loginViewModel.ResetPasswordResponseValue = response.Response.ToString();
                });
            }

            public async void OnSuccessAsync(ZResponse<bool> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    loginViewModel.ResetPasswordResponseValue = response.Response.ToString();
                    var redirectToLogin = loginViewModel.Redirect;
                    //
                    if (redirectToLogin)
                    {
                        if (IsAdmin)
                        {
                            handleAdminAccess();
                            loginViewModel.LoginResponseValue = "";
                        }
                        else
                        {
                            LoadDashBoard(loginViewModel.CurrentUser);
                            loginViewModel.LoginResponseValue = "";

                        }
                    }
                    else
                    {
                        loginViewModel.LoginResponseValue = response.Response.ToString();
                        HandleClosePopUp();
                        //changes here
                        loginViewModel.settingsNotification?.ChangePasswordNotification();
                     }

                  
                });
                 
            }

            private void HandleClosePopUp()
            {
                loginViewModel.ClosePopUp?.closePopup();
            }
       
        
            private void LoadDashBoard(User user)
            {
                loginViewModel.mainPageNavigation?.NavigateToDashBoard();
            }

           
            private void handleCallbackAsync()
            {
                  loginViewModel.loginViewCallback?.SwitchToResetPasswordContainer();
            }
            private void handleAdminAccess()
            {
                  loginViewModel.mainPageNavigation?.NavigateToAdminDashBoard();
            }

            //Presenter call back methods -----------------------------------------------------------------------------------------------------------
       
            public async void OnFailure(ZResponse<LoginResponse> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    loginViewModel.TextBoxVisibility = Windows.UI.Xaml.Visibility.Visible;

                    loginViewModel.LoginResponseValue = response.Response.ToString();
                });
            }

            public async void OnError(BException errorMessage)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    //Block account
                    loginViewModel.TextBoxVisibility = Windows.UI.Xaml.Visibility.Visible;
                    loginViewModel.LoginResponseValue = errorMessage.exceptionMessage.ToString();
                });
            }

            public async void OnSuccessAsync(ZResponse<LoginResponse> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    LoginViewModel.IsAdmin = response.Data.IsAdmin;
                    loginViewModel.CurrentUser = response.Data.currentUser;
                    loginViewModel.TextBoxVisibility = Windows.UI.Xaml.Visibility.Collapsed;
                    //LoginViewModel.user = response.Data.currentUser;
                    if (response.Data.IsAdmin)
                    {
                        if (response.Data.NewUser)
                        {
                            //in vm base hv interface.. in abstract class hv the property of I and set it from view by passing this acces from VM by I so only interface functionalities are visibles
                            handleCallbackAsync();
                        }
                        else
                        {
                            handleAdminAccess();
                        }
                    }
                    else
                    {
                        // loginViewModel.LoginResponseValue = response.Response.ToString();
                        //redirect to next page with user details
                        if (response.Data.NewUser)
                        {
                            //in vm base hv interface.. in abstract class hv the property of I and set it from view by passing this acces from VM by I so only interface functionalities are visibles
                            handleCallbackAsync();
                        }
                        else
                        {
                            //then continue with user profile details display //pass user and id

                            //Debug.WriteLine(LoginViewModel.user.EmailId);
                            LoadDashBoard(loginViewModel.CurrentUser);
                        }

                    }
                });

            }
        }
    }
}