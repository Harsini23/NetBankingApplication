using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page, ILoginViewModel
    {
        private LoginBaseViewModel LoginViewModel;
        public string NewPassword;
        PresenterService LoginVMserviceProviderInstance;

        public LoginPage()
        {
            this.InitializeComponent();
            LoginVMserviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = LoginVMserviceProviderInstance.Services.GetService<LoginBaseViewModel>();
            //setting login view value for callback
            LoginViewModel.LoginViewModelCallback = this;
        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {
         
            LoginViewModel.ValidateUserInput(UserId.Text, Password.Password);
            UserId.Text = ""; Password.Password = "";
            // should be triggered only when user name pass is verified and for new first time users
            //SwitchToResetPasswordContainer();
        }

      
        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                Password.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                Password.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password.ToString();
            if (password.Length > 8 && password.Length < 14 && password.Any(char.IsLower) && password.Any(char.IsUpper) && (!password.Contains(" ")) && CheckForSpecialCharacter(password))
            {
                SubmitLoginDetails.IsEnabled = true;
            }
            else
            {
                SubmitLoginDetails.IsEnabled = false;
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

        public void SwitchToResetPasswordContainer()
        {
            Debug.WriteLine(ResetPasswordcontainer.Visibility.ToString());
            LoginContainer.Visibility = Visibility.Collapsed;
            ResetPasswordcontainer.Visibility = Visibility.Visible;
            Debug.WriteLine(ResetPasswordcontainer.Visibility.ToString());
          
        }
     
        private void RevealModeCheckbox_ChangedReset(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBoxPassword.IsChecked == true)
            {
                PasswordReset.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                PasswordReset.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void RevealModeCheckbox_Changed_RePassword(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox_RePassword.IsChecked == true)
            {
                RePasswordReset.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                RePasswordReset.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void Password_PasswordChangedReset(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password.ToString();
            if (password.Length > 8 && password.Length < 14 && password.Any(char.IsLower) && password.Any(char.IsUpper) && (!password.Contains(" ")) && CheckForSpecialCharacter(password))
            {
                RePasswordReset.IsEnabled = true;
                NewPassword = password;
            }
            else
            {
                RePasswordReset.IsEnabled = false;
            }
        }

        private void Password_Verify(object sender, RoutedEventArgs e)
        {
            var passwordBox = (PasswordBox)sender;
            var password = passwordBox.Password.ToString();
            if (password == NewPassword)
            {
                ReSetPassword.IsEnabled = true;
            }
            else
            {
                ReSetPassword.IsEnabled = false;
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel.ResetPassword(RePasswordReset.Password);
        }

      
    }
}
