﻿using Library.Model;
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
        private IEnumerable<FDType> _fDTypeValues;
        private IEnumerable<CustomerType> _customerTypeValues;
        private GetAllAccountsBaseViewModel _getAllAccountsViewModel;
        private FDAccountBaseViewModel _fDAccountViewModel;
        private string _userAccountNumber;
        private string _today;
        private string _fromAccount;
        private int _selectedMonths = 0;
        private int _selectedYears = 0;
        private int _selectedDays = 0;
        private FDType _accountType = FDType.None; 
        private CustomerType _customerType = CustomerType.None;
        private string Today
        {
            get { return _today; }
            set
            {
                _today = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(FDAccountView), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public FDAccountView()
        {
            this.InitializeComponent();
            _fDTypeValues = Enum.GetValues(typeof(FDType)).Cast<FDType>();
            _customerTypeValues = Enum.GetValues(typeof(CustomerType)).Cast<CustomerType>();
            _getAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            _fDAccountViewModel = PresenterService.GetInstance().Services.GetService<FDAccountBaseViewModel>();
            _fDAccountViewModel.NotificationAlert = this;
            _getAllAccountsViewModel.TransferAmountView = this;
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
            foreach (var accType in _fDTypeValues)
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
            if (_getAllAccountsViewModel.AllAccountNumbers.Count > 1)
            {
                MultipleAccounts.Visibility = Visibility.Visible;
            }
            else if (_getAllAccountsViewModel.AllAccountNumbers.Count == 1)
            {
                SingleAccount.Visibility = Visibility.Visible;
                if (_getAllAccountsViewModel.AllAccountNumbers.Count > 0)
                _userAccountNumber = _getAllAccountsViewModel.AllAccountNumbers[0];
                _fromAccount = _userAccountNumber;
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
            CallCalculate();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            _getAllAccountsViewModel.GetAllAccounts(User.UserId, true);
            _today = CurrentDateTime.GetCurrentDate();
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
            foreach (var i in _getAllAccountsViewModel.allBalances)
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
            _fromAccount = selectedItem.Text;
            SelectAccount.Content = selectedItem.Text;
            _getAllAccountsViewModel.CurrentAccountBalance = selectedItem.Name;
            CallCalculate();
        }

        private void CalculateFD_Click(object sender, RoutedEventArgs e)
        {
           
            BasicValidation();
            if (AccountTypeBox.Content!=null && CustomerTypeBox.Content != null && !string.IsNullOrEmpty(AmountTextBox.Text) && (_selectedMonths != 0 || _selectedYears != 0 || _selectedDays != 0))
            {
                CalculationGrid.Visibility = Visibility.Visible;
                _accountType = (FDType)Enum.Parse(typeof(FDType), AccountTypeBox.Content as string);
                _customerType = (CustomerType)Enum.Parse(typeof(CustomerType), CustomerTypeBox.Content as string);
               
                    _fDAccountViewModel.OpenAccount = false;
                    _fDAccountViewModel.CalculateFD(double.Parse(AmountTextBox.Text), _selectedYears, _selectedMonths, _selectedDays, _customerType, _accountType,User.UserId);
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
            foreach (var cutomerType in _customerTypeValues)
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
            CallCalculate();
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
            _getAllAccountsViewModel.CurrentAccountBalance = "Choose Account";

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
                _selectedMonths = MonthComboBox.SelectedIndex==0? 0: int.Parse(MonthselectedItem.Content as string);
            }
            if (YearselectedItem != null )
            {
                _selectedYears = YearComboBox.SelectedIndex==0? 0:int.Parse(YearselectedItem.Content as string);
            }
            if (DayselectedItem != null )
            {
                _selectedDays = DayComboBox.SelectedIndex==0?0:int.Parse(DayselectedItem.Content as string);
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
                if (AccountTypeBox.Content != null && CustomerTypeBox.Content != null && !string.IsNullOrEmpty(AmountTextBox.Text) && (_selectedMonths != 0 || _selectedYears != 0 || _selectedDays != 0))
                {
                    _accountType = (FDType)Enum.Parse(typeof(FDType), AccountTypeBox.Content as string);
                    _customerType = (CustomerType)Enum.Parse(typeof(CustomerType), CustomerTypeBox.Content as string);

                    _fDAccountViewModel.OpenAccount = true;
                    _fDAccountViewModel.CalculateFD(double.Parse(AmountTextBox.Text), _selectedYears, _selectedMonths, _selectedDays, _customerType, _accountType, User.UserId, _fromAccount);
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

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowHeight = e.NewSize.Height;
            var windowWidth = e.NewSize.Width;
            //Grid.SetRow(BalanceBlock, 1);
            //Grid.SetColumn(BalanceBlock, 0);
            //Grid.SetColumnSpan(BalanceBlock, 2);
            //Grid.SetColumnSpan(AccountDetailsBlock, 2);

            if ((windowWidth < 600 || windowHeight <= 440))
            {
                CalculationGrid.Visibility = Visibility.Collapsed;
                Grid.SetRow(FDAccountGrid, 0);
                Grid.SetRow(CalculationGrid, 1);
                Grid.SetRowSpan(FDAccountGrid, 1);
                Grid.SetRowSpan(CalculationGrid, 1);
                Grid.SetColumn(FDAccountGrid, 0);
                Grid.SetColumn(CalculationGrid, 0);
                Grid.SetColumnSpan(FDAccountGrid, 2);
                Grid.SetColumnSpan(CalculationGrid, 2);
                CalculationGrid.HorizontalAlignment = HorizontalAlignment.Center;
                Grid.SetRowSpan(ErrorMessage, 2);
                Grid.SetColumnSpan(ErrorMessage, 2);
            }
            else
            {
                CalculationGrid.Visibility = Visibility.Visible;
                Grid.SetRow(FDAccountGrid, 0);
                Grid.SetRow(CalculationGrid, 0);
                Grid.SetRowSpan(FDAccountGrid, 2);
                Grid.SetRowSpan(CalculationGrid, 2);
                Grid.SetColumn(FDAccountGrid, 0);
                Grid.SetColumn(CalculationGrid, 1);
                Grid.SetColumnSpan(FDAccountGrid, 1);
                Grid.SetColumnSpan(CalculationGrid, 1);
                CalculationGrid.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRowSpan(ErrorMessage, 2);
                Grid.SetColumnSpan(ErrorMessage, 1);
            }
        }

        private void YearComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)YearComboBox.SelectedItem;

            // If the selected value is 10, disable the other ComboBox controls
            if (selectedItem != null && selectedItem.Content.ToString() == "10")
            {
                MonthComboBox.IsEnabled = false;
                DayComboBox.IsEnabled = false;
            }
            else
            {
                MonthComboBox.IsEnabled = true;
                DayComboBox.IsEnabled = true;
            }
            CallCalculate();
        }
        public void CallCalculate()
        {
            BasicValidation();
            if (AccountTypeBox.Content != null && CustomerTypeBox.Content != null && !string.IsNullOrEmpty(AmountTextBox.Text) && (_selectedMonths != 0 || _selectedYears != 0 || _selectedDays != 0))
            {
                _accountType = (FDType)Enum.Parse(typeof(FDType), AccountTypeBox.Content as string);
                _customerType = (CustomerType)Enum.Parse(typeof(CustomerType), CustomerTypeBox.Content as string);
                if ((double.Parse(AmountTextBox.Text) > 0.0))
                {
                    _fDAccountViewModel.OpenAccount = false;
                    _fDAccountViewModel.CalculateFD(double.Parse(AmountTextBox.Text), _selectedYears, _selectedMonths, _selectedDays, _customerType, _accountType, User.UserId);
                }
            }
        }
        private void AmountTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            CallCalculate();
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallCalculate();
        }

        private void DayComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CallCalculate();
        }
    }
}
