using Library.Model;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public sealed partial class DetailedTransactionView : UserControl//,INotifyPropertyChanged
    {

        private AccountTransactionBObj _selectedTransaction = new AccountTransactionBObj();

        public event PropertyChangedEventHandler PropertyChanged;

        public AccountTransactionBObj SelectedTransaction
        {
            get {
                // return this.DataContext as AccountTransactionBObj;
                Debug.WriteLine(this.DataContext);
               // Bindings.Update();
                return _selectedTransaction;
            }
            set
            {
                this.DataContext = value;
                _selectedTransaction= value;
               // Bindings.Update();
                //OnPropertyChanged(nameof(SelectedTransaction));
            }
        }
        public DetailedTransactionView()
        {
            this.InitializeComponent();
            this.DataContext = _selectedTransaction;
           // this.DataContextChanged += (s, e) => Bindings.Update();
        }


    }

    public class INotifyView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
