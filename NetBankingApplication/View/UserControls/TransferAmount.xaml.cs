using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NetBankingApplication.View.UserControls
{
    public sealed partial class TransferAmount : UserControl
    {
        private string UserId;
        private string Name;
        private string FromAccount;
        private string ToAccount;
        private string RemarkDescription;
        private string Amount;
        private string NewPayeeEnteredName;


        private GetAllPayeeBaseViewModel GetAllPayeeViewModel;

        PresenterService GetAllPayeeVMserviceProviderInstance;

        private TransferAmountBaseViewModel TransferAmountViewModel;

        PresenterService TransferAmountVMserviceProviderInstance;

        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;

        PresenterService GetAllAccountsVMserviceProviderInstance;


        List<String> allRecipientNames = new List<String>();
        List<Payee> allRecipients = new List<Payee>();

        List<Account> allAccounts = new List<Account>();
        List<String> allAccountNumbers = new List<String>();

        public TransferAmount()
        {
            this.InitializeComponent();

            GetAllPayeeVMserviceProviderInstance = PresenterService.GetInstance();
            GetAllPayeeViewModel = GetAllPayeeVMserviceProviderInstance.Services.GetService<GetAllPayeeBaseViewModel>();

            TransferAmountVMserviceProviderInstance = PresenterService.GetInstance();
            TransferAmountViewModel = TransferAmountVMserviceProviderInstance.Services.GetService<TransferAmountBaseViewModel>();

            GetAllAccountsVMserviceProviderInstance = PresenterService.GetInstance();
            GetAllAccountsViewModel = GetAllAccountsVMserviceProviderInstance.Services.GetService<GetAllAccountsBaseViewModel>();
          

        }
        MenuFlyout selectPayeeList;
        MenuFlyout selectAccountList;

        private async void MakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            //Grid.SetColumnSpan(TransferAmountDetails,0);
            //Grid.SetRowSpan(TransferAmountDetails,0);


            //show the current transaction overview receipt
            //TransactionDetails.Visibility = Visibility.Visible;
           

            //get transaction fields
            if (NewPayeeName.Text == null && NewPayeeName.Text == String.Empty && NewPayeeName.Text == "")
            {
                Name = NewPayeeName.Text;
            }

            Amount=AmountTextBox.Text;
            RemarkDescription=RemarkTextBox.Text;
            //get from account from using user id from datamanager
            AmountTransfer amountTranfer = new AmountTransfer
            {
                UserId = "Harsh",
                Name = Name,
                FromAccount = FromAccount,
                ToAccount = ToAccount,
                Remark = RemarkDescription,
                Amount = Amount
            };
            if (amountTranfer.ToAccount!=null && amountTranfer.Amount!=null && amountTranfer.Name!=null)
            {
                TransferAmountViewModel.SendTransaction(amountTranfer,"Harsh");
            }
            ResetUI();

            await ContentDialog.ShowAsync();
            // TransferAmountViewModel.SendTransaction(currentTransaction);
        }

        private void ResetUI()
        {
            SelectPayee.Content = "Select payee";
            NewPayeeName.Visibility = Visibility.Collapsed;
            AccountNumberTextBox.Text = ""; SelectAccount.Content = "Select From Account";
            RemarkTextBox.Text = "";AmountTextBox.Text = "";
            TransactionResult.Text = "";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //load list of payee with all details as payee object
            GetAllPayeeViewModel.GetAllPayee("Harsh");
            allRecipientNames.Clear();
           allRecipientNames= GetAllPayeeViewModel.PayeeNames;
            allRecipients.Clear();
            allRecipients = GetAllPayeeViewModel.AllPayee;

            GetAllAccountsViewModel.GetAllAccounts("Harsh");
            allAccountNumbers.Clear();
            allAccountNumbers = GetAllAccountsViewModel.AllAccountNumbers;
            allAccounts.Clear();
            allAccounts = GetAllAccountsViewModel.AllAccounts;
            TransactionResult.Text = String.Empty;

        }

   

        private void MenuFlyout_Opened(object sender, object e)
        {

            selectPayeeList = sender as MenuFlyout;
            selectPayeeList.Items.Clear();
            var defaultItem = new MenuFlyoutItem();
            defaultItem.Text = "New Payee";
            defaultItem.Click += QuickTransaction_Click;
            selectPayeeList.Items.Add(defaultItem);
            foreach (var i in allRecipientNames)
            {
                var item = new MenuFlyoutItem();
                item.Text = i;
                item.Click += Item_Click;
                item.MinWidth = 150;
                selectPayeeList.Items.Add(item);
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            NewPayeeName.Visibility = Visibility.Collapsed;
            SelectPayee.Content = selectedItem.Text;
            ToAccount=selectedItem.Text;
            foreach (var i in allRecipients)
            {
                if (selectedItem.Text == i.PayeeName)
                {
                    AccountNumberTextBox.Text = i.AccountNumber;
                    AccountNumberTextBox.IsReadOnly = true;
                    Name = i.PayeeName;
                    ToAccount = i.AccountNumber;
                    Name = i.AccountHolderName;
                    break;
                }
            }
        
        }

        private void QuickTransaction_Click(object sender, RoutedEventArgs e)
        {
            AccountNumberTextBox.Text = String.Empty;
            AccountNumberTextBox.IsEnabled = true;
            AccountNumberTextBox.IsReadOnly=false;
            var selectedItem = sender as MenuFlyoutItem;
            SelectPayee.Content = selectedItem.Text;
            ToAccount = selectedItem.Text;
            NewPayeeName.Visibility = Visibility.Visible;
        }

        private void AccountDropdown_Opening(object sender, object e)
        {
            selectAccountList = sender as MenuFlyout;
            selectAccountList.Items.Clear();
            foreach (var i in allAccountNumbers)
            {
                var item = new MenuFlyoutItem();
                item.Text = i;
                item.Click += Account_Selection; ;
                item.MinWidth = 150;
                selectAccountList.Items.Add(item);
            }
        }

        private void Account_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            FromAccount = selectedItem.Text;
            SelectAccount.Content= selectedItem.Text;
            Debug.WriteLine(ToAccount);
        }

        private void NewPayeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var name = (TextBox)sender;
            NewPayeeEnteredName = name.Text.ToString();
           
        }
    }
}
