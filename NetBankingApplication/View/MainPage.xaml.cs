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
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
using System.ComponentModel;
using System.Diagnostics;

namespace NetBankingApplication
{
    public sealed partial class MainPage : Page
    {
       
        public MainPage()
        {
            Debug.WriteLine(Application.Current.Resources);
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            LoadContentFrame.Navigate(typeof(LoginPage));
        }
    }

  

}
