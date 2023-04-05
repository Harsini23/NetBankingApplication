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
    public sealed partial class FDAccountView : UserControl, INotifyPropertyChanged, ISwitchUserView
    {
        IEnumerable<FDType> _FDTypeValues;
        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;
        private FDAccountBaseViewModel FDAccountViewModel;
        private string UserAccountNumber;
        private string today;
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
            GetAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            FDAccountViewModel = PresenterService.GetInstance().Services.GetService<FDAccountBaseViewModel>();
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
                item.Click += Item_Click; ;
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
            SelectAccount.Content = selectedItem.Text;
            GetAllAccountsViewModel.CurrentAccountBalance = selectedItem.Name;
        }

        private void CalculateFD_Click(object sender, RoutedEventArgs e)
        {
            int selectedMonths=0;
            int selectedYears=0;
            int selectedDays=0;
            ComboBoxItem MonthselectedItem = MonthComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem YearselectedItem = YearComboBox.SelectedItem as ComboBoxItem;
            ComboBoxItem DayselectedItem = DayComboBox.SelectedItem as ComboBoxItem;
            if (MonthselectedItem != null )
            {
                 selectedMonths = int.Parse(MonthselectedItem.Content as string);
            }
            if(YearselectedItem != null)
            {
                selectedYears = int.Parse(YearselectedItem.Content as string);
            }
            if (DayselectedItem!=null)
            {
                selectedDays = int.Parse(DayselectedItem.Content as string);
            }
            if (!string.IsNullOrEmpty(AmountTextBox.Text))
            {
                FDAccountViewModel.CalculateFD(double.Parse(AmountTextBox.Text), selectedYears, selectedMonths, selectedDays);
            }
        }
    }
}
