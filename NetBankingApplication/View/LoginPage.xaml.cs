using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View.UserControls;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.DataTransfer;
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
        private LoginBaseViewModel _loginViewModel;
        private DispatcherTimer _timer = new DispatcherTimer();
        public static event Action<User> SetUserUpdate;
        public static event Action<Admin> AdminNavigation;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
        public LoginPage()
        {
            this.InitializeComponent();
            var coreWindow = Window.Current.CoreWindow;
            coreWindow.KeyDown += CoreWindow_KeyDown;

            _loginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            //setting login view value for callback
            //_loginViewModel.MainPageNavigationCallback = this;
            _loginViewModel.LoginViewModelCallback = this;
            _loginViewModel.CloseAllWindowsCallback = new AllAccountsPreview();

            _loginViewModel.ResetPasswordResponseValue = String.Empty;
            UserUpdate.LoginViewModelInstance = _loginViewModel;


        }

        private void Verify_Click(object sender, RoutedEventArgs e)
        {

            if (UserId.Text == "")
            {
                _loginViewModel.TextBoxVisibility = Visibility.Visible;
                ResultText.Text = "Enter User ID";
            }
            else
            {
              
                _loginViewModel.ValidateUserInput(UserId.Text, Password.Password);
                UserId.Text = ""; Password.Password = "";
            }
              
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
            _loginViewModel.TextBoxVisibility = Windows.UI.Xaml.Visibility.Collapsed;
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
            // This will realize the deferred element.
            //this.FindName("ResetGrid");
            ResetpasswordUsercontrol.UserId = _loginViewModel.CurrentUser.UserId;
            ResetpasswordUsercontrol.RedirectionAfterResetPassword += ResetpasswordUsercontrol_RedirectionAfterResetPassword;


            LoginContainer.Visibility = Visibility.Collapsed;
            LoginContainerShadow.Visibility = Visibility.Collapsed;
            ResetGrid.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetGrid.ActualWidth / 2 + 450;
            double verticalOffset = Window.Current.Bounds.Height / 2 - ResetGrid.ActualHeight / 2 + 180;
            ResetGrid.HorizontalOffset = horizontalOffset;
            ResetGrid.VerticalOffset = verticalOffset;
        }

        private void ResetpasswordUsercontrol_RedirectionAfterResetPassword()
        {
            SetUser(_loginViewModel.CurrentUser);
        }

        private void CoreWindow_KeyDown(Windows.UI.Core.CoreWindow sender, Windows.UI.Core.KeyEventArgs args)
        {
            if (args.VirtualKey == Windows.System.VirtualKey.Enter)
            {
                if (SubmitLoginDetails.IsEnabled)
                    Verify_Click(sender,new RoutedEventArgs());
            }
        }

        private void copy_Click(object sender, RoutedEventArgs e)
        {
            DataPackage dataPackage = new DataPackage();
            dataPackage.SetText("UserAdmin@1");
            Clipboard.SetContent(dataPackage);
            _timer.Tick += Timer_Tick;
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Start();
        }
        private void Timer_Tick(object sender, object e)
        {
            _timer.Stop();
            CopyFlyout.Hide();
        }
        public void NavigateToAdmin(Admin admin)
        {
            AdminNavigation?.Invoke(admin);
        }
        public void SetUser(User user)
        {
            SetUserUpdate?.Invoke(user);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            ResetpasswordUsercontrol.RedirectionAfterResetPassword -= ResetpasswordUsercontrol_RedirectionAfterResetPassword;

        }
    }
}
