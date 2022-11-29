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
    public sealed partial class DetailedAccountOverview : Page
    {
        private AccountTransactionsBaseViewModel AccountTransactionsViewModel;

        PresenterService AccountTransactionsVMserviceProviderInstance;
        private string currentUserId;
        private string currentAccount;
        
        public DetailedAccountOverview(string userId)
        {
            this.InitializeComponent();
            AccountTransactionsVMserviceProviderInstance = PresenterService.GetInstance();
            AccountTransactionsViewModel =  AccountTransactionsVMserviceProviderInstance.Services.GetService<AccountTransactionsBaseViewModel>();

            currentUserId = userId;
            //currentAccount = accountNo;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            AccountTransactionsViewModel.GetAllTransactions("978374847492", "Harsh");
        }
    }
}
