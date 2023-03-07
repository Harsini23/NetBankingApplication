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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page
    {
        private User currentuser;

        public SettingsView()
        {
            this.InitializeComponent();
        }

        public SettingsView(User currentuser)
        {
            this.currentuser = currentuser; this.InitializeComponent();
        }


        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                ResetPassword.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                ResetPassword.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            //ResetPasswordGrid.Visibility = Visibility.Visible;
            ResetPasswordGrid.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2 ;
            double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            ResetPasswordGrid.VerticalOffset = verticalOffset;
        
            //if ()
            //{

            //}else if ()
            //{
            //    ResetPasswordGrid.IsOpen = true;
            //    double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2 + 60;
            //    double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            //    ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            //    ResetPasswordGrid.VerticalOffset = verticalOffset;
            //}
         
        }
    }
}
