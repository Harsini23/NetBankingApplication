using Library.Model;
using Library.Model.Enum;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class AddAccount : UserControl, INotifyPropertyChanged, IAccountAddedNotification
    {
        private GetAllUsersBaseViewModel allUsersViewModel;
        private ObservableCollection<User> users;
        public event PropertyChangedEventHandler PropertyChanged;
        private AddAccountBaseViewModel addAccountBaseViewModel;

        //------------------
        IEnumerable<AccountType> _AccountTypeValues;
        IEnumerable<Currency> _CurrencyValues;
        private string SelectedBranch;
        private string SelectedAccountType;
        private string SelectedCurrency;
        private string _amount;
        private GetBranchDetailsBaseViewModel GetBranchDetailsViewModel;

        public AddAccount()
        {
            this.InitializeComponent();
            allUsersViewModel = PresenterService.GetInstance().Services.GetService<GetAllUsersBaseViewModel>();
            allUsersViewModel.GetAllUsers();
            users = allUsersViewModel.AllUsers;

            //-------------
            _AccountTypeValues = Enum.GetValues(typeof(AccountType)).Cast<AccountType>();
            _CurrencyValues = Enum.GetValues(typeof(Currency)).Cast<Currency>();
            GetBranchDetailsViewModel = PresenterService.GetInstance().Services.GetService<GetBranchDetailsBaseViewModel>();
            GetBranchDetailsViewModel.FetchBranchDetails();

            addAccountBaseViewModel = PresenterService.GetInstance().Services.GetService<AddAccountBaseViewModel>();
            addAccountBaseViewModel.addAccountView = this;

        }

        private User _currentSelectedUser ;
        public User CurrentSelectedUser
        {
            get { return _currentSelectedUser; }
            set
            {
                _currentSelectedUser = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            users = allUsersViewModel.AllUsers;
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var suitableItems = new ObservableCollection<string>();
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var user in users)
                {
                    var found = splitText.All((key) =>
                    {
                        return user.UserId.ToLower().Contains(key);
                    });
                    if (found)
                    {
                        suitableItems.Add(user.UserId);
                    }
                }
                if (suitableItems.Count == 0)
                {
                    suitableItems.Add("No results found");
                }
                sender.ItemsSource = suitableItems;
            }
            if (String.IsNullOrEmpty(UserAutoSuggestBox.Text))
            {
                UserInfoGrid.Visibility = Visibility.Collapsed;
                CurrentSelectedUser=null;
            }

        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            UserInfoGrid.Visibility = Visibility.Visible;
            CurrentSelectedUser= users.FirstOrDefault(user => user.UserId == args.SelectedItem.ToString());
            UserAutoSuggestBox.Text = args.SelectedItem.ToString();

        }
        private void TextBox_BalanceOnBeforeTextChanging(TextBox sender,
                                      TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c) && c != '.');

        }

        private void UserAutoSuggestBox_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            users = allUsersViewModel.AllUsers;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            //add new account for existing user
            if(CurrentSelectedUser==null || string.IsNullOrEmpty(SelectedAccountType) || string.IsNullOrEmpty(SelectedCurrency) || string.IsNullOrEmpty(SelectedCurrency)|| string.IsNullOrEmpty(BalanceTextBox.Text))
            {
                ErrorMessage.Text = "Kindly fill all details!";
            }
            else
            {
                addAccountBaseViewModel.AddAccount(new AccountBObj(CurrentSelectedUser.UserId, (AccountType)Enum.Parse(typeof(AccountType), SelectedAccountType), Double.Parse(BalanceTextBox.Text), (Currency)Enum.Parse(typeof(Currency), SelectedCurrency), SelectedBranch,CurrentSelectedUser.UserName));
                ClearUI();
            }
            
        }

        private void ClearUI()
        {
            UserAutoSuggestBox.Text = "";
            UserInfoGrid.Visibility = Visibility.Collapsed;
            BalanceTextBox.Text = "";
            AccountTypeBox.Content = "";
            CurrencyValues.Content = "";
            ErrorMessage.Text = String.Empty;
            SelectBranch.Content = "";
        }
        //--------------------------------------------------------------------




        MenuFlyout Currencies;
        MenuFlyout AllAccountTypes;
        private void MenuFlyout_Opening_AccountType(object sender, object e)
        {
            AllAccountTypes = sender as MenuFlyout;
            AllAccountTypes.Items.Clear();
            foreach (var accType in _AccountTypeValues)
            {

                var item = new MenuFlyoutItem();
                item.Text = accType.ToString();
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                item.CornerRadius = new CornerRadius(5);
                //item.Width = 270;
                item.Click += AccountType_Selection;
                AllAccountTypes.Items.Add(item);
            }
        }
        private void AccountType_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            SelectedAccountType = selectedItem.Text;
            AccountTypeBox.Content = selectedItem.Text;

        }

        private void MenuFlyout_Opening_Currency(object sender, object e)
        {
            Currencies = sender as MenuFlyout;
            Currencies.Items.Clear();
            foreach (var currency in _CurrencyValues)
            {

                var item = new MenuFlyoutItem();
                item.Text = currency.ToString();
                item.HorizontalAlignment = HorizontalAlignment.Stretch;
                item.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                item.CornerRadius = new CornerRadius(5);
                //item.Width = 270;
                item.Click += Currency_Selection;
                Currencies.Items.Add(item);
            }

        }
        private void Currency_Selection(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            SelectedCurrency = selectedItem.Text;
            CurrencyValues.Content = selectedItem.Text;

        }
        private void BalanceTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var amountBox = (TextBox)sender;
            ErrorMessage.Text = String.Empty;
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
                ErrorMessage.Text = String.Empty;
            }

            else if (!IsFloatOrInt(_amount) || Double.Parse(_amount) <= 0 && !String.IsNullOrEmpty(_amount))
            {
                ErrorMessage.Visibility = Visibility.Visible;

                ErrorMessage.Text = "Enter valid amount";
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


        private void UserAutoSuggestBox_LosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            UserAutoSuggestBox.IsSuggestionListOpen = false;

        }

        public void AccountNotification()
        {
            InAppNotification.Show(addAccountBaseViewModel.Response, 3000);

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }

    }
}
