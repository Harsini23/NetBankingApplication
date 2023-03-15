using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
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
    public sealed partial class TransferAmount : UserControl, ISwitchUserView, ZeroBalance
    {
        private string _currentUserId;

        // private string UserId;
        private string Name;
        private string FromAccount;
        private string ToAccount;
        private string RemarkDescription="-";
        private double Amount;
        private string NewPayeeEnteredName;
        private string UserAccountNumber;
        private string UserAccountBalance;
        private string _amount;


        private GetAllPayeeBaseViewModel GetAllPayeeViewModel;
        private TransferAmountBaseViewModel TransferAmountViewModel;
        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;

        public event Action<string> RaiseNotification;

        List<String> allRecipientNames = new List<String>();
        List<Payee> allRecipients = new List<Payee>();

        List<Account> allAccounts = new List<Account>();
        ObservableCollection<String> allAccountNumbers = new ObservableCollection<String>();

       // List<AccountBalance> allBalances = new List<AccountBalance>();
        ObservableCollection<AccountBalance> allAccountBalances = new ObservableCollection<AccountBalance>();

        public TransferAmount(string userId)
        {
            this.InitializeComponent();
        

            _currentUserId = userId;

            GetAllPayeeViewModel = PresenterService.GetInstance().Services.GetService<GetAllPayeeBaseViewModel>();
            TransferAmountViewModel = PresenterService.GetInstance().Services.GetService<TransferAmountBaseViewModel>();
            GetAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();

            GetAllAccountsViewModel.ZerobalanceView = this;
            GetAllAccountsViewModel.TransferAmountView = this;

            GetAllPayeeViewModel.GetAllPayee(_currentUserId);
            GetAllAccountsViewModel.GetAllAccounts(_currentUserId);
            MakeTransaction.IsEnabled = true;

        }
        MenuFlyout selectPayeeList;
        MenuFlyout selectAccountList;

        private async void MakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            //Grid.SetColumnSpan(TransferAmountDetails,0);
            //Grid.SetRowSpan(TransferAmountDetails,0);

            //show the current transaction overview receipt
            //TransactionDetails.Visibility = Visibility.Visible;
            //if (String.IsNullOrEmpty(RemarkTextBox.Text))
            //{
            //    ErrorMessage.Text = "Fill out remark field";
            //}
            //else
            if (String.IsNullOrEmpty(AccountNumberTextBox.Text))
            {
                ErrorMessage.Text = "Fill out account number field";
            }
            else if (String.IsNullOrEmpty(AmountTextBox.Text))
            {
                ErrorMessage.Text = "Fill out amount field";
            }
            else if (!IsFloatOrInt(_amount) || Double.Parse(_amount) <= 0)
            {
                ErrorMessage.Text = "Enter valid amount";
            }
            else if ((string)SelectAccount.Content == (string)"Select From Account" && MultipleAccounts.Visibility!=Visibility.Collapsed)
            {
                ErrorMessage.Text = "Select your account";
            }
            else
            {
                ErrorMessage.Text = String.Empty;

                //get transaction fields
                if (!String.IsNullOrEmpty(NewPayeeName.Text))
                {
                    Name = NewPayeeName.Text;
                    ToAccount = AccountNumberTextBox.Text;
                }

                Amount = Double.Parse(AmountTextBox.Text);
                if (!string.IsNullOrWhiteSpace(RemarkTextBox.Text))
                {
                    RemarkDescription = RemarkTextBox.Text;
                }
                //get from account from using user id from datamanager
                AmountTransfer amountTransfer = new AmountTransfer
                {
                    UserId = _currentUserId,
                    Name = Name,
                    FromAccount = FromAccount,
                    ToAccount = ToAccount,
                    Remark = RemarkDescription,
                    Amount = Amount
                };
                if (amountTransfer.ToAccount != null && amountTransfer.Amount != null && amountTransfer.Name != null)
                {
                    TransferAmountViewModel.SendTransaction(amountTransfer, amountTransfer.UserId);
                }

                ResetUI();
                await ContentDialog.ShowAsync();
                GetAllAccountsViewModel.GetAllAccounts(_currentUserId);

                // TransferAmountViewModel.SendTransaction(currentTransaction);
            }

        }

        private void ResetUI()
        {
            SelectPayee.Content = "Select payee";
            NewPayeeName.Visibility = Visibility.Collapsed;
            AccountNumberTextBox.Text = ""; SelectAccount.Content = "Select From Account";
            RemarkTextBox.Text = ""; AmountTextBox.Text = "";
            NewPayeeName.Text = "";
            // TransactionResult.Text = String.Empty;
            //MakeTransaction.IsEnabled = false;
            ErrorMessage.Text = String.Empty;
            BalanceText.Text = "Choose Account";
            GetAllAccountsViewModel.CurrentAccountBalance = "Choose Account";

        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //load list of payee with all details as payee object
            
            allRecipientNames.Clear();
            allRecipientNames = GetAllPayeeViewModel.PayeeNames;
            allRecipients.Clear();
            allRecipients = GetAllPayeeViewModel.AllPayee;
            ErrorMessage.Text = String.Empty;


            allAccountNumbers.Clear();
            allAccountNumbers = GetAllAccountsViewModel.AllAccountNumbers;
            allAccounts.Clear();
            allAccounts = GetAllAccountsViewModel.AllAccounts;
            allAccountBalances.Clear();
            allAccountBalances = GetAllAccountsViewModel.allBalances;
            SwitchBasedOnUserAccount();


            AccountNumberTextBox.IsEnabled = false;
            AccountNumberTextBox.IsReadOnly = true;

            
            //allAccountNumbers.Clear();
            //allAccountNumbers = GetAllAccountsViewModel.AllAccountNumbers;
            //allAccounts.Clear();
            //allAccounts = GetAllAccountsViewModel.AllAccounts;
            //MultipleAccounts.Visibility = Visibility.Visible;
            //if (allAccountNumbers.Count > 1)
            //{
               // MultipleAccounts.Visibility = Visibility.Visible;
            //}
            //else
            //{
            //    SingleAccount.Visibility = Visibility.Visible;
            //    UserAccountNumber = allAccountNumbers[0];
            //}
            // TransactionResult.Text = String.Empty;

        }





        private void MenuFlyout_Opened(object sender, object e)
        {

            selectPayeeList = sender as MenuFlyout;
            selectPayeeList.Items.Clear();
            var defaultItem = new MenuFlyoutItem();
            defaultItem.Text = "New Payee";
            defaultItem.Click += QuickTransaction_Click;
            defaultItem.CornerRadius = new CornerRadius(5);
            selectPayeeList.Items.Add(defaultItem);
            foreach (var i in allRecipientNames)
            {
                var item = new MenuFlyoutItem();
                item.Text = i;
                item.Click += Item_Click;
                item.CornerRadius = new CornerRadius(5);
                item.MinWidth = 150;
                selectPayeeList.Items.Add(item);
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            NewPayeeName.Visibility = Visibility.Collapsed;
            SelectPayee.Content = selectedItem.Text;
            ToAccount = selectedItem.Text;
            foreach (var i in allRecipients)
            {
                if (selectedItem.Text == i.PayeeName)
                {
                    AccountNumberTextBox.Text = i.AccountNumber;
                    AccountNumberTextBox.IsReadOnly = true;
                    AccountNumberTextBox.IsEnabled = true;
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
            AccountNumberTextBox.IsReadOnly = false;
            var selectedItem = sender as MenuFlyoutItem;
            SelectPayee.Content = selectedItem.Text;
            ToAccount = selectedItem.Text;
            NewPayeeName.Visibility = Visibility.Visible;
        }

        private void AccountDropdown_Opening(object sender, object e)
        {
            selectAccountList = sender as MenuFlyout;
           // AccountBalance.Children.Clear();

            //foreach (var i in GetAllAccountsViewModel.allBalances)
            //{
            //    //var item = new MenuFlyoutItem();

            //    var overallStackPanel = new StackPanel();
            //    overallStackPanel.Name = i.AccountNumber;
            //    overallStackPanel.Margin = new Thickness(5);
            //    overallStackPanel.Tapped += Account_Selection;
            //    overallStackPanel.Children.Add(new TextBlock { Text=i.AccountNumber, Name=i.AccountNumber});
            //    var balanceSp = new StackPanel();
            //    balanceSp.Orientation = Orientation.Horizontal;
            //    balanceSp.Children.Add(new TextBlock { Text = "₹" , Padding=new Thickness(0,0,5,0)});
            //    balanceSp.Children.Add(new TextBlock { Text = i.TotalBalance.ToString() });
            //    overallStackPanel.Children.Add(balanceSp);
            //    AccountBalance.Children.Add(overallStackPanel);

            //}


            selectAccountList.Items.Clear();
            foreach (var i in GetAllAccountsViewModel.allBalances)
            {
                var item = new MenuFlyoutItem();
                item.Text = i.AccountNumber;
                item.Name = i.TotalBalance.ToString();
                item.CornerRadius = new CornerRadius(5);
                item.Click += Account_Selection; ;
                item.MinWidth = 150;
                selectAccountList.Items.Add(item);
            }
        }

        private void Account_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            FromAccount = selectedItem.Text;
            SelectAccount.Content = selectedItem.Text;
            GetAllAccountsViewModel.CurrentAccountBalance = selectedItem.Name;
            Debug.WriteLine(ToAccount);

        }

        private void NewPayeeName_TextChanged(object sender, TextChangedEventArgs e)
        {
            var name = (TextBox)sender;
            NewPayeeEnteredName = name.Text.ToString();

        }

        private void AmountTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var amountBox = (TextBox)sender;
            ErrorMessage.Text = String.Empty;
             _amount = amountBox.Text.ToString();
            double val = 0.0;
            if(amountBox.Text.Length > 0 )
            {
                var parsable = double.TryParse(AmountTextBox.Text,out val);
              
                if (Math.Abs(val % 1) >= 0.01)
                {
                    AmountTextBox.Text = Math.Round(val, 2).ToString();
                    AmountTextBox.SelectionStart = AmountTextBox.Text.Length;
                }
            }

         
            if (_amount == String.Empty)
            {
                ErrorMessage.Text = String.Empty;
            }

           else if (!IsFloatOrInt(_amount) || Double.Parse(_amount) <= 0 && !String.IsNullOrEmpty(_amount) )
            {
                ErrorMessage.Text = "Kindly check and enter valid amount";
                //MakeTransaction.IsEnabled = false;
            }
            else
            {
                ErrorMessage.Text = String.Empty;
            }
        }

        private static bool IsFloatOrInt(string value)
        {
            int intValue;
            float floatValue;
            return Int32.TryParse(value, out intValue) || float.TryParse(value, out floatValue);
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender,
                                         TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        //call from VM incase the user has more than one account
        public void SwitchBasedOnUserAccount()
        {
            if (allAccountNumbers.Count > 1)
            {
                MultipleAccounts.Visibility = Visibility.Visible;
            }
            else if(allAccountNumbers.Count==1)
            {
                SingleAccount.Visibility = Visibility.Visible;
                if(allAccountNumbers.Count > 0)
                UserAccountNumber = allAccountNumbers[0];
                FromAccount = UserAccountNumber;
             // allAccountBalances[0].TotalBalance.ToString();
            }
            else
            {
                //handle ui reload
            }
            Bindings.Update();
        }

        public void ZeroBalanceNotification()
        {
           // int duration = 4000;
            RaiseNotification?.Invoke("Zero balance alert");

            //InAppNotification.Show("Zero Balance Alert!!", duration);
            //disable make payment button
            MakeTransaction.IsEnabled = false;

        }
    }
}
