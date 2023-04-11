using Library;
using Library.BankingNotification;
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
using static Library.Domain.UseCase.GetAllAccounts;

namespace NetBankingApplication.ViewModel
{
    public class GetAllAccountsViewModel : GetAllAccountsBaseViewModel
    {
        GetAllAccounts getAllAccounts;

        public override void GetAllAccounts(string userId, bool fetchTransactionAccounts=false)
        {
            getAllAccounts = new GetAllAccounts(new GetAllAccountsRequest(userId,fetchTransactionAccounts, new CancellationTokenSource()), new PresenterGetAllAccountsCallback(this));
            getAllAccounts.Execute();
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
            BankingNotification.AccountDeleted += BankingNotification_AccountDeleted;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<GetAllAccountsResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetAllAccountsResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                var allAccounts = response.Data.AllAccount;

                populateData(allAccounts);
                populateBalanceData(response.Data.AllAccountBalance);
                handleCallbackAsync();
                BankingNotification.AccountUpdated += BankingNotification_AccountUpdated;
                BankingNotification.AccountBalanceEdited += BankingNotification_AccountBalanceEdited;

            });


            //GetAllAccountsViewModel.AllAccounts.Clear();
            //GetAllAccountsViewModel.AllAccountNumbers.Clear();

            //GetAllAccountsViewModel.AllAccounts = allAccounts;
            //foreach(var i in response.Data.allAccount)
            //{
            //    GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
            //  //  GetAllAccountsViewModel.AllAccounts.Add(i);

            //}
        }

        private async void BankingNotification_AccountBalanceEdited(Account account)
        {

            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                Account accountToEdit = GetAllAccountsViewModel.AllAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);
                if (accountToEdit != null)
                {
                    int index = GetAllAccountsViewModel.AllAccounts.IndexOf(accountToEdit);
                    accountToEdit.TotalBalance = account.TotalBalance;
                    GetAllAccountsViewModel.AllAccounts[index] = accountToEdit;
                }
            });
        }

        private async void BankingNotification_AccountDeleted(Account account)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                GetAllAccountsViewModel.AllAccountNumbers.Remove(account.AccountNumber);
                Account accountToDelete = GetAllAccountsViewModel.AllAccounts.FirstOrDefault(p => p.AccountNumber == account.AccountNumber);
                if (accountToDelete != null)
                {
                    int index = GetAllAccountsViewModel.AllAccounts.IndexOf(accountToDelete);
                    GetAllAccountsViewModel.AllAccounts[index] = accountToDelete;
                    GetAllAccountsViewModel.AllAccounts.Remove(accountToDelete);
                }
                GetAllAccountsViewModel.NotificationMessage = "Successfully Closed Account";
                GetAllAccountsViewModel.NotificationAlert?.CallNotification();
            });
        }

        private async void BankingNotification_AccountUpdated(Account account)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                GetAllAccountsViewModel.AllAccountNumbers.Add(account.AccountNumber);
                GetAllAccountsViewModel.AllAccounts.Add(account);
                //GetAllAccountsViewModel.accounts.Add(account);
            });
        }

        private void handleCallbackAsync()
        {

            GetAllAccountsViewModel?.TransferAmountView?.SwitchBasedOnUserAccount();
        }

        public void populateData(List<Account> allAccounts)
        {

            GetAllAccountsViewModel.AllAccounts.Clear();
            //GetAllAccountsViewModel.accounts.Clear();

            GetAllAccountsViewModel.AllAccountNumbers.Clear();


            // GetAllAccountsViewModel.AllAccounts = allAccounts;
            foreach (var i in allAccounts)
            {
                GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
                GetAllAccountsViewModel.AllAccounts.Add(i);
                //GetAllAccountsViewModel.accounts.Add(i);

            }

        }

        public void populateBalanceData(List<AccountBalance> allBalances)
        {
            if (allBalances.Count == 1)
            {
                GetAllAccountsViewModel.SingleAccountBalance = allBalances[0].TotalBalance.ToString();
                if (allBalances[0].TotalBalance == 0)
                {
                    GetAllAccountsViewModel.ZerobalanceView?.ZeroBalanceNotification();
                }
            }
            GetAllAccountsViewModel.allBalances.Clear();
            foreach (var i in allBalances)
            {

                GetAllAccountsViewModel.allBalances.Add(i);
            }

        }
    }
    public abstract class GetAllAccountsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetAllAccounts(string userId,bool fetchTransactionAccounts=false);
        public ObservableCollection<Account> AllAccounts = new ObservableCollection<Account>();
        public ObservableCollection<String> AllAccountNumbers = new ObservableCollection<string>();
        public ObservableCollection<AccountBalance> allBalances = new ObservableCollection<AccountBalance>();

        public ISwitchUserView TransferAmountView { get; set; }
        public INotificationAlert NotificationAlert { get; set; }
        public ZeroBalance ZerobalanceView { get; set; }

        private string _currentAccountSelection = String.Empty;
        public string CurrentAccountSelection
        {
            get
            {
                if (PreviousSelection != null)
                {
                    return PreviousSelection;
                }
                return this._currentAccountSelection;
            }
            set
            {
                _currentAccountSelection = value;
                OnPropertyChanged(nameof(CurrentAccountSelection));
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
                OnPropertyChanged(nameof(SingleAccountBalance));
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
                if (value == "Choose Account")
                {
                    _currentAccountBalance = value;
                }
                else
                {
                    _currentAccountBalance = "₹ " + value;
                }
                OnPropertyChanged(nameof(CurrentAccountBalance));
            }
        }

        private string _notificationMessage;
        public string NotificationMessage { 
            get { return this._notificationMessage; }
            set { this._notificationMessage = value; OnPropertyChanged(nameof(NotificationMessage)); } 
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

    public interface ZeroBalance
    {
        void ZeroBalanceNotification();

    }
}
