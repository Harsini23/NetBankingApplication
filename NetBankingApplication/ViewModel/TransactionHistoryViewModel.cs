using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace NetBankingApplication.ViewModel
{


    public class TransactionHistoryViewModel : TransactionHistoryBaseViewModel
    {
        TransactionHistoryUseCase Transaction;
        public override void GetTransactionData(string userId)
        {
           
            Transaction = new TransactionHistoryUseCase(new TransactionHistoryRequest(userId, new CancellationTokenSource()), new PresenterTransactionHistoryCallback(this));
            Transaction.Execute();
            
        }
    }


    public class PresenterTransactionHistoryCallback : IPresenterTransactionHistoryCallback
    {
        private TransactionHistoryViewModel transactionHistoryViewModel;
        public PresenterTransactionHistoryCallback()
        {
                
        }
        public PresenterTransactionHistoryCallback(TransactionHistoryViewModel transactionHistoryViewModel)
        {
            this.transactionHistoryViewModel = transactionHistoryViewModel;
        }

        public void OnError(String response)
        {
        }

        public void OnFailure(ZResponse<TransactionHistoryResponse> response)
        {
           
        }

        public void OnSuccess(ZResponse<TransactionHistoryResponse> response)
        {
          var TransctionList=response.Data.allTransactions;
          populateData(TransctionList);
        }

        //newContacts = Contacts.OrderBy(c => c.FirstName);
        //    ContactSort.Clear();
        //    foreach (var item in newContacts)
        //        ContactSort.Add(item);
        public async void populateData( List<Transaction> TransactionList)
        {
          
            int temp = 0;
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  if (TransactionList.Count <= 0)
                  {
                      transactionHistoryViewModel.TextBoxVisibility = Visibility.Visible;
                      return;
                  }
                 
                  transactionHistoryViewModel.TextBoxVisibility = Visibility.Collapsed;


                  transactionHistoryViewModel.AllSortedTransactions.Clear();
                  transactionHistoryViewModel.AllSortedIndexedTransactions.Clear();
                  transactionHistoryViewModel.FinalSortedIndexedTransactions.Clear();

                  var AllTransactions = TransactionList.OrderByDescending(i => DateTime.Parse(i.Date));
                  transactionHistoryViewModel.RecipientNameInitials.Clear();
                  foreach (var i in AllTransactions)
                  {

                      transactionHistoryViewModel.AllSortedTransactions.Add(i);
                      var date = DateTime.Parse(i.Date);
                      TransactionBObj t = new TransactionBObj
                      {
                          TransactionId = i.TransactionId,
                          Date = DateTime.Parse(i.Date),
                          TransactionType = i.TransactionType,
                          Remark = i.Remark,
                          Amount = i.Amount.ToString(),
                          FromAccount = i.FromAccount,
                          ToAccount = i.ToAccount,
                          Status = i.Status,
                          Name = i.Name,
                          Index = temp++,
                          Time=date.TimeOfDay.ToString()
                      };
                      transactionHistoryViewModel.AllSortedIndexedTransactions.Add(t);
                      transactionHistoryViewModel.RecipientNameInitials.Add(i.Name.Substring(0, 1));
                  }


                  var query = from i in transactionHistoryViewModel.AllSortedIndexedTransactions
                              group i by i.Date.Date into g
                              select new { GroupName = g.Key, Items = g };

                 
                  foreach (var g in query)
                  {
                      GroupInfosList info = new GroupInfosList();

                      int n = g.GroupName.Date.Day;
                      string ordinalSuffix = n % 100 == 11 || n % 100 == 12 || n % 100 == 13 ? "th" : n % 10 == 1 ? "st" : n % 10 == 2 ? "nd" : n % 10 == 3 ? "rd" : "th";
                   
                      info.Key = g.GroupName.Date.ToString("dd'\u00A0'MMM'\u00A0'yyyy", CultureInfo.InvariantCulture) ;

                      info.Key = info.Key.Insert(2, ordinalSuffix);
                      info.Count = g.Items.Count();


                      foreach (var item in g.Items)
                      {
                          info.Add(item);
                      }
                      transactionHistoryViewModel.FinalSortedIndexedTransactions.Add(info);
                  }
              });

        }
    }


    public abstract class TransactionHistoryBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<Transaction> AllSortedTransactions = new ObservableCollection<Transaction>();
        public ObservableCollection<TransactionBObj> AllSortedIndexedTransactions = new ObservableCollection<TransactionBObj>();
        public ObservableCollection<GroupInfosList> FinalSortedIndexedTransactions = new ObservableCollection<GroupInfosList>();
        public abstract void GetTransactionData(string UserId);
        public List<Transaction> AllTransactionList= new List<Transaction>(){};

        public List<String> RecipientNameInitials = new List<string>();

        private string _error = String.Empty;
    

        private Visibility _textBoxVisibility = Visibility.Collapsed;
        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            set { _textBoxVisibility = value;
                OnPropertyChangedAsync(nameof(TextBoxVisibility));

            }
        }



    }

}
