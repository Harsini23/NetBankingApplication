using Library.Model;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DashBoard : Page
    {
        public DashBoard()
        {
            this.InitializeComponent();
        }
        User Currentuser;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Currentuser = (User)e.Parameter;
            

        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectedDashboardContentFrame.Navigate(typeof(Overview), Currentuser);
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
                pageType = typeof(Overview);
            }
            else if (args.SelectedItem == BankAccount)
            {
                pageType = typeof(BankAccount);
            }
            else if (args.SelectedItem == PaymentsAndTransfer)
            {
                pageType = typeof(PaymentsAndTransfer);
            }
            else if (args.SelectedItem == CardAndLoans)
            {
                pageType = typeof(CardAndLoans);
            }
            else if (args.SelectedItem == InvestmentAndInsurance)
            {
                pageType = typeof(InvestmentAndInsurance);
            }
            else if (args.SelectedItem == CustomerService)
            {
                pageType = typeof(CustomerService);
            }
            else
            {
                pageType = typeof(Overview);
            }

            SelectedDashboardContentFrame.NavigateToType(pageType, null, navOptions);

        }
    }
}
