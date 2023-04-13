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
using Microsoft.UI;          
using Windows.UI.ViewManagement;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.WindowManagement;


namespace NetBankingApplication
{
    public sealed partial class MainPage : Page, IMainPageNavigation
    {
        private LoginBaseViewModel _loginViewModel;
        private AppWindow _appWindow;
        public MainPage()
        {
         
            this.InitializeComponent();
            _loginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            _loginViewModel.MainPageNavigationCallback = this;
            _loginViewModel.CreateAdminAccount();

            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
         
            Windows.UI.Color colorFromBrush;
            if (this.Resources["SupportingColour"] is SolidColorBrush)
                colorFromBrush = (this.Resources["SupportingColour"] as SolidColorBrush).Color;

            titleBar.BackgroundColor = colorFromBrush;
            titleBar.ButtonBackgroundColor = colorFromBrush;

     
        }
      
        public void NavigateToDashBoard(User user)
        {
            LoadContentFrame.Navigate(typeof(DashBoard),user);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoginPage.SetUserUpdate += LoginPage_SetUserUpdate1; ;
            LoadContentFrame.Navigate(typeof(LoginPage));
        }

        private void LoginPage_SetUserUpdate1(User obj)
        {
            //pass it to dashboard
            var dashboardPage = new DashBoard();
            dashboardPage.User = obj;
            LoadContentFrame.Navigate(dashboardPage.GetType(), dashboardPage);
            //LoadContentFrame.Navigate(typeof(DashBoard), obj);
        }



        public void NavigateToLoginPage()
        {
            LoadContentFrame.Navigate(typeof(LoginPage));
        }

        //load admin authorized panel
        public void NavigateToAdminDashBoard()
        {
            LoadContentFrame.Navigate(typeof(AdminPage));
        }

    }

}
