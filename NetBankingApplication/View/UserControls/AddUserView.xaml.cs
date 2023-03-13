using Library.Model;
using Library.Model.Enum;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
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
    public sealed partial class AddUserView : UserControl,IAddUserView, ShowResponseNotification
    {

        private AddUserBaseViewModel AddUserViewModel;
        IEnumerable<AccountType> _AccountTypeValues;
        IEnumerable<Currency> _CurrencyValues;
        private GetBranchDetailsBaseViewModel GetBranchDetailsViewModel;

        private string SelectedBranch;
        private string SelectedAccountType;
        private string SelectedCurrency;

        public AddUserView()
        {
            this.InitializeComponent();
            AddUserViewModel = PresenterService.GetInstance().Services.GetService<AddUserBaseViewModel>();
            GetBranchDetailsViewModel = PresenterService.GetInstance().Services.GetService<GetBranchDetailsBaseViewModel>();
            AddUserViewModel.adduserView = this;
            AddUserViewModel.addUserNotification = this;
            GetBranchDetailsViewModel.FetchBranchDetails();
            _AccountTypeValues = Enum.GetValues(typeof(AccountType)).Cast<AccountType>();
            _CurrencyValues = Enum.GetValues(typeof(Currency)).Cast<Currency>();
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender,
                                          TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }
      


        private void Submit_Click(object sender, RoutedEventArgs e)
        {
           
            if (UserNameTextBox.Text == String.Empty || MobileNumberTextBox.Text == String.Empty || EmailIdTextBox.Text == String.Empty  || BalanceTextBox.Text == String.Empty || SelectBranch.Content == String.Empty || SelectedBranch==null|| SelectedAccountType == null || SelectedCurrency == null)
            {
                AddUserViewModel.ErrorMessage = "All fields are required*";
            }

            else if (MobileNumberTextBox.Text.Length != 10)
            {
                AddUserViewModel.ErrorMessage = "Enter a valid mobile number";
            }
            else if (!this.EmailIdTextBox.Text.Contains('@') || !this.EmailIdTextBox.Text.Contains('.') || !this.EmailIdTextBox.Text.Contains("com"))
            {
                AddUserViewModel.ErrorMessage = "Enter a valid email id";
            }
            else if (PANTextBox.Text.Length != 10)
            {
                AddUserViewModel.ErrorMessage = "PAN number must be of 10 values";
            }
            else
            {
                UserAccountDetails details = new UserAccountDetails
                {
                    UserName = UserNameTextBox.Text.Trim(),
                    MobileNumber = long.Parse(MobileNumberTextBox.Text),
                    EmailId = EmailIdTextBox.Text.Trim(),
                    AccountType = (AccountType)Enum.Parse(typeof(AccountType),SelectedAccountType),
                    TotalBalance = Double.Parse(BalanceTextBox.Text),
                    Currency = (Currency)Enum.Parse(typeof(Currency), SelectedCurrency),
                    BId = SelectedBranch,
                    PAN = PANTextBox.Text.Trim().ToString()
                };


                AddUserViewModel.AddUser(details);


                UserNameTextBox.Text = String.Empty;
                MobileNumberTextBox.Text = String.Empty;
                EmailIdTextBox.Text = String.Empty;
                //BranchIdTextBox.Text = String.Empty;
                BalanceTextBox.Text = String.Empty;
                AccountTypeBox.Content = "";
                CurrencyValues.Content = "";
                AddUserViewModel.ErrorMessage = String.Empty;
                PANTextBox.Text = String.Empty;
                SelectBranch.Content = "";
                //ShowContentDialogueAsync();

            }
        }

        private void PanNumberTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {

        }

     
        private void Idcopy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(AddUserViewModel.UserId.ToString());
            Clipboard.SetContent(dataPackage);
        }
        private void Passwordcopy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText(AddUserViewModel.Password.ToString());
            Clipboard.SetContent(dataPackage);
        }



        MenuFlyout allBranches;
        private void MenuFlyout_Opening(object sender, object e)
        {
            allBranches = sender as MenuFlyout;
            allBranches.Items.Clear();
            foreach (var i in GetBranchDetailsViewModel.allBranchDetails)
            {
                var item = new MenuFlyoutItem();
                item.Text = i.BId+" - "+i.BCity;
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
            SelectedBranch = selectedItem.Text;
            SelectBranch.Content = selectedItem.Text;
           // GetAllAccountsViewModel.CurrentAccountBalance = selectedItem.Name;
           
        }

        MenuFlyout AllAccountTypes;
        private void MenuFlyout_Opening_AccountType(object sender, object e)
        {
            AllAccountTypes = sender as MenuFlyout; 
            AllAccountTypes.Items.Clear();
            foreach(var accType in _AccountTypeValues)
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

        MenuFlyout Currencies;
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

         async void IAddUserView.ShowContentDialogueAsync()
        {
            var result = await ContentDialog.ShowAsync();
            Bindings.Update();
        }

        public void NotificationUpdate()
        {
            InAppNotification.Show(AddUserViewModel.Response, 3000);
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }
    }
}
