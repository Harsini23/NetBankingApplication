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
using Windows.UI;
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
    public sealed partial class AllAccountsPreview : UserControl, ICloseAllWindows
    {
        private GetAllAccountsBaseViewModel GetAllAccountsViewModel;
        private string userId;
        private string currentUserId;
        static Dictionary<int, AppWindow> appWindows = new Dictionary<int, AppWindow>();

        private LoginBaseViewModel LoginViewModel;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //GetAllAccountsViewModel.GetAllAccounts(userId);
            //Bindings.Update();
            //DetailedAccountOverview detailedAccountOverview = new DetailedAccountOverview("", "");
            //CurrentSelectedModule = detailedAccountOverview;
        }
        public AllAccountsPreview()
        {
          
            //this.InitializeComponent();
            //GetAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            //currentUserId = UserId;
            //GetAllAccountsViewModel.GetAllAccounts("UID6df59172");
            //Bindings.Update();
        }
        public AllAccountsPreview(string userId)
        {
            this.InitializeComponent();
            GetAllAccountsViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            currentUserId = userId;
            GetAllAccountsViewModel.GetAllAccounts(currentUserId);
            Bindings.Update();
        }
       
        public string UserId
        {
            get { return (string)GetValue(UserIdProperty); }
            set
            {
                SetValue(UserIdProperty, value);
                currentUserId = UserId;
            }
        }

        public static readonly DependencyProperty UserIdProperty =
            DependencyProperty.Register("UserId", typeof(string), typeof(TransactionHistory), new PropertyMetadata(null));
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


        private FrameworkElement SomeElemnt;
        //private Frame newFrame;

        public  async Task GoToOpenPage(AccountBobj selectedAccountDetails,int buttonIndex)
        {
            if (!appWindows.TryGetValue(buttonIndex, out AppWindow appWin))
            {
                AppWindow appWindow = await AppWindow.TryCreateAsync();
                appWindows[buttonIndex] = appWindow;
                appWindow.Closed += AppWindow_Closed;
                Frame newFrame = new Frame();
                newFrame.Navigate(typeof(FullAccountDetails), selectedAccountDetails);
                //newFrame.Loaded += NewFrame_Loaded;
                ElementCompositionPreview.SetAppWindowContent(appWindow, newFrame);
                ThemeSwitch.AddUIRootElement(newFrame);//change theme by registering
                await appWindow.TryShowAsync();
            }
            else if(appWindows.TryGetValue(buttonIndex, out AppWindow appW))
            {
              await appW.TryShowAsync();
            }
          

        }

        //private Dictionary<UIContext, FrameworkElement> SomeDictoionary = new Dictionary<UIContext, FrameworkElement>();
        ////private Page _rootPage;
        //private void NewFrame_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var frame = sender as Frame;
        //    var _rootPage = frame.Content as Page;
        //    SomeDictoionary.Add(_rootPage.UIContext, _rootPage);
        //    ThemeSwitch.RegisterElement(_rootPage);

        //}
       
        private void AppWindow_Closed(AppWindow sender, object args)
        {

            var keysToRemove = appWindows.Where(x => x.Value == sender).Select(x => x.Key).ToList();
            //ThemeSwitch.UnRegisterElement(_rootPage);

            //foreach (var i in SomeDictoionary)
            //{
            //    if (sender.UIContext == i.Key)
            //    {
            //        ThemeSwitch.UnRegisterElement(i.Value);
            //    }
            //}

            //// Remove the items
            //SomeDictoionary.Remove(sender.UIContext);
            foreach (var key in keysToRemove)
            {
                appWindows.Remove(key);
            }

        }

        public void closeAllWindows()
        {
            foreach (var i in appWindows)
            {
                i.Value.CloseAsync();
            }
        }
      
    }
}
