using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace NetBankingApplication.ViewModel
{
   
     public class AccountTransactionsViewModel : AccountTransactionsBaseViewModel
    {
        AccountTransactions Transaction;
        public AccountTransactionsViewModel()
        {
            PresenterTransferAmountCallback.ValueChanged += TransferAmountViewModel_ValueChanged;
        }

        private void TransferAmountViewModel_ValueChanged(string value,string user)
        {
            GetAllTransactions(value,user);
        }

        public override void GetAllTransactions(string accountId, string userId)
        {
            Transaction = new AccountTransactions(new AccountTransactionsRequest(accountId,userId), new PresenterAccountTransactionsCallback(this));
            Transaction.Execute();
        }
    }


    public class PresenterAccountTransactionsCallback : IPresenterAccountTransactionsCallback
    {
        private AccountTransactionsViewModel AccountTransactionsViewModel;
        public PresenterAccountTransactionsCallback()
        {
                
        }
        public PresenterAccountTransactionsCallback(AccountTransactionsViewModel AccountTransactionsViewModel)
        {
            this.AccountTransactionsViewModel = AccountTransactionsViewModel;
        }

        public void OnError(ZResponse<AccountTransactionsResponse> response)
        {
        }

        public void OnFailure(ZResponse<AccountTransactionsResponse> response)
        {
           
        }

        public void OnSuccess(ZResponse<AccountTransactionsResponse> response)
        {
          var TransactionList = response.Data.allTransactions;
            AccountTransactionsViewModel.AccountDetails = response.Data.account;
          populateData(TransactionList);
        }

     
        public async void populateData( List<AccountTransactionBObj> TransactionList)
        {
            double income = 0, expense = 0;
            int incomeCount=0,expenseCount=0;
            var SortedTransactionList = TransactionList.OrderByDescending(i => DateTime.Parse(i.DateOfTransaction));
            String recentTransactionDate=SortedTransactionList.FirstOrDefault().DateOfTransaction.ToString();
           
            foreach(var i in SortedTransactionList)
            {
                if (i.TransactionType == Library.Model.Enum.TransactionType.Debited)
                {
                    expense += i.Amount;
                    expenseCount++;
                }
                else
                {
                    income += i.Amount;
                    incomeCount++;
                }
            }

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  AccountTransactionsViewModel.AllSortedAccountTransactions.Clear();
                
                  foreach (var i in SortedTransactionList)
                  {
                      AccountTransactionsViewModel.AllSortedAccountTransactions.Add(i);
                  }
                  AccountTransactionsViewModel.CurrentMonthExpense = expense.ToString();
                  AccountTransactionsViewModel.CurrentMonthIncome = income.ToString(); AccountTransactionsViewModel.CurrentMonthExpenseTransactionCount = expenseCount.ToString();
                  AccountTransactionsViewModel.CurrentMonthIncomeTransactionCount = incomeCount.ToString();
                  AccountTransactionsViewModel.LastTransactionDate = recentTransactionDate.ToString();

              });


        }
    }

    public abstract class AccountTransactionsBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<AccountTransactionBObj> AllSortedAccountTransactions = new ObservableCollection<AccountTransactionBObj>();
        public abstract void GetAllTransactions(string accountno,string userid);

        private Account _accountDetails;
        public Account AccountDetails
        {
            get
            {
                return this._accountDetails;
            }
            set
            {
                _accountDetails= value;
                OnPropertyChangedAsync(nameof(AccountDetails));
            }
        }
        //public string CurrentMonthExpense { get; set; }
       // public string CurrentMonthIncome { get; set; }
        //public string CurrentMonthExpenseTransactionCount { get; set; }
        //public string CurrentMonthIncomeTransactionCount { get; set; }

        private string _currentMonthIncomeTransactionCount = String.Empty;
        public string CurrentMonthIncomeTransactionCount
        {
            get { return this._currentMonthIncomeTransactionCount; }
            set
            {
                _currentMonthIncomeTransactionCount = value;
                OnPropertyChangedAsync(nameof(CurrentMonthIncomeTransactionCount));
                //SetProperty(ref _response, value);
            }
        }

        private string _currentMonthExpenseTransactionCount = String.Empty;
        public string CurrentMonthExpenseTransactionCount
        {
            get { return this._currentMonthExpenseTransactionCount; }
            set
            {
                _currentMonthExpenseTransactionCount = value;
                OnPropertyChangedAsync(nameof(CurrentMonthExpenseTransactionCount));
                //SetProperty(ref _response, value);
            }
        }


        //public string LastTransactionDate { get; set; }

        private string _lastTransactionDate = String.Empty;
        public string LastTransactionDate
        {
            get { return this._lastTransactionDate; }
            set
            {
                _lastTransactionDate = value;
                OnPropertyChangedAsync(nameof(LastTransactionDate));
                //SetProperty(ref _response, value);
            }
        }




        private string _currentMonthExpense = String.Empty;
        public string CurrentMonthExpense
        {
            get { return this._currentMonthExpense; }
            set
            {
                _currentMonthExpense = value;
                OnPropertyChangedAsync(nameof(CurrentMonthExpense));
                //SetProperty(ref _response, value);
            }
        }

        private string _currentMonthIncome = String.Empty;
        public string CurrentMonthIncome
        {
            get { return this._currentMonthIncome; }
            set
            {
                _currentMonthIncome = value;
                OnPropertyChangedAsync(nameof(CurrentMonthIncome));
                //SetProperty(ref _response, value);
            }
        }


    }

   
}
