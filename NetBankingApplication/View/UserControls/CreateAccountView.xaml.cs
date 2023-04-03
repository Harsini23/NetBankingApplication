using Library.Model;
using Library.Model.Enum;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View.Converter;
using NetBankingApplication.ViewModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NetBankingApplication.View.UserControls
{
    public sealed partial class CreateAccountView : UserControl
    {
        public string ErrorMessage;
        IEnumerable<AccountType> _AccountTypeValues;
        IEnumerable<BasicInitialUserAccountType> _basicInitialUserAccountTypes;
        IEnumerable<Currency> _CurrencyValues;
        private string SelectedBranch;
        private string SelectedAccountType;
        private string SelectedCurrency;
        private string _amount;
        private GetBranchDetailsBaseViewModel GetBranchDetailsViewModel;
        public bool FirstAccountCreation { get; set; }
        AccountTypeToStringConverter accountTypeToStringConverter;
        public CreateAccountView()
        {
            this.InitializeComponent();
            _AccountTypeValues = Enum.GetValues(typeof(AccountType)).Cast<AccountType>();
           _basicInitialUserAccountTypes= Enum.GetValues(typeof(BasicInitialUserAccountType)).Cast<BasicInitialUserAccountType>();
            _CurrencyValues = Enum.GetValues(typeof(Currency)).Cast<Currency>();
            GetBranchDetailsViewModel = PresenterService.GetInstance().Services.GetService<GetBranchDetailsBaseViewModel>();
            GetBranchDetailsViewModel.FetchBranchDetails();
            SelectedCurrency= "INR";
            accountTypeToStringConverter = new AccountTypeToStringConverter();
        }

        MenuFlyout Currencies;
        MenuFlyout AllAccountTypes;
        private void MenuFlyout_Opening_AccountType(object sender, object e)
        {
            AllAccountTypes = sender as MenuFlyout;
            AllAccountTypes.Items.Clear();
            if (FirstAccountCreation)
            {
                foreach (var accType in _basicInitialUserAccountTypes)
                {
                    var item = new MenuFlyoutItem();
                    item.Text = (string)accountTypeToStringConverter.Convert(accType, typeof(string), null, string.Empty);
                    item.HorizontalAlignment = HorizontalAlignment.Stretch;
                    item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    item.CornerRadius = new CornerRadius(5);
                    //item.Width = 270;
                    item.Click += AccountType_Selection;
                    AllAccountTypes.Items.Add(item);
                }
            }
            else
            {
                foreach (var accType in _AccountTypeValues)
                {
                    var item = new MenuFlyoutItem();
                    item.Text = (string)accountTypeToStringConverter.Convert(accType, typeof(string), null, string.Empty);
                    item.HorizontalAlignment = HorizontalAlignment.Stretch;
                    item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                    item.CornerRadius = new CornerRadius(5);
                    //item.Width = 270;
                    item.Click += AccountType_Selection;
                    AllAccountTypes.Items.Add(item);
                }
            }

        }
        private void AccountType_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            SelectedAccountType = selectedItem.Text;
            AccountTypeBox.Content = selectedItem.Text;

        }

        //private void MenuFlyout_Opening_Currency(object sender, object e)
        //{
        //    Currencies = sender as MenuFlyout;
        //    Currencies.Items.Clear();
        //    foreach (var currency in _CurrencyValues)
        //    {

        //        var item = new MenuFlyoutItem();
        //        item.Text = currency.ToString();
        //        item.HorizontalAlignment = HorizontalAlignment.Stretch;
        //        item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
        //        item.CornerRadius = new CornerRadius(5);
        //        //item.Width = 270;
        //        item.Click += Currency_Selection;
        //        Currencies.Items.Add(item);
        //    }

        //}
        //private void Currency_Selection(object sender, RoutedEventArgs e)
        //{
        //    var selectedItem = sender as MenuFlyoutItem;
        //    SelectedCurrency = selectedItem.Text;
        //    CurrencyValues.Content = selectedItem.Text;

        //}
        private void BalanceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var amountBox = (TextBox)sender;
            ErrorMessage = String.Empty;
            _amount = amountBox.Text.ToString();
            double val = 0.0;
            if (amountBox.Text.Length > 0)
            {
                var parsable = double.TryParse(BalanceTextBox.Text, out val);

                if (Math.Abs(val % 1) >= 0.0001)
                {
                    BalanceTextBox.Text = Math.Round(val, 2).ToString();
                    BalanceTextBox.SelectionStart = BalanceTextBox.Text.Length;
                }
            }


            if (_amount == String.Empty)
            {
                ErrorMessage = String.Empty;
            }

            else if (!IsFloatOrInt(_amount) || Double.Parse(_amount) <= 0 && !String.IsNullOrEmpty(_amount))
            {
          

                ErrorMessage = "Enter valid amount";
                //MakeTransaction.IsEnabled = false;
            }
            else
            {
                ErrorMessage = String.Empty;
            }
        }
        private static bool IsFloatOrInt(string value)
        {
            int intValue;
            float floatValue;
            return Int32.TryParse(value, out intValue) || float.TryParse(value, out floatValue);
        }



        MenuFlyout allBranches;
        private void MenuFlyout_Opening(object sender, object e)
        {
            allBranches = sender as MenuFlyout;
            allBranches.Items.Clear();
            foreach (var i in GetBranchDetailsViewModel.allBranchDetails)
            {
                var item = new MenuFlyoutItem();
                item.Text = i.BId + " - " + i.BCity;
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                item.CornerRadius = new CornerRadius(5);
                //item.Width = 270;
                item.Name = i.BId.ToString();
                item.Click += Account_Selection;
                //item.HorizontalContentAlignment = HorizontalAlignment.Left;
                allBranches.Items.Add(item);
            }
        }
        private void Account_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            SelectedBranch = selectedItem.Text.Substring(0, selectedItem.Text.IndexOf("-")).Trim();
            SelectBranch.Content = selectedItem.Text;
            // GetAllAccountsViewModel.CurrentAccountBalance = selectedItem.Name;

        }
        private void TextBox_BalanceOnBeforeTextChanging(TextBox sender,
                                  TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c) && c != '.');

        }
        public AccountVobj FetchData()
        {
           AccountType accountType ;
            if (SelectedAccountType != null)
            {
                 accountType = (AccountType)(accountTypeToStringConverter.ConvertBack(SelectedAccountType, typeof(AccountType), null, null));
            }
            else
            {
                accountType = AccountType.None;
            }
            return new AccountVobj(accountType, BalanceTextBox.Text, SelectedCurrency, SelectedBranch);

        }
        public void ClearUI()
        {
            BalanceTextBox.Text = "";
            AccountTypeBox.Content = "";
            //CurrencyValues.Content = "";
            SelectBranch.Content = "";
        }
    }
   
}
