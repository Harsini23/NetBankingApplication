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
    public sealed partial class AddPayeeView : UserControl
    {
        private string currentUserId;
        private AddPayeeBaseViewModel AddPayeeViewModel;
     
        PresenterService AddPayeeVMserviceProviderInstance;
      
        public AddPayeeView(string userId)
        {
            this.InitializeComponent();
            AddPayeeVMserviceProviderInstance = PresenterService.GetInstance();
            AddPayeeViewModel = AddPayeeVMserviceProviderInstance.Services.GetService<AddPayeeBaseViewModel>();
            currentUserId = userId;
            Result.Text = "";

        }

        private void AddPayee_Click(object sender, RoutedEventArgs e)
        {
            Payee newRecipent = new Payee { UserID = currentUserId, AccountHolderName = AccountHolderName.Text, AccountNumber = Accountnumber.Text, IfscCode = IfscCode.Text, BankName = BankName.Text, PayeeName = PayeeName.Text };
            AddPayeeViewModel.AddPayee(newRecipent);
            PayeeName.Text = String.Empty;
            AccountHolderName.Text = String.Empty;
            Accountnumber.Text = String.Empty;
            IfscCode.Text = String.Empty;
            BankName.Text = String.Empty;

        }

        //private void Testing_Click(object sender, RoutedEventArgs e)
        //{
        //    //PayeeTemplate.PayeeNameProperty
        //    PayeeTemplate p = new PayeeTemplate();
        //}
    }
}
