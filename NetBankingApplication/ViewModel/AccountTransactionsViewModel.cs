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
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;

namespace NetBankingApplication.ViewModel
{
   
     public class AccountTransactionsViewModel : AccountTransactionsBaseViewModel
    {
        AccountTransactions Transaction;
        private static string Accountnumber;
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
            Transaction = new AccountTransactions(new AccountTransactionsRequest(accountId,userId, new CancellationTokenSource()), new PresenterAccountTransactionsCallback(this));
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

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<AccountTransactionsResponse> response)
        {
           
        }

        public async void OnSuccessAsync(ZResponse<AccountTransactionsResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                AccountTransactionsViewModel.AccountDetails = response.Data.account;
                populateData(response.Data.allTransactions);
            });
        }

        public  void populateData( List<AccountTransactionBObj> TransactionList)
        {
          
            double income = 0, expense = 0;
            int incomeCount=0,expenseCount=0;
            var SortedTransactionList = TransactionList.OrderByDescending(i => DateTime.Parse(i.DateOfTransaction));
            var SortedTransactionOfCurrentMonth = SortedTransactionList.Where(i => DateTime.Parse(i.DateOfTransaction).Month == DateTime.Now.Date.Month);
            String recentTransactionDate="";
            if (SortedTransactionList != null && SortedTransactionList.Count() >0)
            {
              recentTransactionDate = SortedTransactionList.FirstOrDefault().DateOfTransaction.ToString();
            }
           
            foreach(var i in SortedTransactionOfCurrentMonth)
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
            AccountTransactionsViewModel.AllSortedAccountTransactions.Clear();

            if (TransactionList.Count <= 0)
                  {
                      AccountTransactionsViewModel.TextBoxVisibility = Visibility.Visible;
                //AccountTransactionsViewModel.GridSplitterVisibility = Visibility.Collapsed;  
                      return;
                  }
                  AccountTransactionsViewModel.TextBoxVisibility = Visibility.Collapsed;

                  foreach (var i in SortedTransactionList)
                  {
                      AccountTransactionsViewModel.AllSortedAccountTransactions.Add(i);
                  }
                  AccountTransactionsViewModel.CurrentMonthExpense = expense.ToString();
                  AccountTransactionsViewModel.CurrentMonthIncome = income.ToString(); AccountTransactionsViewModel.CurrentMonthExpenseTransactionCount = expenseCount.ToString();
                  AccountTransactionsViewModel.CurrentMonthIncomeTransactionCount = incomeCount.ToString();
                  AccountTransactionsViewModel.LastTransactionDate = recentTransactionDate.ToString();

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
                OnPropertyChanged(nameof(AccountDetails));
            }
        }
        //public string CurrentMonthExpense { get; set; }
       // public string CurrentMonthIncome { get; set; }
        //public string CurrentMonthExpenseTransactionCount { get; set; }
        //public string CurrentMonthIncomeTransactionCount { get; set; }

        private string _currentMonthIncomeTransactionCount = "0";
        public string CurrentMonthIncomeTransactionCount
        {
            get { return this._currentMonthIncomeTransactionCount; }
            set
            {
                _currentMonthIncomeTransactionCount = value;
                OnPropertyChanged(nameof(CurrentMonthIncomeTransactionCount));
                //SetProperty(ref _response, value);
            }
        }

        private string _currentMonthExpenseTransactionCount = "0";
        public string CurrentMonthExpenseTransactionCount
        {
            get { return this._currentMonthExpenseTransactionCount; }
            set
            {
                _currentMonthExpenseTransactionCount = value;
                OnPropertyChanged(nameof(CurrentMonthExpenseTransactionCount));
            }
        }


        //public string LastTransactionDate { get; set; }

        private string _lastTransactionDate = "No transactions yet ;)";
        public string LastTransactionDate
        {
            get { return this._lastTransactionDate; }
            set
            {
                _lastTransactionDate = value;
                OnPropertyChanged(nameof(LastTransactionDate));
            }
        }


        private string _currentMonthExpense = "0";
        public string CurrentMonthExpense
        {
            get { return this._currentMonthExpense; }
            set
            {
                _currentMonthExpense = value;
                OnPropertyChanged(nameof(CurrentMonthExpense));
            }
        }

        private string _currentMonthIncome = "0";
        public string CurrentMonthIncome
        {
            get { return this._currentMonthIncome; }
            set
            {
                _currentMonthIncome = value;
                OnPropertyChanged(nameof(CurrentMonthIncome));
            }
        }




        private Visibility _textBoxVisibility = Visibility.Collapsed;
        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            set
            {
                _textBoxVisibility = value;
                OnPropertyChanged(nameof(TextBoxVisibility));

            }
        }


        //private Visibility _gridSplitterVisibility = Visibility.Collapsed;
        //public Visibility GridSplitterVisibility
        //{
        //    get { return _gridSplitterVisibility; }
        //    set
        //    {
        //        _gridSplitterVisibility = value;
        //        OnPropertyChanged(nameof(GridSplitterVisibility));

        //    }
        //}



    }


}
