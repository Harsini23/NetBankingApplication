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
using Library.BankingNotification;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class DashBoard : Page,INotifyPropertyChanged
    {

        private LoginBaseViewModel _loginViewModel;
        public static event Action RaiseLogoutNotification;
        public static event Action RaiseCloseWindowNotification;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(DashBoard), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public DashBoard()
        {
            this.InitializeComponent();
            Bindings.Update();
            BankingNotification.UserUpdated += BankingNotification_UserUpdated;
            UISettings uiSettings = new UISettings();
            uiSettings.ColorValuesChanged += UiSettings_ColorValuesChanged;
            
            SwitchThemeUIValues();
            _loginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
        }

        private async void BankingNotification_UserUpdated(User user)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                User = user;
            });
        }

        private void SwitchThemeUIValues()
        {
            if (ThemeSwitch.CurrentTheme == ElementTheme.Light)
            {
                OppositeTheme = "Dark mode";
                OppositeThemeIcon = "";
            }
            else if (ThemeSwitch.CurrentTheme == ElementTheme.Dark)
            {
                OppositeTheme = "Light mode";
                OppositeThemeIcon = "";
             
            }
        }
        private void UiSettings_ColorValuesChanged(UISettings sender, object args)
        {
            ThemeSwitch.CurrentTheme = ThemeSwitch.CurrentTheme == ElementTheme.Light ? ElementTheme.Dark : ElementTheme.Light;
            SwitchThemeUIValues();
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is DashBoard dashboard && dashboard.User != null)
            {
                User = dashboard.User;
                // Do something with the user object
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DashBoardNavigation.SelectedItem = Overview;
        }


        private void DashBoardNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
        //    DashBoard dashboard = new DashBoard();
        //    dashboard.User = this.User;// Set a non-null User property

            // Set the DataContext of the NavigationContentControl to mainViewModel 
            NavigationContentControl.DataContext = this;

            if (args.SelectedItem == Overview)
            {
                //set currentUser as depedency property

                //Overview overview = new Overview();
                //overview.User = CurrentUser;
                //CurrentSelectedModule = overview;
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate1"]).LoadContent();
                HeaderTitle = "Overview";
                DashBoardNavigation.AlwaysShowHeader = false;
            }
            else if (args.SelectedItem == BankAccount)
            {
                //BankAccount bankAccount = new BankAccount();
                //bankAccount.User = CurrentUser;
                //CurrentSelectedModule = bankAccount;
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate2"]).LoadContent();

                HeaderTitle = "Account Details";
                DashBoardNavigation.AlwaysShowHeader = true;
            }
            else if (args.SelectedItem == PaymentsAndTransfer)
            {
                //PaymentsAndTransfer paymentsAndTransfer = new PaymentsAndTransfer();
                //paymentsAndTransfer.User = CurrentUser;
                //CurrentSelectedModule = paymentsAndTransfer;
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate3"]).LoadContent();

                HeaderTitle = "Payment and Transfer";
                DashBoardNavigation.AlwaysShowHeader = true;
            }
            else if (args.SelectedItem == Settings)
            {
                //SettingsView settings = new SettingsView();
                //settings.User = CurrentUser;
                //CurrentSelectedModule = settings;
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate4"]).LoadContent();
                HeaderTitle = "Settings";
                DashBoardNavigation.AlwaysShowHeader = true;
            }
            else if (args.SelectedItem == FixedDeposit)
            {
                //FDNavigationOverview FDAccount = new FDNavigationOverview();
                //FDAccount.User = CurrentUser;
                //CurrentSelectedModule = FDAccount;
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate5"]).LoadContent();
                HeaderTitle = "Fixed Deposit";
                DashBoardNavigation.AlwaysShowHeader = true;
            }
            else
            {
                //Overview overview = new Overview();
                //overview.User = CurrentUser;
                //CurrentSelectedModule = overview;
                NavigationContentControl.Content = ((DataTemplate)this.Resources["UserControlTemplate1"]).LoadContent();
                HeaderTitle = "Overview";
                DashBoardNavigation.AlwaysShowHeader = false;
            }

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
        private User _currentUser;
        public User CurrentUser
        {
            get { return _currentUser; }
            set
            {
                _currentUser = value;
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
            RaiseCloseWindowNotification?.Invoke();
            RaiseLogoutNotification?.Invoke();
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
