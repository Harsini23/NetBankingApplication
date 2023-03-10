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

            }
            else if(args.SelectedItem == ViewTransactions)
            {
                CurrentSelectedItem = new TransactionHistory(currentUser.UserId);
            }
            else if(args.SelectedItem == AddPayee)
            {
               var addPayeeView  = new AddPayeeView(currentUser.UserId);
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

        private void TransferAmount_RaiseNotification(string obj)
        {
            ExampleInAppNotification.Show(obj, 3000);
            NotificationMessage = obj;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentSelectedItem = new TransferAmount(currentUser.UserId);
            PaymentsAndTransferNavigation.SelectedItem = Transfer;
          
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ExampleInAppNotification.Dismiss();
        }
    }
}
