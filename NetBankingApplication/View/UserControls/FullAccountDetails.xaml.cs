using Library.Model;
using Library.Model.Enum;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class FullAccountDetails : Page, IAccountView, ICloseWindow
    {
        private AccountBobj _selectedAccount;
        private string _selectedAccountNumber;
        private string _selectedUserId;
        private string _expense;
        private AppWindow _appWindow;
        // private string test;

        public Account CurrentSelectedAccount;

        private AccountTransactionsBaseViewModel _accountTransactionViewModel;
        private GetBranchDetailsBaseViewModel _getBranchDetailsViewModel;
        private FDAccountDetailsBaseViewModel _fDAccountDetailsViewModel;

        public FullAccountDetails()
        {
            this.InitializeComponent();
            _accountTransactionViewModel = PresenterService.GetInstance().Services.GetService<AccountTransactionsBaseViewModel>();
            _accountTransactionViewModel.AccountView = this;

            _getBranchDetailsViewModel = PresenterService.GetInstance().Services.GetService<GetBranchDetailsBaseViewModel>();
            _fDAccountDetailsViewModel = PresenterService.GetInstance().Services.GetService<FDAccountDetailsBaseViewModel>();
            _fDAccountDetailsViewModel.CloseWindow = this;
            Bindings.Update();

            // if AccountTransactionsViewModel AccountDetails type is fd change template!
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _appWindow = (AppWindow)((object[])e.Parameter)[0];
            _selectedAccount = (AccountBobj)((object[])e.Parameter)[1];
            CurrentSelectedAccount = _selectedAccount.Account;
            _selectedAccountNumber = _selectedAccount.Account.AccountNumber.ToString();
            _selectedUserId = _selectedAccount.UserId;

            _accountTransactionViewModel.GetAllTransactions(_selectedAccountNumber, _selectedUserId);

            _getBranchDetailsViewModel.FetchBranchDetails(_selectedAccount.Account.BId);
            Bindings.Update();

        }


        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double windowHeight = e.NewSize.Height;
            double windowWidth = e.NewSize.Width;

            if (windowHeight < 300 || windowWidth < 850)
            {
                //NarrowLayout 

                Grid.SetRow(BalanceBlock, 1);
                Grid.SetColumn(BalanceBlock, 0);
                Grid.SetColumnSpan(BalanceBlock, 2);
                Grid.SetColumnSpan(AccountDetailsBlock, 2);
                BalanceBlock.HorizontalAlignment = HorizontalAlignment.Left;
            }
            else
            {
                Grid.SetRow(BalanceBlock, 0);
                Grid.SetColumn(BalanceBlock, 1);
                Grid.SetColumnSpan(BalanceBlock, 1);
                Grid.SetColumnSpan(AccountDetailsBlock, 1);
                BalanceBlock.HorizontalAlignment = HorizontalAlignment.Center;


            }
        }

        public void SwichBasedOnAccountType(string AccountNumber)
        {

            if (_accountTransactionViewModel.AccountDetails.AccountType == AccountType.FDAccount)
            {
                _fDAccountDetailsViewModel.GetFDDetails(AccountNumber);
                OverallFDSummary.Visibility = Visibility.Visible;

            }
            else
            {
                OverallSummary.Visibility = Visibility.Visible;
            }
        }

        private void CloseFD_Click(object sender, RoutedEventArgs e)
        {
            _fDAccountDetailsViewModel.CloseFD(_fDAccountDetailsViewModel.CurrentFDAccount, _selectedAccount.UserId);
            //close app window
           
        }

        public void CloseWindow()
        {
            _appWindow.CloseAsync();
        }
    }


}
