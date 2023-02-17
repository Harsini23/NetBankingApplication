using Library.Model;
using System;
using NetBankingApplication.ViewModel;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using NetBankingApplication.View.UserControls;
using Windows.UI.ViewManagement;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class DashBoard : Page,INotifyPropertyChanged
    {

        private LoginBaseViewModel LoginViewModel;
        PresenterService LoginVMserviceProviderInstance;
        //private String OppositeTheme="Light mode";
        //private string OppositeThemeIcon = "";
       // Windows.UI.Xaml.ApplicationTheme currentTheme;
        public DashBoard()
        {
            this.InitializeComponent();
            UISettings uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged; ;

            // currentTheme = Application.Current.RequestedTheme;
            SwitchThemeUIValues();
            LoginVMserviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = LoginVMserviceProviderInstance.Services.GetService<LoginBaseViewModel>();

        }

        private void SwitchThemeUIValues()
        {
            if (ThemeSwitch.CurrentTheme == ElementTheme.Light)
            {
                OppositeTheme = "Dark mode";
                OppositeThemeIcon = "";
            }
            else if (ThemeSwitch.CurrentTheme == ElementTheme.Dark)
            {
                OppositeTheme = "Light mode";
                OppositeThemeIcon = "";
            }
        }
        private void UiSettings_ColorValuesChanged(UISettings sender, object args)
        {
            
            if((ThemeSwitch.CurrentTheme == ElementTheme.Light))
            {
                ThemeSwitch.CurrentTheme = ElementTheme.Dark;
            }
            else
            {
                ThemeSwitch.CurrentTheme = ElementTheme.Light;
            }
            SwitchThemeUIValues();

        }

        User Currentuser;

      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Currentuser = (User)e.Parameter;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Overview overview = new Overview(Currentuser);
          
            CurrentSelectedModule = overview;
            HeaderTitle = "Overview";
            DashBoardNavigation.SelectedItem = Overview;
            //_dashboardNavigationViewModel.SetOverview();
            // SelectedDashboardContentFrame.Navigate(typeof(Overview), Currentuser);
        }


        private void DashBoardNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        //    FrameNavigationOptions navOptions = new FrameNavigationOptions();
        //    navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
        //    if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
        //    {
        //        navOptions.IsNavigationStackEnabled = false;
        //    }

            Type pageType;

            if (args.SelectedItem == Overview)
            {

                Overview overview = new Overview(Currentuser);

                CurrentSelectedModule = overview;
                HeaderTitle = "Overview";
                DashBoardNavigation.AlwaysShowHeader = false;

                //_dashboardNavigationViewModel.SetOverview();
                //  pageType = typeof(Overview);
            }
            else if (args.SelectedItem == BankAccount)
            {
                //_dashboardNavigationViewModel.SetBankAccount();

                BankAccount bankAccount = new BankAccount(Currentuser.UserId);

                CurrentSelectedModule = bankAccount;
                HeaderTitle = "Account Details";
                DashBoardNavigation.AlwaysShowHeader = true;
                //  pageType = typeof(BankAccount);
            }
            else if (args.SelectedItem == PaymentsAndTransfer)
            {
                PaymentsAndTransfer paymentsAndTransfer = new PaymentsAndTransfer(Currentuser);
                CurrentSelectedModule = paymentsAndTransfer;
                // pageType = typeof(PaymentsAndTransfer);
                HeaderTitle = "Payment and Transfer";
                DashBoardNavigation.AlwaysShowHeader = true;
            }
            else if (args.SelectedItem == Settings)
            {
                HeaderTitle = "Settings";
              //  pageType = typeof(CardAndLoans);
            }
            else
            {
                HeaderTitle = "Overview";
                DashBoardNavigation.AlwaysShowHeader = false;
                //   pageType = typeof(Overview);
            }

            //SelectedDashboardContentFrame.NavigateToType(pageType, null, navOptions);

        }

        //notify events for navigation change

        public event PropertyChangedEventHandler PropertyChanged;

        private UserControl _currentSelectedModule;
        public UserControl CurrentSelectedModule
        {
            get { return _currentSelectedModule; }
            set { _currentSelectedModule = value;
                NotifyPropertyChanged();
            }
        }
        private String _headerTitle="OverView";
        public String HeaderTitle
        {
            get { return _headerTitle; }
            set
            {
                _headerTitle=value;
                NotifyPropertyChanged();
            }
        }


        private String _oppositeTheme;
        public String OppositeTheme
        {
            get { return _oppositeTheme; }
            set
            {
                _oppositeTheme = value;
                NotifyPropertyChanged();
            }
        }
        private String _oppositeThemeIcon;
        public String OppositeThemeIcon
        {
            get { return _oppositeThemeIcon; }
            set
            {
                _oppositeThemeIcon = value;
                NotifyPropertyChanged();
            }
        }

        private async void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
              });
        }

        private void Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //logout logic
            LoginViewModel.Logout();
        }

        private async void ThemeChange_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ThemeSwitch.CurrentTheme == ElementTheme.Dark)
            {
                await ThemeSwitch.ChangeTheme(ElementTheme.Light);
                ThemeSwitch.CurrentTheme = ElementTheme.Light;
            }
            else
            {
                await ThemeSwitch.ChangeTheme(ElementTheme.Dark);
                ThemeSwitch.CurrentTheme = ElementTheme.Dark;
            }
            SwitchThemeUIValues();
        }
    }
}
