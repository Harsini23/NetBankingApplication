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
    public sealed partial class TransferAmount : UserControl, ISwitchUserView, ZeroBalance, ISuggestAndAddPayeeView
    {
        private string Name;
        private string FromAccount;
        private string ToAccount;
        private string RemarkDescription="-";
        private double Amount;
        private string NewPayeeEnteredName;
        private string UserAccountNumber;
        private string _amount;
        private bool IsNewPayee;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

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

        public event Action<String, String> SendPayee;

        public TransferAmount()
        {
            this.InitializeComponent();
        
            GetAllPayeeViewModel = PresenterService.GetInstance().Services.GetService<GetAllPayeeBaseViewModel>();
            TransferAmountViewModel = PresenterService.GetInstance().Services.GetService<TransferAmountBaseViewModel>();
            GetAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            GetAllAccountsViewModel.ZerobalanceView = this;
            GetAllAccountsViewModel.TransferAmountView = this;
            MakeTransaction.IsEnabled = true;

        }
        MenuFlyout selectPayeeList;
        MenuFlyout selectAccountList;

        private async void MakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            
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
                    UserId = User.UserId,
                    Name = Name,
                    FromAccount = FromAccount,
                    ToAccount = ToAccount,
                    Remark = RemarkDescription,
                    Amount = Amount
                };
                if (amountTransfer.ToAccount != null && amountTransfer.Amount != null && amountTransfer.Name != null)
                {
                    TransferAmountViewModel.suggestionPopUp = this;
                    TransferAmountViewModel.SendTransaction(amountTransfer, amountTransfer.UserId);
                    //set instance in VM to call usercontrol to conver it into added payee

                    TransferAmountViewModel.NewPayee = IsNewPayee;
                }

                ResetUI();
                await ContentDialog.ShowAsync();
                GetAllAccountsViewModel.GetAllAccounts(User.UserId);

                // TransferAmountViewModel.SendTransaction(currentTransaction);
            }

        }
        private void TextBox_BalanceOnBeforeTextChanging(TextBox sender,
                                      TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c) && c != '.');

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

            GetAllPayeeViewModel.GetAllPayee(User.UserId);
            GetAllAccountsViewModel.GetAllAccounts(User.UserId);
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
            IsNewPayee = false;

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
            IsNewPayee=true;

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

        public void addPayeeView()
        {
            //shows popup to add the payee or close
            //AddPayeePopup.IsOpen = true;
           
            //if add payee- redirect to addpayee with name and account value filled out
        }

        private void AddPayeePopup_Closed(object sender, object e)
        {

        }

        private void YesProceed_Click(object sender, RoutedEventArgs e)
        {
            //redirect to addpayee with params - payee name and acc no.
            SendPayee?.Invoke(Name, ToAccount);
        }

        private void NoLater_Click(object sender, RoutedEventArgs e)
        {
            AddPayeePopup.IsOpen = false;
        }

        private void ContentDialog_Closed(ContentDialog sender, ContentDialogClosedEventArgs args)
        {
            if (IsNewPayee)
            {
                IsNewPayee = false;
                AddPayeePopup.IsOpen = true;
                double horizontalOffset = Window.Current.Bounds.Width / 2 - AddPayeePopup.ActualWidth / 2 + 200;
                double verticalOffset = Window.Current.Bounds.Height / 2 - AddPayeePopup.ActualHeight / 2+100;
                AddPayeePopup.HorizontalOffset = horizontalOffset;
                AddPayeePopup.VerticalOffset = verticalOffset;
            }
        
        }
    }
}
