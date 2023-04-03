using Library.Model;
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
    public sealed partial class PaymentsAndTransfer : UserControl, INotifyPropertyChanged
    {
        //CurrentSelectedItem
        private bool _newPayeeSuggestionAccepted;
        private string _payeeName;
        private string _accountNumber;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
        public PaymentsAndTransfer()
        {
            this.InitializeComponent();
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

        private string _notificationMessage;
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set
            {
                _notificationMessage = value;
                NotifyPropertyChanged();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void PaymentsAndTransferNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            if (args.SelectedItem == Transfer)
            {
                var transferAmount = new TransferAmount();
                transferAmount.User = User;
                CurrentSelectedItem = transferAmount;
                transferAmount.RaiseNotification += TransferAmount_RaiseNotification;
                transferAmount.SendPayee += TransferAmount_SendPayee;

            }
            else if (args.SelectedItem == ViewTransactions)
            {
                TransactionHistory transactionHistory = new TransactionHistory();
                transactionHistory.User = User;
                CurrentSelectedItem = transactionHistory;
            }
            else if (args.SelectedItem == AddPayee)
            {
                AddPayeeView addPayeeView= new AddPayeeView();
                addPayeeView.User = User;
                if (_newPayeeSuggestionAccepted)
                {
                    _newPayeeSuggestionAccepted = false;
                    addPayeeView.PassedPayeeName = _payeeName;
                    addPayeeView.PassedAccountNumber=_accountNumber;
                }
                CurrentSelectedItem = addPayeeView;
                addPayeeView.RaiseNotification += TransferAmount_RaiseNotification;
            }
            else
            {
                var viewAndEditPayee = new ViewAndEditPayee();
                viewAndEditPayee.User = User;
                CurrentSelectedItem = viewAndEditPayee;
                viewAndEditPayee.RaiseNotification += TransferAmount_RaiseNotification;
            }

        }

        private void TransferAmount_SendPayee(string arg1, string arg2)
        {
            _newPayeeSuggestionAccepted = true;
            _payeeName = arg1;
            _accountNumber = arg2;
            PaymentsAndTransferNavigation.SelectedItem = PaymentsAndTransferNavigation.MenuItems[2];
            // SwitchToAddPayee(arg1, arg2);
        }

        //public void SwitchToAddPayee(string payeeName,string AccountNumber)
        //{
        //    //var addPayeeView = new AddPayeeView(currentUser.UserId,payeeName, AccountNumber);
        //    //CurrentSelectedItem = addPayeeView;
        //    //addPayeeView.RaiseNotification += TransferAmount_RaiseNotification;

        //    //PaymentsAndTransferNavigation.SelectedItem= CurrentSelectedItem;
        //    PaymentsAndTransferNavigation.SelectedItem = PaymentsAndTransferNavigation.MenuItems[2];


        //}
        private void TransferAmount_RaiseNotification(string obj)
        {
            InAppNotification.Show(obj, 3000);
            NotificationMessage = obj;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentsAndTransferNavigation.SelectedItem = Transfer;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }
    }
}
