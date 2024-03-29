﻿using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.UI.Controls;
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
using Windows.Media.Capture;
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
    public sealed partial class SettingsView : Page, ISettingsView, INotifyPropertyChanged, IUserUpdateNotification
    {
        // private User currentuser;
        // private char UserInitial;
        private LoginBaseViewModel _loginViewModel;
        private UpdateUserBaseViewModel _updateViewModel;

        private PasswordVerificationBaseViewModel _passwordVerificationViewModel;
        ResetPassword myUserControl;


        public event PropertyChangedEventHandler PropertyChanged;
        String ProfilePath;
        public static readonly DependencyProperty UserProperty = DependencyProperty.Register(nameof(User), typeof(User), typeof(SettingsView), new PropertyMetadata(null));
        public User User
        {
            get { return (User)GetValue(UserProperty); }
            set
            {
                SetValue(UserProperty, value);
                if (User.ProfilePath != null)
                {
                    IsProfileEnabled = true;
                }
            }
        }
        private bool _isProfileEnabled;
        public bool IsProfileEnabled
        {
            get { return _isProfileEnabled; }
            set
            {
                _isProfileEnabled = value;
                NotifyPropertyChanged();
                if (value == true)
                {
                    ProfileUpdateIcon = "";

                }
                else
                {
                    ProfileUpdateIcon = "";
                }
            }
        }
        private string _profileUpdateIcon;
        public string ProfileUpdateIcon
        {
            get { return _profileUpdateIcon; }
            set
            {
                _profileUpdateIcon = value;
                NotifyPropertyChanged();
            }
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
            //_loginViewModel = PresenterService.GetInstance().Services.GetService<LoginBaseViewModel>();
            //_loginViewModel.ClosePopUp = this;
            //_loginViewModel.settingsNotification = this;
            _updateViewModel = PresenterService.GetInstance().Services.GetService<UpdateUserBaseViewModel>();
            _updateViewModel.settingsView = this;
            _passwordVerificationViewModel = PresenterService.GetInstance().Services.GetService<PasswordVerificationBaseViewModel>();
            _passwordVerificationViewModel.settingsView = this;
            _passwordVerificationViewModel.TextBoxVisibility = Visibility.Collapsed;
            myUserControl = (ResetPassword)this.FindName("ResetPasswordComponent");


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
                _passwordVerificationViewModel.TextBoxVisibility = Visibility.Visible;
                _passwordVerificationViewModel.ResponseValue = "Kindly enter old Password";
            }
            else
            {
                //send and check if old password matches
                _passwordVerificationViewModel.CheckPassword(_updateViewModel.CurrentUser.UserId, password);
            }


        }

        //public void closePopup()
        //{
        //    ResetPasswordGrid.IsOpen = false;
        //    InAppNotification.Show(_loginViewModel.ResetPasswordResponseValue, 3000);
        //    NotificationMessage = _loginViewModel.ResetPasswordResponseValue;
        //    //AcknowledgementDialogue.ShowAsync();
        //    //DispatcherTimer timer = new DispatcherTimer();
        //    //timer.Interval = TimeSpan.FromSeconds(1);
        //    //timer.Tick += (s, args) =>
        //    //{
        //    //    AcknowledgementDialogue.Hide();
        //    //    timer.Stop();
        //    //};
        //    //timer.Start();
        //}
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
            else if (EmailId.Text.Trim() == _updateViewModel.CurrentUser.EmailId && Name.Text.Trim() == _updateViewModel.CurrentUser.UserName && long.Parse(Phonenumber.Text.Trim()) == _updateViewModel.CurrentUser.MobileNumber && ProfilePath == null)
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
                    ProfilePathUri = _updateViewModel.CurrentUser.ProfilePath;
                }
                var updatedUserValue = new User
                {
                    UserId = _updateViewModel.CurrentUser.UserId,
                    EmailId = EmailId.Text.Trim(),
                    UserName = Name.Text.Trim(),
                    MobileNumber = long.Parse(Phonenumber.Text.Trim()),
                    IsBlocked = _updateViewModel.CurrentUser.IsBlocked,
                    PAN = _updateViewModel.CurrentUser.PAN,
                    ProfilePath = ProfilePathUri
                };

                //  UpdateCurrentPage();
                ProfilePath = null;
                _updateViewModel.UpdateUser(updatedUserValue);
                UserProfileError.Visibility = Visibility.Collapsed;
                EditingFields.Visibility = Visibility.Collapsed;
                EditProfile.Visibility = Visibility.Visible;
                EmailId.Text = _updateViewModel.CurrentUser.EmailId;
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

            myUserControl.RedirectionAfterResetPassword += ResetpasswordUsercontrol_RedirectionAfterResetPassword;

            ResetPasswordGrid.IsOpen = true;
            double horizontalOffset = Window.Current.Bounds.Width / 2 - ResetPasswordGrid.ActualWidth / 2 + 20;
            double verticalOffset = Window.Current.Bounds.Height / 2 - ResetPasswordGrid.ActualHeight / 2;
            ResetPasswordGrid.HorizontalOffset = horizontalOffset;
            ResetPasswordGrid.VerticalOffset = verticalOffset;

        }
        private void ResetpasswordUsercontrol_RedirectionAfterResetPassword()
        {
            NavigateAfterReset("Succesfully changed password");
        }

        private void Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserProfileError.Visibility = Visibility.Collapsed;
            if (EmailId.Text.Trim() == _updateViewModel.CurrentUser.EmailId && Name.Text.Trim() == _updateViewModel.CurrentUser.UserName && long.Parse(Phonenumber.Text.Trim()) == _updateViewModel.CurrentUser.MobileNumber)
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
            _passwordVerificationViewModel.TextBoxVisibility = Visibility.Collapsed;
        }

        public void RemoveErrors()
        {
            _passwordVerificationViewModel.ResponseValue = "";
            ResetPasswordPasswordBox.Password = String.Empty;
        }



        public void UpdateUserNotification()
        {
            InAppNotification.Show(_passwordVerificationViewModel.ResponseValue, 3000);
            NotificationMessage = _updateViewModel.ResponseValue;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InAppNotification.Dismiss();
        }

        //public void ChangePasswordNotification()
        //{
        //    InAppNotification.Show(_loginViewModel.ResetPasswordResponseValue, 3000);
        //    NotificationMessage = _loginViewModel.ResetPasswordResponseValue;
        //}

        private void ResetPasswordGrid_Closed(object sender, object e)
        {
            //clear resetpassword UI data
            //ResetPassword myUserControl = (ResetPassword)this.FindName("ResetPasswordComponent");
            myUserControl.ResetUI();
        }

        private async void Initial_Tapped(object sender, TappedRoutedEventArgs e)
        {
            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;
            captureUI.PhotoSettings.CroppedSizeInPixels = new Size(200, 200);

            StorageFile photo = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (photo != null)
            {
                // User cancelled photo capture
                //return;
                StorageFolder destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("ProfilePhotoFolder", CreationCollisionOption.OpenIfExists);

                StorageFile newFile = await photo.CopyAsync(destinationFolder, "ProfilePhoto.jpg", NameCollisionOption.ReplaceExisting);
                await photo.DeleteAsync();


                var temp = new BitmapImage(new Uri(newFile.Path));
                ProfilePath = temp.UriSource.LocalPath.ToString();
                Initial.ProfilePicture = temp;

                //update db
                var updatedUserValue = new User
                {
                    UserId = _updateViewModel.CurrentUser.UserId,
                    EmailId = _updateViewModel.CurrentUser.EmailId,
                    UserName = _updateViewModel.CurrentUser.UserName,
                    MobileNumber = _updateViewModel.CurrentUser.MobileNumber,
                    IsBlocked = _updateViewModel.CurrentUser.IsBlocked,
                    PAN = _updateViewModel.CurrentUser.PAN,
                    ProfilePath = ProfilePath
                };

                //  UpdateCurrentPage();

                _updateViewModel.UpdateUser(updatedUserValue);
                ProfilePath = null;

            }
            return;
            // SetProfile();
        }
        private async void SetProfile()
        {
            var picker = new FileOpenPicker();
            picker.ViewMode = PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");

            StorageFile file = await picker.PickSingleFileAsync();

            //If a file was selected, save it in the app folder
            if (file != null)
            {
                IsProfileEnabled = true;
                StorageFolder appFolder = ApplicationData.Current.LocalFolder;
                StorageFile newFile = await file.CopyAsync(appFolder, file.Name, NameCollisionOption.ReplaceExisting);

                // Update the image source with the new file

                var temp = new BitmapImage(new Uri(newFile.Path));
                ProfilePath = temp.UriSource.LocalPath.ToString();
                Initial.ProfilePicture = temp;

                //update db
                var updatedUserValue = new User
                {
                    UserId = _updateViewModel.CurrentUser.UserId,
                    EmailId = _updateViewModel.CurrentUser.EmailId,
                    UserName = _updateViewModel.CurrentUser.UserName,
                    MobileNumber = _updateViewModel.CurrentUser.MobileNumber,
                    IsBlocked = _updateViewModel.CurrentUser.IsBlocked,
                    PAN = _updateViewModel.CurrentUser.PAN,
                    ProfilePath = ProfilePath
                };

                //  UpdateCurrentPage();

                _updateViewModel.UpdateUser(updatedUserValue);
                ProfilePath = null;

            }
            //prompt to save profile or reset back only if file !=null
            //save and show notification
        }

        private void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            _updateViewModel.CurrentUser = User;
            _updateViewModel.CurrentUserInitial = User.UserName.Substring(0, 1)[0];
            if (_updateViewModel.CurrentUser.ProfilePath != null)
            {
                IsProfileEnabled = true;
            }
            else
            {
                IsProfileEnabled = false;
            }
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
            EmailId.Text = _updateViewModel.CurrentUser.EmailId;
            Name.Text = _updateViewModel.CurrentUser.UserName;
            Phonenumber.Text = _updateViewModel.CurrentUser.MobileNumber.ToString();
        }

        private void EditIcon_Click(object sender, RoutedEventArgs e)
        {
            if (_updateViewModel.CurrentUser.ProfilePath != null)
            {
                //ask to upload new or remove
                var menuFlyout = new MenuFlyout()
                {
                    Placement = FlyoutPlacementMode.Bottom,
                    LightDismissOverlayMode = LightDismissOverlayMode.Auto,
                    MenuFlyoutPresenterStyle = (Style)this.Resources["MenuDropDownContentStyle"],

                };

                // Create the menu flyout items
                var menuItem1 = new MenuFlyoutItem { Text = "Set new" };
                var menuItem2 = new MenuFlyoutItem { Text = "Remove" };
                menuItem1.Click += MenuItem1_Click;
                menuItem2.Click += MenuItem2_Click;

                // Add the menu flyout items to the menu flyout
                menuFlyout.Items.Add(menuItem1);
                menuFlyout.Items.Add(menuItem2);

                // Attach the menu flyout to the button
                menuFlyout.ShowAt(EditIcon);
                IsProfileEnabled = true;
            }
            else
            {
                SetProfile();
                IsProfileEnabled = false;
                //    IsProfileEnabled = true;
            }
            // SetProfile();
        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            //remove and show notification
            var updatedUserValue = new User
            {
                UserId = _updateViewModel.CurrentUser.UserId,
                EmailId = _updateViewModel.CurrentUser.EmailId,
                UserName = _updateViewModel.CurrentUser.UserName,
                MobileNumber = _updateViewModel.CurrentUser.MobileNumber,
                IsBlocked = _updateViewModel.CurrentUser.IsBlocked,
                PAN = _updateViewModel.CurrentUser.PAN,
                ProfilePath = null
            };
            IsProfileEnabled = false;
            //  UpdateCurrentPage();
            ProfilePath = null;
            _updateViewModel.UpdateUser(updatedUserValue);
        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            SetProfile();
            IsProfileEnabled = true;
        }

        public void NavigateAfterReset(string response)
        {
            ResetPasswordGrid.IsOpen = false;
            InAppNotification.Show(response, 3000);
            NotificationMessage = response;
        }

        private void Initial_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PersonPictureGrid.Opacity = 1;
            if (_updateViewModel.CurrentUser.ProfilePath is string imagePath)
            {
                BitmapImage bitmapImage = new BitmapImage();
                //bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
                if (imagePath != null || string.IsNullOrWhiteSpace(imagePath))
                {
                    bitmapImage = new BitmapImage(new Uri(imagePath));
                }
                Initial.ProfilePicture = bitmapImage;
            }
            else
            {
                Initial.ProfilePicture = null;
            }
        }

        private void Initial_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            PersonPictureGrid.Opacity = 0.3;
            Initial.ProfilePicture = new BitmapImage(new Uri("ms-appx:///Assets/cam.jfif"));

        }

        //private void Initial_PointerEntered(object sender, PointerRoutedEventArgs e)
        //{
        //    Initial.ProfilePicture = new BitmapImage(new Uri("ms-appx:///Assets/bgfinal.jpg"));
        //}

        //private void Initial_PointerExited(object sender, PointerRoutedEventArgs e)
        //{
        //    if (updateViewModel.CurrentUser.ProfilePath is string imagePath)
        //    {
        //        BitmapImage bitmapImage = new BitmapImage();
        //        //bitmapImage.UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute);
        //        if (imagePath != null || string.IsNullOrWhiteSpace(imagePath))
        //        {
        //            bitmapImage = new BitmapImage(new Uri(imagePath));
        //        }
        //        Initial.ProfilePicture = bitmapImage;
        //    }

        //}
    }
}
