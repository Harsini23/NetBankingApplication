using Library.Model;
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
    public sealed partial class CardComponentTemplate : UserControl
    {
        public CardComponent CardComponent { get { return this.DataContext as CardComponent; } }

        public CardComponentTemplate()
        {
            this.InitializeComponent();
            this.DataContextChanged += CardComponentTemplate_DataContextChanged;
        }

        private void CardComponentTemplate_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            Bindings.Update();
        }
    }
}
