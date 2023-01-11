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

namespace NetBankingApplication
{
    public sealed partial class MainPage : Page, IMainPageNavigation
    {
        private LoginBaseViewModel LoginViewModel;
        PresenterService LoginVMserviceProviderInstance;
        public MainPage()
        {
            Debug.WriteLine(Application.Current.Resources);
            this.InitializeComponent();
            LoginVMserviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = LoginVMserviceProviderInstance.Services.GetService<LoginBaseViewModel>();
            LoginViewModel.MainPageNavigationCallback = this;
        }

        public void NavigateToDashBoard(User user)
        {
            LoadContentFrame.Navigate(typeof(DashBoard),user);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
             LoadContentFrame.Navigate(typeof(LoginPage));
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
