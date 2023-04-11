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

    public sealed partial class FDNavigationOverview : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(FDNavigationOverview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
        }
   

        private UserControl _currentSelectedItem;

        public event PropertyChangedEventHandler PropertyChanged;

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
        public FDNavigationOverview()
        {
            this.InitializeComponent();
        }

        private void PaymentsAndTransferNavigation_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {

            if (args.SelectedItem == OpenFD)
            {
                var fdAccount = new FDAccountView();
                fdAccount.User = User;
                CurrentSelectedItem = fdAccount;
            }
            else if (args.SelectedItem == ViewFDRates )
            {
            
            }
            else
            {
              
            }

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            PaymentsAndTransferNavigation.SelectedItem = OpenFD;

        }
    }
}
