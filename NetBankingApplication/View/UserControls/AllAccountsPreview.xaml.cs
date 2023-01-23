using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
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
        Dictionary<int, AppWindow> appWindows = new Dictionary<int, AppWindow>();
     

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
                var Account = new Account();
                Account = account;
            var selectedAccountDetails = new AccountBobj
            {
                Account = Account,
                UserId = userId,
            };

          
           var button = sender as Button;
            var item = button.DataContext;
            var container = AllTransactionListView.ContainerFromItem(item);
            var index = AllTransactionListView.IndexFromContainer(container);
            await GoToOpenPage(selectedAccountDetails,index);

        }

        public  async Task GoToOpenPage(AccountBobj selectedAccountDetails,int buttonIndex)
        {

            AppWindow appWindow;
            if (!appWindows.TryGetValue(buttonIndex, out appWindow) || !appWindow.IsVisible)
            {
                appWindow = await AppWindow.TryCreateAsync();
                appWindows[buttonIndex] = appWindow;
                appWindow.Closed += AppWindow_Closed;
                Frame OpenPage1 = new Frame();
                OpenPage1.Navigate(typeof(FullAccountDetails), selectedAccountDetails);
                ElementCompositionPreview.SetAppWindowContent(appWindow, OpenPage1);
                await appWindow.TryShowAsync();

            }
            else
            {
                await appWindow.TryShowAsync();
            }

        }
       
        private void AppWindow_Closed(AppWindow sender, object args)
        {

            var keysToRemove = appWindows.Where(x => x.Value == sender).Select(x => x.Key).ToList();

            // Remove the items
            foreach (var key in keysToRemove)
            {
                appWindows.Remove(key);
            }

        }



    }
}
