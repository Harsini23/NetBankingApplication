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
        private string currentUserId;
        private AddPayeeBaseViewModel AddPayeeViewModel;

        public event Action<string> RaiseNotification;

        public AddPayeeView(string userId,string payeeName="",string accountNumber="")
        {
            this.InitializeComponent();
            AddPayeeViewModel = PresenterService.GetInstance().Services.GetService<AddPayeeBaseViewModel>();
            AddPayeeViewModel.AddPayeeView = this;
            currentUserId = userId;
            PayeeName.Text = payeeName;
            Accountnumber.Text = accountNumber;
         //   Result.Text = "";

        }
        public AddPayeeView() { }
        private void AddPayee_Click(object sender, RoutedEventArgs e)
        {
            if (AccountHolderName.Text==String.Empty|| Accountnumber.Text==String.Empty|| IfscCode.Text==String.Empty|| BankName.Text==String.Empty|| PayeeName.Text==String.Empty)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "All fields are required";
            }
            else
            {
                Payee newRecipent = new Payee { UserID = currentUserId, AccountHolderName = AccountHolderName.Text, AccountNumber = Accountnumber.Text, IfscCode = IfscCode.Text, BankName = BankName.Text, PayeeName = PayeeName.Text };
                AddPayeeViewModel.AddPayee(newRecipent);
                PayeeName.Text = String.Empty;
                AccountHolderName.Text = String.Empty;
                Accountnumber.Text = String.Empty;
                IfscCode.Text = String.Empty;
                BankName.Text = String.Empty;
                ErrorMessage.Text= String.Empty;

                //AddPayeeDialog.ShowAsync();
                //DispatcherTimer timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(1);
                //timer.Tick += (s, args) =>
                //{
                //    AddPayeeDialog.Hide();
                //    timer.Stop();
                //};
                //timer.Start();



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
    }
}
