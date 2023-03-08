using Library.Model;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace NetBankingApplication.View.UserControls
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsView : Page, IClosePopUp
    {
        private User currentuser;
        private LoginBaseViewModel LoginViewModel;
        private UpdateUserBaseViewModel updateViewModel;
        public SettingsView()
        {
            this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            LoginViewModel.ClosePopUp = this;
            ErrorMessage.Visibility = Visibility.Collapsed;
        }


        public SettingsView(User currentuser)
        {
            this.currentuser = currentuser; this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            LoginViewModel.ClosePopUp = this;

            updateViewModel = PresenterService.GetInstance().Services.GetService<UpdateUserBaseViewModel>();
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

            if (string.IsNullOrEmpty(ResetPassword.Password))
            {
                ErrorMessage.Visibility = Visibility.Visible;
                ErrorMessage.Text = "Kindly enter old Password";
            }
            else 
            {
                //send and check if old password matches


                ResetPasswordGrid.IsOpen = true;
                double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2 + 60;
                double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
                ResetPasswordGrid.HorizontalOffset = horizontalOffset;
                ResetPasswordGrid.VerticalOffset = verticalOffset;
            }

        }

        public void closePopup()
        {
            ResetPasswordGrid.IsOpen = false;
            AcknowledgementDialogue.ShowAsync();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += (s, args) =>
            {
                AcknowledgementDialogue.Hide();
                timer.Stop();
            };
            timer.Start();
        }
        private void TextBox_OnBeforeTextChanging(TextBox sender,
                                       TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }


        private void SaveUserProfile_Click(object sender, RoutedEventArgs e)
        {
            if(Name.Text!=null && Phonenumber.Text!=null && EmailId.Text != null && Phonenumber.Text.Length==10 && EmailId.Text.Contains('@') && EmailId.Text.Contains('.') && EmailId.Text.Contains("com"))
            {
                var updatedUserValue = new User
                {
                    UserId=currentuser.UserId,
                    EmailId = EmailId.Text,
                    UserName = Name.Text,
                    MobileNumber = long.Parse(Phonenumber.Text),
                    IsBlocked=currentuser.IsBlocked,
                    PAN=currentuser.PAN
                };

                updateViewModel.UpdateUser(updatedUserValue);
            }
        }
    }
}
