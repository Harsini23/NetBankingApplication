using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
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
    public sealed partial class TransferAmount : UserControl
    {

        private GetAllPayeeBaseViewModel GetAllPayeeViewModel;

        PresenterService GetAllPayeeVMserviceProviderInstance;

        private TransferAmountBaseViewModel TransferAmountViewModel;

        PresenterService TransferAmountVMserviceProviderInstance;

        List<String> allRecipientNames = new List<String>();
        List<Payee> allRecipients = new List<Payee>();

        public TransferAmount()
        {
            this.InitializeComponent();

            GetAllPayeeVMserviceProviderInstance = PresenterService.GetInstance();
            GetAllPayeeViewModel = GetAllPayeeVMserviceProviderInstance.Services.GetService<GetAllPayeeBaseViewModel>();

            TransferAmountVMserviceProviderInstance = PresenterService.GetInstance();
            TransferAmountViewModel = TransferAmountVMserviceProviderInstance.Services.GetService<TransferAmountBaseViewModel>();

        }
        MenuFlyout selectPayeeList;

        private void MakeTransaction_Click(object sender, RoutedEventArgs e)
        {
            //Grid.SetColumnSpan(TransferAmountDetails,0);
            //Grid.SetRowSpan(TransferAmountDetails,0);
            TransactionDetails.Visibility = Visibility.Visible;
            //get transaction fields

            Transaction currentTransaction = new Transaction{
                TransactionId=" AKFAOJ#$",
                Date="23-11-2022 9:34",
                Status=true,
                FromAccount="78876535412",
                ToAccount="798490832098",
                Name="Monica",
                Remark="Food",
                TransactionAmout="2000",
                TransactionType=Library.Model.Enum.TransactionType.Debited,
                UserId="Harsh",
            };
            TransferAmountViewModel.SendTransaction(currentTransaction);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //load list of payee with all details as payee object
            GetAllPayeeViewModel.GetAllPayee("Harsh");
            allRecipientNames.Clear();
           allRecipientNames= GetAllPayeeViewModel.PayeeNames;
            allRecipients.Clear();
            allRecipients = GetAllPayeeViewModel.AllPayee;

        }

   

        private void MenuFlyout_Opened(object sender, object e)
        {

            selectPayeeList = sender as MenuFlyout;
            selectPayeeList.Items.Clear();
            var defaultItem = new MenuFlyoutItem();
            defaultItem.Text = "New Payee";
            defaultItem.Click += QuickTransaction_Click;
            selectPayeeList.Items.Add(defaultItem);
            foreach (var i in allRecipientNames)
            {
                var item = new MenuFlyoutItem();
                item.Text = i;
                item.Click += Item_Click;
                item.MinWidth = 150;
                selectPayeeList.Items.Add(item);
            }
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = sender as MenuFlyoutItem;
            SelectPayee.Content = selectedItem.Text;
            foreach (var i in allRecipients)
            {
                if (selectedItem.Text == i.PayeeName)
                {
                    AccountNumberTextBox.Text = i.AccountNumber;
                    AccountNumberTextBox.IsReadOnly = true;
                }
            }
        
        }

        private void QuickTransaction_Click(object sender, RoutedEventArgs e)
        {
            AccountNumberTextBox.Text = String.Empty;
            AccountNumberTextBox.IsEnabled = true;
            AccountNumberTextBox.IsReadOnly=false;
            var selectedItem = sender as MenuFlyoutItem;
            SelectPayee.Content = selectedItem.Text;
        }
    }
}
