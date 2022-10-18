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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace NetBankingApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ILoginViewModel LoginViewModel;
        public MainPage()
        {
            this.InitializeComponent();
            var serviceProviderInstance = PresenterService.GetInstance();
            LoginViewModel = serviceProviderInstance.Services.GetService<ILoginViewModel>();
            OnUIEventTrigger();
        }
        //button click event trigger to call view model
        public void OnUIEventTrigger()
        {
            LoginViewModel.ValidateUserInput();
        }
    }
    public interface ILoginViewModel
    {
        void ValidateUserInput();
    }
}
