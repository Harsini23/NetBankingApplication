using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace NetBankingApplication.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DetailedAccountOverview : Page, ISwitchUserView
    {

        public AccountTransactionBObj CurrentSelectedTransaction;

        private AccountTransactionsBaseViewModel AccountTransactionsViewModel;

        PresenterService AccountTransactionsVMserviceProviderInstance;
        private string currentUserId;
        private string currentAccount;
        private string CurrentUserAccountNumber;
        private User currentUser;
        private List<String> AllAccounts;

        //public event PropertyChangedEventHandler PropertyChanged;
        PresenterService TransferAmountVMserviceProviderInstance;

        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;

        PresenterService GetAllAccountsVMserviceProviderInstance;
        public DetailedAccountOverview(User user)
        {
            this.InitializeComponent();

            AccountTransactionsVMserviceProviderInstance = PresenterService.GetInstance();
            AccountTransactionsViewModel = AccountTransactionsVMserviceProviderInstance.Services.GetService<AccountTransactionsBaseViewModel>();


            GetAllAccountsVMserviceProviderInstance = PresenterService.GetInstance();
            GetAllAccountsViewModel = GetAllAccountsVMserviceProviderInstance.Services.GetService<GetAllAccountsBaseViewModel>();
            GetAllAccountsViewModel.TransferAmountView = this;


            currentUserId = user.UserId;

         
            //if (user.HasSingleAccount)
            //{
            //    SingleAccountnumberTextblock.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    SelectAccountDropdown.Visibility = Visibility.Visible;
            //}
            //currentAccount = accountNo;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //take current accountnumber and userid
            // GetAllAccountsViewModel.GetAllAccounts(_currentUserId);
            GetAllAccountsViewModel.GetAllAccounts(currentUserId);
            AllAccounts = GetAllAccountsViewModel.AllAccountNumbers;
            CurrentUserAccountNumber = GetAllAccountsViewModel.AllAccountNumbers[0];
            AccountTransactionsViewModel.GetAllTransactions(CurrentUserAccountNumber, currentUserId);
            SelectAccountDropdown.Content = GetAllAccountsViewModel.AllAccountNumbers[0];
        }

        private void AllTransactionsOnAccountListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            CurrentSelectedTransaction = e.ClickedItem as AccountTransactionBObj;
            //DetailedTransactionView detailedTransactionView = new DetailedTransactionView();
            //detailedTransactionView.SelectedTransaction = CurrentSelectedTransaction;
        }
        MenuFlyout selectAccountList;
        private void MenuFlyout_Opening(object sender, object e)
        {
           
            selectAccountList = sender as MenuFlyout;
            selectAccountList.Items.Clear();
            foreach (var i in GetAllAccountsViewModel.AllAccountNumbers)
            {
                if (i != CurrentUserAccountNumber)
                {
                    var item = new MenuFlyoutItem();
                    item.Text = i;
                    item.Click += Account_Selection; ;
                    item.MinWidth = 150;
                    selectAccountList.Items.Add(item);
                }
               
            }

        }

        private void Account_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            CurrentUserAccountNumber = selectedItem.Text;
            SelectAccountDropdown.Content = selectedItem.Text;
            AccountTransactionsViewModel.GetAllTransactions(CurrentUserAccountNumber, currentUserId);

        }

        public void SwitchBasedOnUserAccount()
        {
            if (AllAccounts.Count==1)
            {
                SingleAccountnumberTextblock.Visibility = Visibility.Visible;
            }
            else
            {
                SelectAccountDropdown.Visibility = Visibility.Visible;
            }
            Bindings.Update();

        }
    }
}
