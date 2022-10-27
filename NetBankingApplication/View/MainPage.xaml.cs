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
        private LoginBaseViewModel LoginViewModel;
        public MainPage()
        {
            Debug.WriteLine(Application.Current.Resources);
            this.InitializeComponent();
            
          var serviceProviderInstance = PresenterService.GetInstance();
          LoginViewModel = serviceProviderInstance.Services.GetService<LoginBaseViewModel>();

        }
        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel.ValidateUserInput(UserId.Text,Password.Password);
            UserId.Text = "";Password.Password = "";
        }

        private void SubmitUserIdCredential_Click(object sender, RoutedEventArgs e)
        {

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
            if(password.Length > 8 && password.Length < 14 && password.Any(char.IsLower) && password.Any(char.IsUpper) && (! password.Contains(" ") ) && CheckForSpecialCharacter(password))
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }

  

}
