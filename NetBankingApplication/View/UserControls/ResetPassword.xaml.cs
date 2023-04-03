using Microsoft.Extensions.DependencyInjection;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace NetBankingApplication.View.UserControls
{
    public sealed partial class ResetPassword : UserControl
    {
        private LoginBaseViewModel LoginViewModel;
        public string NewPassword;
        public bool redirect { get; set; }
        public ResetPassword()
        {
            this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            //setting login view value for callback
            //LoginViewModel.LoginViewModelCallback = this;
            LoginViewModel.CloseAllWindowsCallback = new AllAccountsPreview();
            //LoginViewModel.ClosePopUp = new SettingsView();
            LoginViewModel.ResetPasswordResponseValue = String.Empty;
        }

        //private void RevealModeCheckbox_ChangedReset(object sender, RoutedEventArgs e)
        //{
        //    if (revealModeCheckBoxPassword.IsChecked == true)
        //    {
        //        PasswordReset.PasswordRevealMode = PasswordRevealMode.Visible;
        //    }
        //    else
        //    {
        //        PasswordReset.PasswordRevealMode = PasswordRevealMode.Hidden;
        //    }
        //}

        private void RevealModeCheckbox_Changed_RePassword(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox_RePassword.IsChecked == true)
            {
                PasswordReset.PasswordRevealMode = PasswordRevealMode.Visible;
                RePasswordReset.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                PasswordReset.PasswordRevealMode = PasswordRevealMode.Hidden;
                RePasswordReset.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void Password_PasswordChangedReset(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password.ToString();
            if (password.Length > 8 && password.Length < 128 && password.Any(char.IsLower) && password.Any(char.IsUpper) && (!password.Contains(" ")) && CheckForSpecialCharacter(password))
            {
                RePasswordReset.IsEnabled = true;
                NewPassword = password;
            }
            else
            {
                RePasswordReset.IsEnabled = false;
            }
        }
        private bool CheckForSpecialCharacter(string passwd)
        {
            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            foreach (char ch in specialChArray)
            {
                if (passwd.Contains(ch))
                    return true;
            }
            return false;
        }

        private void Password_Verify(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password.ToString();
            if (password == NewPassword)
            {
                ReSetPassword.IsEnabled = true;
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                ReSetPassword.IsEnabled = false;
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
          if(RePasswordReset.Password== PasswordReset.Password)
            {
                LoginViewModel.Redirect = redirect;
                LoginViewModel.ResetPassword(RePasswordReset.Password);
                PasswordReset.Password = "";
                RePasswordReset.Password = "";
                ErrorTextBlock.Visibility = Visibility.Collapsed;
            }
            else
            {
                ErrorTextBlock.Visibility = Visibility.Visible;
            }
        }

        public void ResetUI()
        {
            PasswordReset.Password = String.Empty;
            //revealModeCheckBoxPassword.IsChecked = false;
            RePasswordReset.Password = String.Empty;
            revealModeCheckBox_RePassword.IsChecked = false;
            ErrorTextBlock.Visibility = Visibility.Collapsed;
        }

        
    }
}
