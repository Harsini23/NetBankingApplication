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
        PresenterService AddUserVMserviceProviderInstance;


        IEnumerable<AccountType> _AccountTypeValues;
        IEnumerable<Currency> _CurrencyValues;
        public AddUserView()
        {
            this.InitializeComponent();
            AddUserVMserviceProviderInstance = PresenterService.GetInstance();
            AddUserViewModel = AddUserVMserviceProviderInstance.Services.GetService<AddUserBaseViewModel>();
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
            if (UserNameTextBox.Text==String.Empty||MobileNumberTextBox.Text==String.Empty||EmailIdTextBox.Text==String.Empty||UserAccountNumberTextBox.Text==String.Empty|| BalanceTextBox.Text==String.Empty|| BranchIdTextBox.Text==String.Empty|| AccountTypeBox.SelectedItem==null|| CurrencyValues.SelectedItem==null)
            {
                AddUserViewModel.ErrorMessage = "All fields are required*";
            }

            else if (MobileNumberTextBox.Text.Length!=10)
            {
                AddUserViewModel.ErrorMessage = "Enter a valid mobile number";
            }
            else if(!this.EmailIdTextBox.Text.Contains('@') || !this.EmailIdTextBox.Text.Contains('.')|| !this.EmailIdTextBox.Text.Contains("com")){
                AddUserViewModel.ErrorMessage = "Enter a valid email id";
            }
            else if (PANTextBox.Text.Length!=10)
            {
                AddUserViewModel.ErrorMessage = "Enter valid PAN number";
            }
            else
            {
                UserAccountDetails details = new UserAccountDetails
                {
                    UserName = UserNameTextBox.Text,
                    MobileNumber = long.Parse(MobileNumberTextBox.Text),
                    EmailId = EmailIdTextBox.Text,
                    AccountNumber = UserAccountNumberTextBox.Text,
                    AccountType = Enum.Parse<AccountType>(AccountTypeBox.SelectedItem.ToString()),
                    TotalBalance = Double.Parse(BalanceTextBox.Text),
                    Currency = Enum.Parse<Currency>(CurrencyValues.SelectedItem.ToString()),
                    BId = BranchIdTextBox.Text.ToString()
                };


                AddUserViewModel.AddUser(details);


                UserNameTextBox.Text = String.Empty;
                MobileNumberTextBox.Text = String.Empty;
                EmailIdTextBox.Text = String.Empty;
                BranchIdTextBox.Text = String.Empty;
                BalanceTextBox.Text = String.Empty;
                UserAccountNumberTextBox.Text = String.Empty;
                AccountTypeBox.SelectedItem = null;
                CurrencyValues.SelectedItem = null;
                AddUserViewModel.ErrorMessage = String.Empty;
            }
        

        }

        private void PanNumberTextBox_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {

        }

     
    }
}
