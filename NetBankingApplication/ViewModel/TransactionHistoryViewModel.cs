﻿using Library.Data.DataManager;
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


    public class TransactionHistoryViewModel : TransactionBaseViewModel
    {
        TransactionHistoryUseCase Transaction;
        public override void GetTransactionData()
        {
            //load the observable collection frm db
            //for (int i = 2; i < 20; i++)
            //{
            //    AllTransactions.Add(new Transaction
            //    {
            //        UserId = "Harsh",
            //        TransactionId = "T0000" + i,
            //        Date = "21-11-2022",
            //        TransactionType = (Library.Model.Enum.TransactionType)1,
            //        Remark = "Outing",
            //        TransactionAmout = "20" + i * 200,
            //        FromAccount = "89036457389231",
            //        ToAccount = "89036457389234",
            //        Status = true

            //    });
            //}

            Transaction = new TransactionHistoryUseCase(new TransactionHistoryRequest("Harsh"), new PresenterTransactionHistoryCallback(this));
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

        public void OnError(ZResponse<TransactionHistoryResponse> response)
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


        public async void populateData( List<Transaction> TransactionList)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  transactionHistoryViewModel.AllTransactions.Clear();
                  foreach (var i in TransactionList)
                  {
                      transactionHistoryViewModel.AllTransactions.Add(i);
                  }
              });
                
        }
    }


    public abstract class TransactionBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<Transaction> AllTransactions = new ObservableCollection<Transaction>();
        public abstract void GetTransactionData();
        public List<Transaction> AllTransactionList= new List<Transaction>(){};
    }
}
