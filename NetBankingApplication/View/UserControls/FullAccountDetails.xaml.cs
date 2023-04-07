﻿using Library.Model;
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

    public sealed partial class FullAccountDetails : Page, IAccountView
    {
        AccountBobj selectedAccount;
        private string selectedAccountNumber;
        private string selectedUserId;
        private string expense;
        // private string test;

        public Account CurrentSelectedAccount;

        private AccountTransactionsBaseViewModel AccountTransactionViewModel;

        private GetBranchDetailsBaseViewModel GetBranchDetailsViewModel;
        private FDAccountDetailsBaseViewModel FDAccountDetailsViewModel;

        public FullAccountDetails()
        {
            this.InitializeComponent();
            AccountTransactionViewModel = PresenterService.GetInstance().Services.GetService<AccountTransactionsBaseViewModel>();
            AccountTransactionViewModel.AccountView = this;

            GetBranchDetailsViewModel = PresenterService.GetInstance().Services.GetService<GetBranchDetailsBaseViewModel>();
            FDAccountDetailsViewModel = PresenterService.GetInstance().Services.GetService<FDAccountDetailsBaseViewModel>();
            Bindings.Update();

            // if AccountTransactionsViewModel AccountDetails type is fd change template!
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            selectedAccount = e.Parameter as AccountBobj;
            CurrentSelectedAccount = selectedAccount.Account;
            selectedAccountNumber = selectedAccount.Account.AccountNumber.ToString();
            selectedUserId = selectedAccount.UserId;

            AccountTransactionViewModel.GetAllTransactions(selectedAccountNumber, selectedUserId);

            GetBranchDetailsViewModel.FetchBranchDetails(selectedAccount.Account.BId);
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

            if (AccountTransactionViewModel.AccountDetails.AccountType == AccountType.FDAccount)
            {
                FDAccountDetailsViewModel.GetFDDetails(AccountNumber);
                OverallFDSummary.Visibility = Visibility.Visible;

            }
            else
            {
                OverallSummary.Visibility = Visibility.Visible;
            }

        }
    }


}
