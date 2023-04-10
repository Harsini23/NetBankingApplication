using Library.Model;
using Library.Model.Enum;
using Library.Util;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NetBankingApplication.View.UserControls
{
    public sealed partial class FDAccountView : UserControl, INotifyPropertyChanged, ISwitchUserView,INotificationAlert
    {
        IEnumerable<FDType> _FDTypeValues;
        IEnumerable<CustomerType> _CustomerTypeValues;
        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;
        private FDAccountBaseViewModel FDAccountViewModel;
        private string UserAccountNumber;
        private string today;
        private string FromAccount;
        int selectedMonths = 0;
        int selectedYears = 0;
        int selectedDays = 0;
        FDType accountType = FDType.None; CustomerType customerType = CustomerType.None;
        private string Today
        {
            get { return today; }
            set
            {
                today = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public FDAccountView()
        {
            this.InitializeComponent();
            _FDTypeValues = Enum.GetValues(typeof(FDType)).Cast<FDType>();
            _CustomerTypeValues = Enum.GetValues(typeof(CustomerType)).Cast<CustomerType>();
            GetAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            FDAccountViewModel = PresenterService.GetInstance().Services.GetService<FDAccountBaseViewModel>();
            FDAccountViewModel.NotificationAlert = this;
            GetAllAccountsViewModel.TransferAmountView = this;
        }

        private void OpenFD_Click(object sender, RoutedEventArgs e)
        {
            //   AddFDAccount.IsOpen = true;
        }

        MenuFlyout AllAccountTypes;
        private void MenuFlyout_Opening_AccountType(object sender, object e)
        {
            AllAccountTypes = sender as MenuFlyout;
            AllAccountTypes.Items.Clear();
            foreach (var accType in _FDTypeValues)
            {
                if (accType == FDType.None) { continue; }
                var item = new MenuFlyoutItem();
                item.Text = accType.ToString();
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                item.CornerRadius = new CornerRadius(5);
                item.Click += Item_Click; 
                AllAccountTypes.Items.Add(item);
            }
        }
        public void SwitchBasedOnUserAccount()
        {
            if (GetAllAccountsViewModel.AllAccountNumbers.Count > 1)
            {
                MultipleAccounts.Visibility = Visibility.Visible;
            }
            else if (GetAllAccountsViewModel.AllAccountNumbers.Count == 1)
            {
                SingleAccount.Visibility = Visibility.Visible;
                if (GetAllAccountsViewModel.AllAccountNumbers.Count > 0)
                UserAccountNumber = GetAllAccountsViewModel.AllAccountNumbers[0];
                FromAccount = UserAccountNumber;
                // allAccountBalances[0].TotalBalance.ToString();
            }
            else
            {
                //handle ui reload
            }
            Bindings.Update();
        }
        private void TextBox_BalanceOnBeforeTextChanging(TextBox sender,
                                   TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c) && c != '.');

        }


        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            AccountTypeBox.Content = selectedItem.Text;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetAllAccountsViewModel.GetAllAccounts(User.UserId);
            today = CurrentDateTime.GetCurrentDate();
            YearComboBox.Items.Add(new ComboBoxItem { Content="--"});
            MonthComboBox.Items.Add(new ComboBoxItem { Content="--"});
            DayComboBox.Items.Add(new ComboBoxItem { Content="--"});
            for (int i = 1; i <= 10; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                YearComboBox.Items.Add(item);
            }
            for (int i = 1; i <= 11; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                MonthComboBox.Items.Add(item);
            }
            for (int i = 1; i <= 30; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i.ToString();
                DayComboBox.Items.Add(item);
            }
        }
        MenuFlyout selectAccountList;

        public event PropertyChangedEventHandler PropertyChanged;

        private void AccountDropdown_Opening(object sender, object e)
        {
            selectAccountList = sender as MenuFlyout;


            selectAccountList.Items.Clear();
            foreach (var i in GetAllAccountsViewModel.allBalances)
            {
                var item = new MenuFlyoutItem();
                item.Text = i.AccountNumber;
                item.Name = i.TotalBalance.ToString();
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
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
        }

        private void CalculateFD_Click(object sender, RoutedEventArgs e)
        {
            BasicValidation();
            if (AccountTypeBox.Content!=null && CustomerTypeBox.Content != null && !string.IsNullOrEmpty(AmountTextBox.Text) && (selectedMonths != 0 || selectedYears != 0 || selectedDays != 0))
            {
                accountType = (FDType)Enum.Parse(typeof(FDType), AccountTypeBox.Content as string);
                customerType = (CustomerType)Enum.Parse(typeof(CustomerType), CustomerTypeBox.Content as string);
               
                    FDAccountViewModel.OpenAccount = false;
                    FDAccountViewModel.CalculateFD(double.Parse(AmountTextBox.Text), selectedYears, selectedMonths, selectedDays, customerType, accountType,User.UserId);
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Enter all fileds";
            }
        }
        MenuFlyout AllCustomerTypes;
        private void MenuFlyout_Opening_CustomerType(object sender, object e)
        {
            AllCustomerTypes = sender as MenuFlyout;
            AllCustomerTypes.Items.Clear();
            foreach (var cutomerType in _CustomerTypeValues)
            {
                if (cutomerType == CustomerType.None) { continue; }
                var item = new MenuFlyoutItem();
                item.Text = cutomerType.ToString();
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                item.CornerRadius = new CornerRadius(5);
                item.Click += Item_Click1; 
                AllCustomerTypes.Items.Add(item);
            }
        }

        private void Item_Click1(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            CustomerTypeBox.Content = selectedItem.Text;
        }
        private void ClearUI()
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
            SelectAccount.Content = "Select From Account";
            YearComboBox.SelectedIndex = -1; MonthComboBox.SelectedIndex=-1; DayComboBox.SelectedIndex = -1;
            CustomerTypeBox.Content = string.Empty;
            AccountTypeBox.Content = string.Empty;
            AmountTextBox.Text = "";
            ErrorMessage.Text = String.Empty;
            BalanceText.Text = "Choose Account";
            GetAllAccountsViewModel.CurrentAccountBalance = "Choose Account";

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }
        private void BasicValidation()
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
         
            ComboBoxItem MonthselectedItem = MonthComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem YearselectedItem = YearComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem DayselectedItem = DayComboBox.SelectedItem as ComboBoxItem;
            if (MonthselectedItem != null )
            {
                selectedMonths = MonthComboBox.SelectedIndex==0? 0: int.Parse(MonthselectedItem.Content as string);
            }
            if (YearselectedItem != null )
            {
                selectedYears = YearComboBox.SelectedIndex==0? 0:int.Parse(YearselectedItem.Content as string);
            }
            if (DayselectedItem != null )
            {
                selectedDays = DayComboBox.SelectedIndex==0?0:int.Parse(DayselectedItem.Content as string);
            }
        }

        private void OpenFDAccount_Click(object sender, RoutedEventArgs e)
        {
            BasicValidation();
            if ((string)SelectAccount.Content == (string)"Select From Account" && MultipleAccounts.Visibility != Visibility.Collapsed)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Select your account";
            }
            else
            {
                if (AccountTypeBox.Content != null && CustomerTypeBox.Content != null && !string.IsNullOrEmpty(AmountTextBox.Text) && (selectedMonths != 0 || selectedYears != 0 || selectedDays != 0) && SelectAccount.Content != "Select From Account")
                {
                    accountType = (FDType)Enum.Parse(typeof(FDType), AccountTypeBox.Content as string);
                    customerType = (CustomerType)Enum.Parse(typeof(CustomerType), CustomerTypeBox.Content as string);

                    FDAccountViewModel.OpenAccount = true;
                    FDAccountViewModel.CalculateFD(double.Parse(AmountTextBox.Text), selectedYears, selectedMonths, selectedDays, customerType, accountType, User.UserId, FromAccount);
                    ClearUI();
                }
                else
                {
                    ErrorMessage.Visibility = Visibility.Visible;
                    ErrorMessage.Text = "Enter all fileds";
                }
                
            }

        }

        public void CallNotification()
        {
            InAppNotification.Show(3000);
        }
    }
}
