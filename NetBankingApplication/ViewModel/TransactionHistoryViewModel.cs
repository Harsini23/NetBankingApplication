using Library;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
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

        public void OnError(BException response)
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
                      DateTime time = DateTime.ParseExact(date.TimeOfDay.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                      var finaltime=time.ToString("hh:mm tt");
                      TransactionDateType transactionDateType;
                    
                      if (date.Date == DateTime.Now.Date)
                      {
                          transactionDateType = TransactionDateType.Today;
                          var a = DateTime.Now.AddDays(-1);
                      }else if (date.Date== DateTime.Now.AddDays(-1).Date)
                      {
                          transactionDateType = TransactionDateType.Yesterday;
                          var tDate=DateTime.ParseExact(i.Date, "d-M-yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                          finaltime = tDate.ToString("ddd MMM dd");
                      }
                      else if (date.Date >= DateTime.Now.AddDays(-7).Date)
                      {
                          transactionDateType = TransactionDateType.Last7Days;
                          var tDate = DateTime.ParseExact(i.Date, "d-M-yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                          finaltime = tDate.ToString("ddd MMM dd");

                      }
                      else if (date.Month == DateTime.Now.Month)
                      {
                          transactionDateType = TransactionDateType.EarlierThisMonth;
                          var tDate = DateTime.ParseExact(i.Date, "d-M-yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                          finaltime = tDate.ToString("ddd MMM dd");
                      }
                      else
                      {
                          transactionDateType = TransactionDateType.PreviousTransactions;
                          var tDate = DateTime.ParseExact(i.Date, "d-M-yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                          finaltime = tDate.ToString("ddd MMM dd");
                      }
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
                          Time= finaltime,
                          TransactionDateType=transactionDateType
                      };
                      transactionHistoryViewModel.AllSortedIndexedTransactions.Add(t);
                      transactionHistoryViewModel.RecipientNameInitials.Add(i.Name.Substring(0, 1));
                  }
                  //group by unique transactiondatetype


                  var query = from i in transactionHistoryViewModel.AllSortedIndexedTransactions
                              group i by i.TransactionDateType into g
                              select new { GroupName = g.Key, Items = g };

                 
                  foreach (var g in query)
                  {
                      GroupInfosList info = new GroupInfosList();

                      //switch case: to show item properly
                      switch (g.GroupName)
                      {
                          case TransactionDateType.Today:
                              info.Key = "Today";
                              break;
                          case TransactionDateType.Yesterday:
                              info.Key = "Yesterday";
                              break;
                          case TransactionDateType.Last7Days:
                              info.Key = "Last 7 days";
                              break;
                          case TransactionDateType.EarlierThisMonth:
                              info.Key = "Earlier this month";
                              break;
                          case TransactionDateType.PreviousTransactions:
                              info.Key = "Previous Transactions";
                              break;

                      }

                      //int n = g.GroupName.Date.Day;
                      //string ordinalSuffix = n % 100 == 11 || n % 100 == 12 || n % 100 == 13 ? "th" : n % 10 == 1 ? "st" : n % 10 == 2 ? "nd" : n % 10 == 3 ? "rd" : "th";
                      //if(g.GroupName.Date == DateTime.Now.Date)
                      //{
                      //    info.Key = "Today ";
                      //}
                      //else if(g.GroupName.Date == DateTime.Now.AddDays(-1))
                      //{
                      //    info.Key = "Yesterday";
                      //}
                      //else if(g.GroupName.Date >= DateTime.Now.AddDays(-7))
                      //{
                      //    info.Key = "Last 7 Days";
                      //}
                      //else if(g.GroupName.Month == DateTime.Now.Month)
                      //{
                      //    info.Key = "Earlier this month";
                      //}
                      //else
                      //{
                      //    info.Key = "Previous Transactions";
                      //}
                     // info.Key = g.GroupName.ToString();
                   
                      //info.Key = g.GroupName.Date.ToString("dd'\u00A0'MMM'\u00A0'yyyy", CultureInfo.InvariantCulture) ;

                      //info.Key = info.Key.Insert(2, ordinalSuffix);
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

        private Visibility _dataTemplateChanged = Visibility.Collapsed;
        public Visibility DataTemplateChanged
        {
            get { return _dataTemplateChanged; }
            set
            {
                _dataTemplateChanged = value;
                OnPropertyChangedAsync(nameof(DataTemplateChanged));

            }
        }



    }

}
