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
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
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
        String ProfilePath;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(Overview), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set { SetValue(UserProperty, value); }
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

        public SettingsView()
        {
            this.InitializeComponent();
            LoginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            LoginViewModel.ClosePopUp = this;
            LoginViewModel.settingsNotification = this;
            updateViewModel = PresenterService.GetInstance().Services.GetService<UpdateUserBaseViewModel>();
            updateViewModel.settingsView = this;
            passwordVerificationViewModel = PresenterService.GetInstance().Services.GetService<PasswordVerificationBaseViewModel>();
            passwordVerificationViewModel.settingsView = this;
            passwordVerificationViewModel.TextBoxVisibility = Visibility.Collapsed;

        }


        private void RevealModeCheckbox_Changed(object sender, RoutedEventArgs e)
        {
            if (revealModeCheckBox.IsChecked == true)
            {
                ResetPasswordPasswordBox.PasswordRevealMode = PasswordRevealMode.Visible;
            }
            else
            {
                ResetPasswordPasswordBox.PasswordRevealMode = PasswordRevealMode.Hidden;
            }
        }

        private void ResetPassword_Click(object sender, RoutedEventArgs e)
        {

            //ResetPasswordGrid.IsOpen = true;
            //double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2;
            //double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            //ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            //ResetPasswordGrid.VerticalOffset = verticalOffset;
            var password = ResetPasswordPasswordBox.Password;
           
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
            else if (EmailId.Text.Trim() == updateViewModel.CurrentUser.EmailId && Name.Text.Trim() == updateViewModel.CurrentUser.UserName && long.Parse(Phonenumber.Text.Trim()) == updateViewModel.CurrentUser.MobileNumber && ProfilePath==null)
            {
                UserProfileError.Visibility = Visibility.Visible;
                UserProfileError.Text = "No changes to be saved;)";
            }
            else
            {
                string ProfilePathUri = "";
                if (ProfilePath != null)
                {
                    ProfilePathUri = ProfilePath;
                }
                else
                {
                    ProfilePathUri = updateViewModel.CurrentUser.ProfilePath;
                }
                var updatedUserValue = new User
                {
                    UserId = updateViewModel.CurrentUser.UserId,
                    EmailId = EmailId.Text.Trim(),
                    UserName = Name.Text.Trim(),
                    MobileNumber = long.Parse(Phonenumber.Text.Trim()),
                    IsBlocked = updateViewModel.CurrentUser.IsBlocked,
                    PAN = updateViewModel.CurrentUser.PAN,
                    ProfilePath = ProfilePathUri
                };

              //  UpdateCurrentPage();
                ProfilePath = null;
                updateViewModel.UpdateUser(updatedUserValue);
                UserProfileError.Visibility = Visibility.Collapsed;
                EditingFields.Visibility = Visibility.Collapsed;
                EditProfile.Visibility = Visibility.Visible;
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
            double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2 + 20;
            double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            ResetPasswordGrid.VerticalOffset = verticalOffset;

        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserProfileError.Visibility = Visibility.Collapsed;
            if(EmailId.Text.Trim() == updateViewModel.CurrentUser.EmailId && Name.Text.Trim() == updateViewModel.CurrentUser.UserName && long.Parse(Phonenumber.Text.Trim()) == updateViewModel.CurrentUser.MobileNumber)
            {
                SaveUserProfile.IsEnabled = false;
            }
            else
            {
                SaveUserProfile.IsEnabled = true;
            }

        }

        private void ResetPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordVerificationViewModel.TextBoxVisibility = Visibility.Collapsed;
        }

        public void RemoveErrors()
        {
            passwordVerificationViewModel.ResponseValue = "";
            ResetPasswordPasswordBox.Password = String.Empty;
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

        private void ResetPasswordGrid_Closed(object sender, object e)
        {
            //clear resetpassword UI data
            ResetPassword myUserControl = (ResetPassword)this.FindName("ResetPasswordComponent");
            myUserControl.ResetUI();
        }

        private async void Initial_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();

            // If a file was selected, save it in the app folder
            if (file != null)
            {
                StorageFolder appFolder = ApplicationData.Current.LocalFolder;
                StorageFile newFile = await file.CopyAsync(appFolder, file.Name, NameCollisionOption.ReplaceExisting);

                // Update the image source with the new file
                var temp= new BitmapImage(new Uri(newFile.Path));
                ProfilePath = temp.UriSource.LocalPath.ToString();

                Initial.ProfilePicture = temp;
            }
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            updateViewModel.CurrentUser = User;
            updateViewModel.CurrentUserInitial = User.UserName.Substring(0, 1)[0];
        }

        private void EditProfile_Click(object sender, RoutedEventArgs e)
        {
            EditingFields.Visibility = Visibility.Visible;
            EditProfile.Visibility = Visibility.Collapsed;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            EditingFields.Visibility = Visibility.Collapsed;
            EditProfile.Visibility = Visibility.Visible;
        }
    }
}
