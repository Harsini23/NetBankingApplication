using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
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
    public sealed partial class AllAccountsPreview : UserControl
    {
        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;

        PresenterService GetAllAccountsVMserviceProviderInstance;
        private string userId;

     
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //GetAllAccountsViewModel.GetAllAccounts(userId);
            //Bindings.Update();
            //DetailedAccountOverview detailedAccountOverview = new DetailedAccountOverview("", "");
            //CurrentSelectedModule = detailedAccountOverview;
        }
        public AllAccountsPreview(string userId)
        {
            this.InitializeComponent();

            this.userId = userId;
            GetAllAccountsVMserviceProviderInstance = PresenterService.GetInstance();
            GetAllAccountsViewModel = GetAllAccountsVMserviceProviderInstance.Services.GetService<GetAllAccountsBaseViewModel>();
            GetAllAccountsViewModel.GetAllAccounts(userId);
            Bindings.Update();
        }


        private async void DisplayFullAccountDetails_Click_1(object sender, RoutedEventArgs e)
        {
            var selectedItem = (sender as FrameworkElement).DataContext;

            // Use the selected item as needed
            var account = selectedItem as Account;
            var selectedAccountDetails = new AccountBobj
            {
                Account = account,
                UserId = userId
            };

            var myView = CoreApplication.CreateNewView();
            //can overload
            int newViewId = 0;

            await myView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Frame newFrame = new Frame();
                newFrame.Content = myView;
                newFrame.Navigate(typeof(FullAccountDetails), selectedAccountDetails);//object parameter
                Window.Current.Content = newFrame;
                Window.Current.Activate();
              
                newViewId = ApplicationView.GetForCurrentView().Id;

            });

            await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId, ViewSizePreference.UseMinimum);
        }
    }
}
