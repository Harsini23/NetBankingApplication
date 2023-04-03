using Library.Model;
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
    public sealed partial class AddPayeeView : UserControl, INotificationAlert
    {
        private AddPayeeBaseViewModel AddPayeeViewModel;

        public event Action<string> RaiseNotification;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }

        public static readonly DependencyProperty PayeeNameProperty = DependencyProperty.Register(nameof(PassedPayeeName), typeof(string), typeof(Overview), new PropertyMetadata(null));
        public string PassedPayeeName
        {
            get { return (string)GetValue(PayeeNameProperty); }
            set { SetValue(PayeeNameProperty, value); }
        }
        public static readonly DependencyProperty AccountNumberProperty = DependencyProperty.Register(nameof(PassedAccountNumber), typeof(string), typeof(Overview), new PropertyMetadata(null));
        public string PassedAccountNumber
        {
            get { return (string)GetValue(AccountNumberProperty); }
            set { SetValue(AccountNumberProperty, value); }
        }

        public AddPayeeView()
        {
            this.InitializeComponent();
            AddPayeeViewModel = PresenterService.GetInstance().Services.GetService<AddPayeeBaseViewModel>();
            AddPayeeViewModel.AddPayeeView = this;

        }
        private void AddPayee_Click(object sender, RoutedEventArgs e)
        {
            if (AccountHolderName.Text==String.Empty|| Accountnumber.Text==String.Empty|| IfscCode.Text==String.Empty|| BankName.Text==String.Empty|| PayeeName.Text==String.Empty)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "All fields are required";
            }
            else
            {
                Payee newRecipent = new Payee { UserID = User.UserId, AccountHolderName = AccountHolderName.Text, AccountNumber = Accountnumber.Text, IfscCode = IfscCode.Text, BankName = BankName.Text, PayeeName = PayeeName.Text };
                AddPayeeViewModel.AddPayee(newRecipent);
                PayeeName.Text = String.Empty;
                AccountHolderName.Text = String.Empty;
                Accountnumber.Text = String.Empty;
                IfscCode.Text = String.Empty;
                BankName.Text = String.Empty;
                ErrorMessage.Text= String.Empty;

           
            }

           
        }

        private void TextBox_OnBeforeTextChanging(TextBox sender,
                                          TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        public void CallNotification()
        {
            RaiseNotification?.Invoke(AddPayeeViewModel.AddPayeeResponseValue);

        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            ErrorMessage.Visibility = Visibility.Collapsed;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if(PassedPayeeName!=null && PassedAccountNumber != null)
            {
                PayeeName.Text = PassedPayeeName;
                Accountnumber.Text = PassedAccountNumber;
            }
        }
    }
}
