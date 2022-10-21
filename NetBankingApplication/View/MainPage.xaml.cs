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

namespace NetBankingApplication
{
    public sealed partial class MainPage : Page
    {
        private ILoginViewModel LoginViewModel;
        public MainPage()
        {
          this.InitializeComponent();
          
          var serviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = /*new LoginBaseViewModel();*/serviceProviderInstance.Services.GetService<ILoginViewModel>();
          //  this.DataContext = new NotifyPropertyBase();

            // this.DataContext = LoginViewModel;
            // DataContext = ActivatorUtilities.GetServiceOrCreateInstance(serviceProviderInstance.Services, typeof(LoginViewModel));
        }
        private void Verify_Click(object sender, RoutedEventArgs e)
        {
            LoginViewModel.ValidateUserInput(UserId.Text,Password.Text);
        }
    }
    public interface ILoginViewModel:INotifyPropertyChanged
    {
        void ValidateUserInput(string userId,string password);
         string LoginResponseValue { get; set; }
    }
}
