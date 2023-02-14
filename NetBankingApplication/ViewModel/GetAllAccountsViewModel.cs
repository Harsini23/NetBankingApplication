using Library;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class GetAllAccountsViewModel : GetAllAccountsBaseViewModel
    {
        GetAllAccounts getAllAccounts;

        ISwitchUserView tranferView;
        public override void GetAllAccounts(string userId)
        {
            SetValueForCallback();
            getAllAccounts = new GetAllAccounts(new GetAllAccountsRequest(userId, new CancellationTokenSource()),  new PresenterGetAllAccountsCallback(this));
            getAllAccounts.Execute();
        }
        private void SetValueForCallback()
        {
            tranferView = TransferAmountView;
        }
    }

    public class PresenterGetAllAccountsCallback : IPresenterGetAllAccountsCallback
    {
        private GetAllAccountsViewModel GetAllAccountsViewModel;
        public PresenterGetAllAccountsCallback()
        {

        }
        public PresenterGetAllAccountsCallback(GetAllAccountsViewModel getAllAccountsViewModel)
        {
            this.GetAllAccountsViewModel = getAllAccountsViewModel;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<GetAllAccountsResponse> response)
        {

        }

        public void OnSuccess(ZResponse<GetAllAccountsResponse> response)
        {
            var allAccounts = response.Data.allAccount;
            populateData(allAccounts);
            populateBalanceData(response.Data.allAccountBalance);
            handleCallbackAsync();
           
            //GetAllAccountsViewModel.AllAccounts.Clear();
            //GetAllAccountsViewModel.AllAccountNumbers.Clear();

            //GetAllAccountsViewModel.AllAccounts = allAccounts;
            //foreach(var i in response.Data.allAccount)
            //{
            //    GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
            //  //  GetAllAccountsViewModel.AllAccounts.Add(i);

            //}
        }
            private async Task handleCallbackAsync()
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  GetAllAccountsViewModel?.TransferAmountView?.SwitchBasedOnUserAccount();
              });
            }

        public async void populateData(List<Account> allAccounts)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {

                  GetAllAccountsViewModel.AllAccounts.Clear();
                  GetAllAccountsViewModel.accounts.Clear();

                  GetAllAccountsViewModel.AllAccountNumbers.Clear();
                 // GetAllAccountsViewModel.AllAccounts = allAccounts;
                  foreach (var i in allAccounts)
                  {
                      GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
                      GetAllAccountsViewModel.AllAccounts.Add(i);
                      GetAllAccountsViewModel.accounts.Add(i);

                  }
              });
        }

        public async void populateBalanceData(List<AccountBalance> allBalances)
        {
            if (allBalances.Count == 1)
            {
                GetAllAccountsViewModel.SingleAccountBalance = allBalances[0].TotalBalance.ToString();
            }
       
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  GetAllAccountsViewModel.allBalances.Clear();
                  foreach(var i in allBalances)
                  {
                      
                      GetAllAccountsViewModel.allBalances.Add(i);
                  }
             
              });
        }    
    }
    public abstract class GetAllAccountsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetAllAccounts(string userId);
        public List<Account> AllAccounts = new List<Account>();
        public ObservableCollection<String> AllAccountNumbers = new ObservableCollection<string>();
        public ObservableCollection<Account> accounts= new ObservableCollection<Account>();
        public ObservableCollection<AccountBalance> allBalances= new ObservableCollection<AccountBalance>();
        public ISwitchUserView TransferAmountView { get; set; }

        private  string _currentAccountSelection = String.Empty;
        public  string CurrentAccountSelection
        {
            get {
                if (PreviousSelection != null)
                {
                    return PreviousSelection;
                }
                return this._currentAccountSelection;
            }
            set
            {
                _currentAccountSelection = value;
                OnPropertyChangedAsync(nameof(CurrentAccountSelection));
                //SetProperty(ref _response, value);
            }
        }

        private string _singleAccountBalance = String.Empty;
        public string SingleAccountBalance
        {
            get
            {
                return this._singleAccountBalance;
            }
            set
            {
                _singleAccountBalance = "₹ " + value;
                OnPropertyChangedAsync(nameof(SingleAccountBalance));
                //SetProperty(ref _response, value);
            }
        }


        private string _currentAccountBalance = "Choose Account";
        public string CurrentAccountBalance
        {
            get
            {
                return this._currentAccountBalance;
            }
            set
            {
                _currentAccountBalance = "₹ "+value;
                OnPropertyChangedAsync(nameof(CurrentAccountBalance));
                //SetProperty(ref _response, value);
            }
        }


        public static string PreviousSelection
        {
            get; set;
        }

    }

    public interface ISwitchUserView
    {
         void SwitchBasedOnUserAccount();
    }
}
