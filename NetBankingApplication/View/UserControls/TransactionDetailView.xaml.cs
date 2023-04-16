using Library.Model;
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
    public sealed partial class TransactionDetailView : UserControl
    {
        public AccountTransactionBObj transaction;
        public TransactionDetailView()
        {
            this.InitializeComponent();
            this.DataContext = new AccountTransactionBObj();
            this.DataContextChanged += TransactionDetailView_DataContextChanged;
        }

        private void TransactionDetailView_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
           if(this.DataContext != null)
            {
               this.transaction = (AccountTransactionBObj)this.DataContext;
                Bindings.Update();
            }
        }
    }
}
