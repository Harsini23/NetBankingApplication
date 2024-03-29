﻿using System;
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
    public sealed partial class PayeeTemplate : UserControl
    {
        public PayeeTemplate()
        {
            this.InitializeComponent();
        }
        public string PayeeNameTextBox
        {
            get { return (string)GetValue(PayeeNameProperty); }
            set { SetValue(PayeeNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PayeeName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PayeeNameProperty =
            DependencyProperty.Register("PayeeNameTextBox", typeof(string), typeof(PayeeTemplate), new PropertyMetadata(null));
    
    }
}
