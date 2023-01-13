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

        public DashBoard()
        {
            this.InitializeComponent();

            LoginVMserviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = LoginVMserviceProviderInstance.Services.GetService<LoginBaseViewModel>();

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

                //_dashboardNavigationViewModel.SetOverview();
                //  pageType = typeof(Overview);
            }
            else if (args.SelectedItem == BankAccount)
            {
                //_dashboardNavigationViewModel.SetBankAccount();

                BankAccount bankAccount = new BankAccount(Currentuser);

                CurrentSelectedModule = bankAccount;
                HeaderTitle = "Bank Account";
                //  pageType = typeof(BankAccount);
            }
            else if (args.SelectedItem == PaymentsAndTransfer)
            {
                PaymentsAndTransfer paymentsAndTransfer = new PaymentsAndTransfer(Currentuser);
                CurrentSelectedModule = paymentsAndTransfer;
                // pageType = typeof(PaymentsAndTransfer);
                HeaderTitle = "Payment and Transfer";
            }
            else if (args.SelectedItem == CardAndLoans)
            {
                HeaderTitle = "Card and Loans";
              //  pageType = typeof(CardAndLoans);
            }
            else
            {
                HeaderTitle = "Overview";
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
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Logout_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //logout logic
            LoginViewModel.Logout();
        }
    }
}
