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
                CurrentSelectedItem = new TransferAmount();
            }
            else if(args.SelectedItem == ViewTransactions)
            {
                CurrentSelectedItem = new TransactionHistory();
            }
            else
            {
                CurrentSelectedItem = new AddPayeeView();
            }
          
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentSelectedItem = new TransferAmount();
        }
    }
}
