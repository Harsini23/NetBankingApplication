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
    public sealed partial class AddUserView : UserControl, IAddUserView, ShowResponseNotification
    {

        private AddUserBaseViewModel AddUserViewModel;
        public AddUserView()
        {
            this.InitializeComponent();
            AddUserViewModel = PresenterService.GetInstance().Services.GetService<AddUserBaseViewModel>();
            AddUserViewModel.adduserView = this;
            AddUserViewModel.addUserNotification = this;
         
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender,
                                          TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));

        }
        private void TextBox_BalanceOnBeforeTextChanging(TextBox sender,
                                        TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c) && c != '.');

        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var accountDetails = CreateAccountViewDetails.FetchData();

            if (UserNameTextBox.Text == String.Empty || MobileNumberTextBox.Text == String.Empty || EmailIdTextBox.Text == String.Empty )
            {
                AddUserViewModel.ErrorMessage = "All fields are required*";
            }

            else if (MobileNumberTextBox.Text.Length != 10)
            {
                AddUserViewModel.ErrorMessage = "Enter a valid mobile number";
            }
            else if (!this.EmailIdTextBox.Text.Contains('@') || !this.EmailIdTextBox.Text.Contains('.'))
            {
                AddUserViewModel.ErrorMessage = "Enter a valid email id";
            }
            else if (PANTextBox.Text.Length != 10)
            {
                AddUserViewModel.ErrorMessage = "PAN number must be of 10 values";
            }
            else if (string.IsNullOrEmpty(accountDetails.Balance) || string.IsNullOrEmpty(accountDetails.Branch) || string.IsNullOrEmpty(accountDetails.Currency) || accountDetails.AccountType==null)
            {
                AddUserViewModel.ErrorMessage = "Enter all account details";
            }
            else if (Double.Parse(accountDetails.Balance) <= 1 && accountDetails.AccountType != AccountType.SalaryAccount)
            {
                ErrorMessage.Text = "Only savings account can have zero balance!";
            }
            else
            {
                UserAccountDetails details = new UserAccountDetails
                {
                    UserName = UserNameTextBox.Text.Trim(),
                    MobileNumber = long.Parse(MobileNumberTextBox.Text),
                    EmailId = EmailIdTextBox.Text.Trim(),
                    AccountType = accountDetails.AccountType,
                    TotalBalance = Double.Parse(accountDetails.Balance),
                    Currency = (Currency)Enum.Parse(typeof(Currency), accountDetails.Currency),
                    BId = accountDetails.Branch,
                    PAN = PANTextBox.Text.Trim().ToString()
                };


                AddUserViewModel.AddUser(details);

                CreateAccountViewDetails.ClearUI();
                UserNameTextBox.Text = String.Empty;
                MobileNumberTextBox.Text = String.Empty;
                EmailIdTextBox.Text = String.Empty;
                AddUserViewModel.ErrorMessage = String.Empty;
                PANTextBox.Text = String.Empty;
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
        private static bool IsFloatOrInt(string value)
        {
            int intValue;
            float floatValue;
            return Int32.TryParse(value, out intValue) || float.TryParse(value, out floatValue);
        }

    }
}
