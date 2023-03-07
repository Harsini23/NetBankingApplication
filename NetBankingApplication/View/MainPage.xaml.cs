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
using Microsoft.UI;           // Needed for WindowId.
using Windows.UI.ViewManagement;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI.WindowManagement;
//using Microsoft.UI.Windowing; // Needed for AppWindow.
//using WinRT.Interop;          // Needed for XAML/HWND interop

namespace NetBankingApplication
{
    public sealed partial class MainPage : Page, IMainPageNavigation
    {
        private LoginBaseViewModel LoginViewModel;
        LibraryInitialization libraryInitialization;
        private AppWindow m_AppWindow;
        public MainPage()
        {
            libraryInitialization = LibraryInitialization.GetInstance();
            libraryInitialization.InitializeDb();
            this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            LoginViewModel.MainPageNavigationCallback = this;
            LoginViewModel.CreateAdminAccount();


            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.Auto;
            //ApplicationView.GetForCurrentView().Title = "TEST";
            ApplicationViewTitleBar titleBar = ApplicationView.GetForCurrentView().TitleBar;
           // Windows.UI.Color color = ColorHelper.ToColor(Application.Current.Resources["SupportingColour"].ToString());

            //var brush = this.Resources["SupportingColour"];
            Windows.UI.Color colorFromBrush;
            if (this.Resources["SupportingColour"] is SolidColorBrush)
                colorFromBrush = (this.Resources["SupportingColour"] as SolidColorBrush).Color;

            titleBar.BackgroundColor = colorFromBrush;
            titleBar.ButtonBackgroundColor = colorFromBrush;

            //IntPtr hWnd = WindowNative.GetWindowHandle(this);
            //WindowId wndId = Win32Interop.GetWindowIdFromWindow(hWnd);
            //var m_AppWindow= AppWindow.GetFromWindowId(wndId);

            //m_AppWindow.Title = "App title";
            //m_AppWindow.TitleBar.BackgroundColor = color;
        }
      
        public void NavigateToDashBoard(User user)
        {
            LoadContentFrame.Navigate(typeof(DashBoard),user);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContentFrame.Navigate(typeof(LoginPage));
            //User user = new User
            //{
            //    UserId = "UID6df59172",
            //    UserName = "Rachel",
            //    MobileNumber = 9872634150,
            //    EmailId = "rachelgreeen@gmail.com",
            //    IsBlocked = false,
            //    PAN = "18DF34D34S"
            //};
            //LoadContentFrame.Navigate(typeof(DashBoard), user);


            //User user = new User
            //{
            //    UserId = "UID6c1e8228",
            //    UserName = "Monica",
            //    MobileNumber = 9872523511,
            //    EmailId = "monica@gmail.com",
            //    IsBlocked = false,
            //    PAN = "2V342WS424"
            //};
            //LoadContentFrame.Navigate(typeof(DashBoard), user);
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
