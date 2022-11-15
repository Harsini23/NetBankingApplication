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
        private DashboardNavigationBaseViewModel _dashboardNavigationViewModel;
        PresenterService DashboardNavigationVMserviceProviderInstance;
        public DashBoard()
        {
            this.InitializeComponent();
            DashboardNavigationVMserviceProviderInstance = PresenterService.GetInstance();
            _dashboardNavigationViewModel = DashboardNavigationVMserviceProviderInstance.Services.GetService<DashboardNavigationBaseViewModel>();
        }
        User Currentuser;

      
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Currentuser = (User)e.Parameter;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Overview overview = new Overview();
            overview.CurrentUser = Currentuser;
            CurrentSelectedModule = overview;
            //_dashboardNavigationViewModel.SetOverview();
            // SelectedDashboardContentFrame.Navigate(typeof(Overview), Currentuser);
        }


        private void DashBoardNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            FrameNavigationOptions navOptions = new FrameNavigationOptions();
            navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            {
                navOptions.IsNavigationStackEnabled = false;
            }

            Type pageType;

            if (args.SelectedItem == Overview)
            {
                Overview overview = new Overview();
                overview.CurrentUser = Currentuser;
                CurrentSelectedModule = overview;

                //_dashboardNavigationViewModel.SetOverview();
                //  pageType = typeof(Overview);
            }
            else if (args.SelectedItem == BankAccount)
            {
                //_dashboardNavigationViewModel.SetBankAccount();
                BankAccount bankAccount = new BankAccount();
                bankAccount.CurrentUser = Currentuser;
                CurrentSelectedModule = bankAccount;
                //  pageType = typeof(BankAccount);
            }
            else if (args.SelectedItem == PaymentsAndTransfer)
            {
               // pageType = typeof(PaymentsAndTransfer);
            }
            else if (args.SelectedItem == CardAndLoans)
            {
              //  pageType = typeof(CardAndLoans);
            }
            else if (args.SelectedItem == InvestmentAndInsurance)
            {
               // pageType = typeof(InvestmentAndInsurance);
            }
            else if (args.SelectedItem == CustomerService)
            {
               // pageType = typeof(CustomerService);
            }
            else
            {
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
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
