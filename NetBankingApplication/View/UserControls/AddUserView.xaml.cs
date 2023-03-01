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
    public sealed partial class AddUserView : UserControl
    {

        private AddUserBaseViewModel AddUserViewModel;
        IEnumerable<AccountType> _AccountTypeValues;
        IEnumerable<Currency> _CurrencyValues;
        private GetBranchDetailsBaseViewModel GetBranchDetailsViewModel;

        private string SelectedBranch;

        public AddUserView()
        {
            this.InitializeComponent();
            AddUserViewModel = PresenterService.GetInstance().Services.GetService<AddUserBaseViewModel>();
            GetBranchDetailsViewModel = PresenterService.GetInstance().Services.GetService<GetBranchDetailsBaseViewModel>();
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
           
            if (UserNameTextBox.Text == String.Empty || MobileNumberTextBox.Text == String.Empty || EmailIdTextBox.Text == String.Empty  || BalanceTextBox.Text == String.Empty || SelectBranch.Content == String.Empty || SelectedBranch==null|| AccountTypeBox.SelectedItem == null || CurrencyValues.SelectedItem == null)
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
                    UserName = UserNameTextBox.Text,
                    MobileNumber = long.Parse(MobileNumberTextBox.Text),
                    EmailId = EmailIdTextBox.Text,
                    AccountType = Enum.Parse<AccountType>(AccountTypeBox.SelectedItem.ToString()),
                    TotalBalance = Double.Parse(BalanceTextBox.Text),
                    Currency = Enum.Parse<Currency>(CurrencyValues.SelectedItem.ToString()),
                    BId = SelectedBranch,
                    PAN = PANTextBox.Text.ToString()
                };


                AddUserViewModel.AddUser(details);


                UserNameTextBox.Text = String.Empty;
                MobileNumberTextBox.Text = String.Empty;
                EmailIdTextBox.Text = String.Empty;
                //BranchIdTextBox.Text = String.Empty;
                BalanceTextBox.Text = String.Empty;
                AccountTypeBox.SelectedItem = null;
                CurrencyValues.SelectedItem = null;
                AddUserViewModel.ErrorMessage = String.Empty;
                PANTextBox.Text = String.Empty;

                ShowContentDialogueAsync();

            }
        }

        public async Task ShowContentDialogueAsync()
        {
            var result = await ContentDialog.ShowAsync();
            Bindings.Update();  
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
                item.Name = i.BId.ToString();
                item.Click += Account_Selection; ;
                item.MinWidth = 150;
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


    }
}
