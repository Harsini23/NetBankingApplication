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
            String recentTransactionDate="";

            if (TransactionList.Count > 0)
            {
                recentTransactionDate = TransactionList[0].DateOfTransaction;
            }

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

                  AccountTransactionsViewModel?.updateBindingInstance?.updateBindingsAsync();


              });

            //CoreApplicationView currentView = CoreApplication.GetCurrentView();
            //await currentView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    AccountTransactionsViewModel.updateBindingInstance.updateBindingsAsync();
            //    // BindingOperations.GetBindingExpression(myTextBlock, TextBlock.TextProperty).Update();
            //});
          
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  AccountTransactionsViewModel.updateBindingInstance?.updateBindingsAsync();
                  // Perform UI-related work here
                  // like updating bindings, access UI elements and so on
                  // Bindings.Update();
                  //newViewId = ApplicationView.GetForCurrentView().Id;
              });


        }
    }

    public abstract class AccountTransactionsBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<AccountTransactionBObj> AllSortedAccountTransactions = new ObservableCollection<AccountTransactionBObj>();
        public abstract void GetAllTransactions(string accountno,string userid);

        public CoreDispatcher _dispatcher { get; set; }

        public Account AccountDetails;
        //public string CurrentMonthExpense { get; set; }
        public string CurrentMonthIncome { get; set; }
        public string CurrentMonthExpenseTransactionCount { get; set; }
        public string CurrentMonthIncomeTransactionCount { get; set; }

        public string LastTransactionDate { get; set; }

        private string _currentMonthExpense = String.Empty;
        public string CurrentMonthExpense
        {
            get { return this._currentMonthExpense; }
            set
            {
                _currentMonthExpense = value;
                OnViewPropertyChange(nameof(CurrentMonthExpense), _dispatcher);
                //SetProperty(ref _response, value);
            }
        }

       // private string _currentMonthExpense;

        public IUpdateBindings updateBindingInstance { get; set; }
    }

    public interface IUpdateBindings
    {
        Task updateBindingsAsync();
    }
}
