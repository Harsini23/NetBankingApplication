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

  

        public AddAccount()
        {
            this.InitializeComponent();
            allUsersViewModel = PresenterService.GetInstance().Services.GetService<GetAllUsersBaseViewModel>();
            allUsersViewModel.GetAllUsers();
            users = allUsersViewModel.AllUsers;

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
            AccountVobj accountDetails= CreateAccountViewDetails.FetchData();
         
            if (CurrentSelectedUser == null)
            {
                ErrorMessage.Text = "Kindly fill user details!";
            }
            else if (string.IsNullOrEmpty(accountDetails.Balance) || string.IsNullOrEmpty(accountDetails.Branch) || string.IsNullOrEmpty(accountDetails.Currency) || string.IsNullOrEmpty(accountDetails.AccountType))
            {
                ErrorMessage.Text = "Kindly fill account details";
            }
            else if(Double.Parse(accountDetails.Balance)<=1 && accountDetails.AccountType != "SalaryAccount")
            {
                ErrorMessage.Text = "Only savings account can have zero balance!";
            }
            else
            {
                addAccountBaseViewModel.AddAccount(new AccountBObj(CurrentSelectedUser.UserId, (AccountType)Enum.Parse(typeof(AccountType), accountDetails.AccountType), Double.Parse(accountDetails.Balance), (Currency)Enum.Parse(typeof(Currency), accountDetails.Currency), accountDetails.Branch,CurrentSelectedUser.UserName));
                ClearUI();
                CreateAccountViewDetails.ClearUI();
            }
            
        }

        private void ClearUI()
        {
            UserAutoSuggestBox.Text = "";
            UserInfoGrid.Visibility = Visibility.Collapsed;
            ErrorMessage.Text = String.Empty;
        }
    
        private static bool IsFloatOrInt(string value)
        {
            int intValue;
            float floatValue;
            return Int32.TryParse(value, out intValue) || float.TryParse(value, out floatValue);
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
