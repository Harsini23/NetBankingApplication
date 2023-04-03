using Library.Model;
using Library.Model.Enum;
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
    public sealed partial class BankAccount : UserControl, INotifyPropertyChanged
    {
        //public static string currentUserId;
        //public User CurrentUser;
        private AddAccountBaseViewModel addAccountBaseViewModel;

        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public BankAccount()
        {
            this.InitializeComponent();
            addAccountBaseViewModel = PresenterService.GetInstance().Services.GetService<AddAccountBaseViewModel>();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private UserControl _currentSelectedItem;
        public UserControl CurrentSelectedItem
        {
            get { return _currentSelectedItem; }
            set
            {
                _currentSelectedItem = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //CurrentSelectedItem = new AllAccountsPreview(User.UserId);
            BankAccountNavigation.SelectedItem = AccountsPreview;
        }


        private void BankAccountNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            if (args.SelectedItem == AccountsPreview)
            {
                AllAccountsPreview allAccountsPreview = new AllAccountsPreview();
                allAccountsPreview.User = User;
                CurrentSelectedItem = allAccountsPreview;
            }
            else if (args.SelectedItem == AccountDetails)
            {
                DetailedAccountOverview detailedAccountOverview = new DetailedAccountOverview();
                detailedAccountOverview.User = User;
                CurrentSelectedItem = detailedAccountOverview;
            }
            else
            {
                AllAccountsPreview allAccountsPreview = new AllAccountsPreview();
                allAccountsPreview.User = User;
                CurrentSelectedItem = allAccountsPreview;
            }

        }


        private void AccountDropdown_Opening(object sender, object e)
        {
            //selectAccountList = sender as MenuFlyout;
            //selectAccountList.Items.Clear();
            //foreach (var i in GetAllAccountsViewModel.AllAccountNumbers)
            //{
            //    var item = new MenuFlyoutItem();
            //    item.Text = i;
            //    item.Click += Account_Selection; ;
            //    item.MinWidth = 150;
            //    selectAccountList.Items.Add(item);
            //}
        }

        private void CreateAccount_Click(object sender, RoutedEventArgs e)
        {
            CreateAccountGrid.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - CreateAccountGrid.ActualWidth / 2 +100;
            double verticalOffset = Window.Current.Bounds.Height / 2 - CreateAccountGrid.ActualHeight / 2 ;
            CreateAccountGrid.HorizontalOffset = horizontalOffset;
            CreateAccountGrid.VerticalOffset = verticalOffset;
        }

        private void CreateAccountGrid_Closed(object sender, object e)
        {
            CreateNewAccountViewComponent.ClearUI();
            ErrorMessage.Text="";
        }

        private void CreateNewAccount_Click(object sender, RoutedEventArgs e)
        {
            //add new account for existing user
            AccountVobj accountDetails = CreateNewAccountViewComponent.FetchData();

            if (string.IsNullOrEmpty(accountDetails.Balance) || string.IsNullOrEmpty(accountDetails.Branch) || string.IsNullOrEmpty(accountDetails.Currency) || accountDetails.AccountType==AccountType.None)
            {
                ErrorMessage.Text = "Kindly fill account details";
            }
            else if (Double.Parse(accountDetails.Balance) <= 1 && accountDetails.AccountType != AccountType.SalaryAccount)
            {
                ErrorMessage.Text = "Only savings account can have zero balance!";
            }
            else
            {
                addAccountBaseViewModel.AddAccount(new AccountBObj(User.UserId,accountDetails.AccountType, Double.Parse(accountDetails.Balance), (Currency)Enum.Parse(typeof(Currency), accountDetails.Currency), accountDetails.Branch, User.UserName));
                CreateNewAccountViewComponent.ClearUI();
                CreateAccountGrid.IsOpen = false;
            }

        }
      
    }
}
