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
using static Library.Domain.UseCase.TransactionHistoryUseCase;

namespace NetBankingApplication.ViewModel
{


    public class TransactionHistoryViewModel : TransactionHistoryBaseViewModel
    {
       private TransactionHistoryUseCase _transaction;
        public override void GetTransactionData(string userId, bool showOnlyRecentTransactions = false)
        {
            _transaction = new TransactionHistoryUseCase(new TransactionHistoryRequest(userId, new CancellationTokenSource(), showOnlyRecentTransactions), new PresenterTransactionHistoryCallback(this));
            _transaction.Execute();

        }
    }


    public class PresenterTransactionHistoryCallback : IPresenterTransactionHistoryCallback
    {
        private TransactionHistoryViewModel _transactionHistoryViewModel;
        public PresenterTransactionHistoryCallback()
        {

        }
        public PresenterTransactionHistoryCallback(TransactionHistoryViewModel transactionHistoryViewModel)
        {
            this._transactionHistoryViewModel = transactionHistoryViewModel;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<TransactionHistoryResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<TransactionHistoryResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                populateData(response.Data.allTransactions);
            });

        }

        //newContacts = Contacts.OrderBy(c => c.FirstName);
        //    ContactSort.Clear();
        //    foreach (var item in newContacts)
        //        ContactSort.Add(item);
        public async void populateData(List<AmountTransaction> TransactionList)
        {

            int temp = 0;

            if (TransactionList.Count <= 0)
            {
                _transactionHistoryViewModel.TextBoxVisibility = Visibility.Visible;
                return;
            }

            _transactionHistoryViewModel.TextBoxVisibility = Visibility.Collapsed;
            _transactionHistoryViewModel.AllSortedTransactions.Clear();
            _transactionHistoryViewModel.AllSortedIndexedTransactions.Clear();
            _transactionHistoryViewModel.FinalSortedIndexedTransactions.Clear();

            var AllTransactions = TransactionList.OrderByDescending(i => DateTime.Parse(i.Date));
            _transactionHistoryViewModel.RecipientNameInitials.Clear();
            foreach (var i in AllTransactions)
            {

                _transactionHistoryViewModel.AllSortedTransactions.Add(i);
                var date = DateTime.Parse(i.Date);
                DateTime time = DateTime.ParseExact(date.TimeOfDay.ToString(), "HH:mm:ss", CultureInfo.InvariantCulture);
                var finaltime = time.ToString("hh:mm tt");
                TransactionDateType transactionDateType;

                if (date.Date == DateTime.Now.Date)
                {
                    transactionDateType = TransactionDateType.Today;
                    var a = DateTime.Now.AddDays(-1);
                }
                else if (date.Date == DateTime.Now.AddDays(-1).Date)
                {
                    transactionDateType = TransactionDateType.Yesterday;
                    var tDate = DateTime.ParseExact(i.Date, "d-M-yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
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
                    Time = finaltime,
                    TransactionDateType = transactionDateType
                };
                _transactionHistoryViewModel.AllSortedIndexedTransactions.Add(t);
                if ( String.IsNullOrEmpty(i.Name))
                {
                    _transactionHistoryViewModel.RecipientNameInitials.Add(i.Name.Substring(0, 1));
                }
            }
            //group by unique transactiondatetype


            var query = from i in _transactionHistoryViewModel.AllSortedIndexedTransactions
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
                info.Count = g.Items.Count();


                foreach (var item in g.Items)
                {
                    info.Add(item);
                }
                _transactionHistoryViewModel.FinalSortedIndexedTransactions.Add(info);
            }
        }
    }


    public abstract class TransactionHistoryBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<AmountTransaction> AllSortedTransactions = new ObservableCollection<AmountTransaction>();
        public ObservableCollection<TransactionBObj> AllSortedIndexedTransactions = new ObservableCollection<TransactionBObj>();
        public ObservableCollection<GroupInfosList> FinalSortedIndexedTransactions = new ObservableCollection<GroupInfosList>();
        //public abstract void GetTransactionData(string UserId);
        public abstract void GetTransactionData(string UserId, bool showOnlyRecentTransactions);
        public List<AmountTransaction> AllTransactionList = new List<AmountTransaction>() { };

        public List<String> RecipientNameInitials = new List<string>();

        private string _error = String.Empty;


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

        private Visibility _dataTemplateChanged = Visibility.Collapsed;
        public Visibility DataTemplateChanged
        {
            get { return _dataTemplateChanged; }
            set
            {
                _dataTemplateChanged = value;
                OnPropertyChanged(nameof(DataTemplateChanged));

            }
        }



    }

}
