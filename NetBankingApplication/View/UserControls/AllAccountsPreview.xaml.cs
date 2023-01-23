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
        // public List<AppWindow> appWindows = new List<AppWindow>();
        // Dictionary<int, int> appWindowList = new Dictionary<int, int>();


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
        public AllAccountsPreview()
        {

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
        //   button.IsEnabled = false;

            var item = button.DataContext;

            // Get the container of the item
            var container = AllTransactionListView.ContainerFromItem(item);

            // Get the index of the item
            var index = AllTransactionListView.IndexFromContainer(container);

            await GoToOpenPage(selectedAccountDetails,index);

        }

        //public void DisableButton( )
        //{
        //    AllTransactionListView.
        //}


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

                // await appWindow.TryShowAsync();
            }
            else
            {
                await appWindow.TryShowAsync();
            }
            //AppWindow appWindow = await AppWindow.TryCreateAsync();
          //  appWindows.Add(appWindow);
          //  appWindowList.Add(appWindows.IndexOf(appWindow), buttonIndex);
            //appWindow.Closed += AppWindow_Closed;
          
          

        }
        //private void AppWindow_Closed(AppWindow sender, object args)
        //{
        //    int index = appWindows.IndexOf(sender);
        //    foreach (var appWindow in appWindowList)
        //    {
        //        if (appWindow.Key == index)
        //        {
        //            var val = appWindow.Value;
        //            var container = AllTransactionListView.ContainerFromIndex(val) as FrameworkElement;
        //            if (container != null)
        //            {
        //                var button = container.FindName("DisplayFullAccountDetails") as Button;
        //                if (button != null)
        //                {
        //                    button.IsEnabled = true;
        //                }
        //            }
        //        }
        //    }
        //}
        private void AppWindow_Closed(AppWindow sender, object args)
        {

            var keysToRemove = appWindows.Where(x => x.Value == sender).Select(x => x.Key).ToList();

            // Remove the items
            foreach (var key in keysToRemove)
            {
                appWindows.Remove(key);
            }

           // appWindows.RemoveWhere(x => x.Value == sender);
        }



    }
}
