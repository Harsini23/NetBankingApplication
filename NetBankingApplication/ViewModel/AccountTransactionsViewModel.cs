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
            var SortedTransactionList = TransactionList.OrderByDescending(i => DateTime.Parse(i.DateOfTransaction));
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  AccountTransactionsViewModel.AllSortedAccountTransactions.Clear();

                  foreach (var i in SortedTransactionList)
                  AccountTransactionsViewModel.AllSortedAccountTransactions.Add(i);
              });
        }
    }


    public abstract class AccountTransactionsBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<AccountTransactionBObj> AllSortedAccountTransactions = new ObservableCollection<AccountTransactionBObj>();
         public abstract void GetAllTransactions(string accountno,string userid );
        public Account AccountDetails;
    }
}
