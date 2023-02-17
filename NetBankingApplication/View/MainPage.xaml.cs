using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
using System.ComponentModel;
using System.Diagnostics;
using Library.Model;
using Library;

namespace NetBankingApplication
{
    public sealed partial class MainPage : Page, IMainPageNavigation
    {
        private LoginBaseViewModel LoginViewModel;
        PresenterService LoginVMserviceProviderInstance;
        LibraryInitialization libraryInitialization;
        public MainPage()
        {
            libraryInitialization = LibraryInitialization.GetInstance();
            libraryInitialization.InitializeDb();
            this.InitializeComponent();
            LoginVMserviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = LoginVMserviceProviderInstance.Services.GetService<LoginBaseViewModel>();
            LoginViewModel.MainPageNavigationCallback = this;
            LoginViewModel.CreateAdminAccount();
        }

        public void NavigateToDashBoard(User user)
        {
            LoadContentFrame.Navigate(typeof(DashBoard),user);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //LoadContentFrame.Navigate(typeof(LoginPage));
            User user = new User
            {
                UserId = "UID6df59172",
                UserName = "Rachel",
                MobileNumber = 9872634150,
                EmailId = "rachelgreeen@gmail.com",
                IsBlocked = false,
                PAN = "18DF34D34S"
            };
            LoadContentFrame.Navigate(typeof(DashBoard), user);
        }


        public void NavigateToLoginPage()
        {
            LoadContentFrame.Navigate(typeof(LoginPage));
        }

        //load admin authorized panel
        public void NavigateToAdminDashBoard(User user)
        {
            LoadContentFrame.Navigate(typeof(AdminPage));
        }
    }

}
