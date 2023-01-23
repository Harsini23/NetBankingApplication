using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
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
    /// 

    public sealed partial class FullAccountDetails : Page, IUpdateBindings,INotifyPropertyChanged
    {
        AccountBobj selectedAccount;
        private string selectedAccountNumber;
        private string selectedUserId;
        private string expense;
       // private string test;

        public Account CurrentSelectedAccount;

        private AccountTransactionsBaseViewModel AccountTransactionsViewModel;
        PresenterService AccountTransactionsVMserviceProviderInstance;

        public event PropertyChangedEventHandler PropertyChanged;


        protected async Task OnPropertyChangedAsync(string propertyName)
        {
            //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            //    Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //    {

            //    });
            var myView =  CoreApplication.GetCurrentView();

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            //await myView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
               
            //});
        }

        // Set the data context for the new view
     


        public FullAccountDetails()
        {
            this.InitializeComponent();
            AccountTransactionsVMserviceProviderInstance = PresenterService.GetInstance();
            AccountTransactionsViewModel = AccountTransactionsVMserviceProviderInstance.Services.GetService<AccountTransactionsBaseViewModel>();

           // AccountTransactionsViewModel.GetAllTransactions(selectedAccountNumber, selectedUserId);
            Bindings.Update();
            this.DataContextChanged += (s, e) => this.Bindings.Update();
            //Debug.WriteLine(AccountTransactionsViewModel.AllSortedAccountTransactions);

        }
        private async void PopulateData()
        {
            expense=AccountTransactionsViewModel.CurrentMonthExpense;
             //   Bindings.Update();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            selectedAccount = e.Parameter as AccountBobj;
            CurrentSelectedAccount= selectedAccount.Account;
            selectedAccountNumber = selectedAccount.Account.AccountNumber.ToString();
            selectedUserId = selectedAccount.UserId;
           AccountTransactionsViewModel.updateBindingInstance = this;
            AccountTransactionsViewModel.GetAllTransactions(selectedAccountNumber, selectedUserId);

            AccountTransactionsViewModel._dispatcher = selectedAccount._dispatcher;
            PopulateData();
           // this.DataContextChanged += (s, e) => Bindings.Update();
            Bindings.Update();


        }

        //public async Task updateBindingsAsync()
        //{
        //    PopulateData();

        //    //int currentViewId = ApplicationView.GetForCurrentView().Id;
           
        //    //if (currentViewId != null)
        //    //{
        //    //    // Use the Dispatcher to access the new window's UI thread
        //    //    await currentViewId.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
        //    //    {
        //    //        // Update the bindings
        //    //        Bindings.Update();
        //    //    });
        //    //}
        //    // Bindings.Update();
        //}

        async Task IUpdateBindings.updateBindingsAsync()
        {
            //PopulateData();
            //CoreApplicationView targetView = CoreApplication.GetCurrentView();
            //await targetView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
             test=AccountTransactionsViewModel.CurrentMonthExpense;
                this.Bindings.Update();
            Debug.WriteLine("");
                // Update the target view here
            //});

        }
        private string _test = String.Empty;
        public string test
        {
            get { return this._test; }
            set
            {
                _test = value;
                OnPropertyChangedAsync(nameof(test));
                //SetProperty(ref _response, value);
            }
        }




        //calculate total income and expense of that month on that particular account (credited and debited info)
    }


}
