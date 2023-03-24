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
        User currentUser;
        private bool _newPayeeSuggestionAccepted;
        private string _payeeName;
        private string _accountNumber;
        public PaymentsAndTransfer(User currentUSer)
        {
            this.InitializeComponent();
            this.currentUser = currentUSer;
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
            //FrameNavigationOptions navOptions = new FrameNavigationOptions();
            //navOptions.TransitionInfoOverride = args.RecommendedNavigationTransitionInfo;
            //if (sender.PaneDisplayMode == NavigationViewPaneDisplayMode.Top)
            //{
            //    navOptions.IsNavigationStackEnabled = false;
            //}

            if (args.SelectedItem == Transfer)
            {
                    var transferAmount= new TransferAmount(currentUser.UserId);
                CurrentSelectedItem = transferAmount;
                transferAmount.RaiseNotification += TransferAmount_RaiseNotification;
                transferAmount.SendPayee += TransferAmount_SendPayee;

            }
            else if(args.SelectedItem == ViewTransactions)
            {
                CurrentSelectedItem = new TransactionHistory(currentUser.UserId);
            }
            else if(args.SelectedItem == AddPayee)
            {
                AddPayeeView addPayeeView;
                if (_newPayeeSuggestionAccepted)
                {
                    _newPayeeSuggestionAccepted = false;
                    addPayeeView = new AddPayeeView(currentUser.UserId,_payeeName,_accountNumber);
                }
                else
                {
                    addPayeeView = new AddPayeeView(currentUser.UserId);
                }
                CurrentSelectedItem = addPayeeView;
                addPayeeView.RaiseNotification += TransferAmount_RaiseNotification;
            }
            else
            {
                var viewAndEditPayee = new ViewAndEditPayee(currentUser.UserId);
                CurrentSelectedItem = viewAndEditPayee;
                viewAndEditPayee.RaiseNotification+= TransferAmount_RaiseNotification;
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
            CurrentSelectedItem = new TransferAmount(currentUser.UserId);
            PaymentsAndTransferNavigation.SelectedItem = Transfer;
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }
    }
}
