using Library.Model;
using Microsoft.Extensions.DependencyInjection;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page, IClosePopUp, ISettingsView, INotifyPropertyChanged, IUserUpdateNotification, IChangePasswordNotification
    {
       // private User currentuser;
       // private char UserInitial;
        private LoginBaseViewModel LoginViewModel;
        private UpdateUserBaseViewModel updateViewModel;
        private PasswordVerificationBaseViewModel passwordVerificationViewModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public SettingsView()
        {
            this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            LoginViewModel.settingsNotification = this;

            LoginViewModel.ClosePopUp = this;
            passwordVerificationViewModel.TextBoxVisibility = Visibility.Collapsed;
        }

        private string _notificationMessage;
        public string NotificationMessage
        {
            get { return _notificationMessage; }
            set
            {
                _notificationMessage = value;
                NotifyPropertyChanged();
            }
        }
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SettingsView(User currentuser)
        {
            //this.currentuser = currentuser; 
            this.InitializeComponent();
          //  this.UserInitial = currentuser.UserName.Substring(0, 1)[0];
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            LoginViewModel.ClosePopUp = this;

            updateViewModel = PresenterService.GetInstance().Services.GetService<UpdateUserBaseViewModel>();
            updateViewModel.CurrentUser = currentuser;
            updateViewModel.settingsView = this;
            updateViewModel.CurrentUserInitial= currentuser.UserName.Substring(0, 1)[0];
            passwordVerificationViewModel = PresenterService.GetInstance().Services.GetService<PasswordVerificationBaseViewModel>();
            passwordVerificationViewModel.settingsView = this;
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

            //ResetPasswordGrid.IsOpen = true;
            //double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2;
            //double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            //ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            //ResetPasswordGrid.VerticalOffset = verticalOffset;
            var password = ResetPassword.Password;
           
            if (string.IsNullOrEmpty(password))
            {
                passwordVerificationViewModel.TextBoxVisibility = Visibility.Visible;
                passwordVerificationViewModel.ResponseValue="Kindly enter old Password";
            }
            else
            {
                //send and check if old password matches
                passwordVerificationViewModel.CheckPassword(updateViewModel.CurrentUser.UserId,password);
            }
         

        }

        public void closePopup()
        {
            ResetPasswordGrid.IsOpen = false;
            InAppNotification.Show(LoginViewModel.ResetPasswordResponseValue, 3000);
            NotificationMessage = LoginViewModel.ResetPasswordResponseValue;
            //AcknowledgementDialogue.ShowAsync();
            //DispatcherTimer timer = new DispatcherTimer();
            //timer.Interval = TimeSpan.FromSeconds(1);
            //timer.Tick += (s, args) =>
            //{
            //    AcknowledgementDialogue.Hide();
            //    timer.Stop();
            //};
            //timer.Start();
        }
        private void TextBox_OnBeforeTextChanging(TextBox sender,
                                       TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }


        private void SaveUserProfile_Click(object sender, RoutedEventArgs e)
        {
            var phonenumber = Phonenumber.Text.Trim();
            if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Phonenumber.Text) || string.IsNullOrWhiteSpace(EmailId.Text))
            {
                UserProfileError.Visibility = Visibility.Visible;
                UserProfileError.Text = "Fields cannot be empty!";
            }
            else if (phonenumber.Length != 10)
            {
                UserProfileError.Visibility = Visibility.Visible;
                UserProfileError.Text = "Check your phone number;)";
            }
            else if (!EmailId.Text.Contains('@') || !EmailId.Text.Contains('.') || !EmailId.Text.Contains("com"))
            {
                UserProfileError.Visibility = Visibility.Visible;
                UserProfileError.Text = "Check your emaild id;)";
            }
            else if (EmailId.Text.Trim() == updateViewModel.CurrentUser.EmailId && Name.Text.Trim() == updateViewModel.CurrentUser.UserName && long.Parse(Phonenumber.Text.Trim()) == updateViewModel.CurrentUser.MobileNumber)
            {
                UserProfileError.Visibility = Visibility.Visible;
                UserProfileError.Text = "No changes to be saved;)";
            }
            else
            {
                var updatedUserValue = new User
                {
                    UserId = updateViewModel.CurrentUser.UserId,
                    EmailId = EmailId.Text.Trim(),
                    UserName = Name.Text.Trim(),
                    MobileNumber = long.Parse(Phonenumber.Text.Trim()),
                    IsBlocked = updateViewModel.CurrentUser.IsBlocked,
                    PAN = updateViewModel.CurrentUser.PAN
                };

                UpdateCurrentPage();

                updateViewModel.UpdateUser(updatedUserValue);
                UserProfileError.Visibility = Visibility.Collapsed;
                //AcknowledgementDialogue.ShowAsync();
                //DispatcherTimer timer = new DispatcherTimer();
                //timer.Interval = TimeSpan.FromSeconds(1);
                //timer.Tick += (s, args) =>
                //{
                //    AcknowledgementDialogue.Hide();
                //    timer.Stop();
                //};
                //timer.Start();
            }
        }
        private void UpdateCurrentPage()
        {
            DisplayEmailId.Text = EmailId.Text.Trim();
            Username.Text = Name.Text.Trim();
            Initial.Initials = Name.Text.Trim()[0].ToString();
        }

        public void TriggerResetPasswordPopup()
        {
            ResetPasswordGrid.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2 + 60;
            double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            ResetPasswordGrid.VerticalOffset = verticalOffset;

        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserProfileError.Visibility = Visibility.Collapsed;

        }

        private void ResetPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordVerificationViewModel.TextBoxVisibility = Visibility.Collapsed;
        }

        public void RemoveErrors()
        {
            passwordVerificationViewModel.ResponseValue = "";
            ResetPassword.Password = String.Empty;
        }

       

        public void UpdateUserNotification()
        {
            InAppNotification.Show(passwordVerificationViewModel.ResponseValue, 3000);
            NotificationMessage = updateViewModel.ResponseValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }

        public void ChangePasswordNotification()
        {
            InAppNotification.Show(LoginViewModel.ResetPasswordResponseValue, 3000);
            NotificationMessage = updateViewModel.ResponseValue;
        }
    }
}
