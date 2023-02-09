﻿using Library.Data.DataManager;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.TransactionHistoryUseCase;

namespace Library.Domain.UseCase
{

    public interface ITransactionHistoryDataManager
    {
        void GetAllTransactions(TransactionHistoryRequest request, TransactionHistoryCallback response);//call back
    }

    public class TransactionHistoryRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }

        public TransactionHistoryRequest(string userId,CancellationTokenSource cts)
        {
            UserId = userId;
            CtsSource = cts;
        }
    }

    public interface IPresenterTransactionHistoryCallback: IResponseCallbackBaseCase<TransactionHistoryResponse>
    {
    }
    public class TransactionHistoryUseCase:UseCaseBase<TransactionHistoryResponse>
    {


        private ITransactionHistoryDataManager TransactionHistoryDataManager;
        private TransactionHistoryRequest TransactionHistoryRequest;
        IPresenterTransactionHistoryCallback TransactionHistoryResponse;
        public TransactionHistoryUseCase(TransactionHistoryRequest request, IPresenterTransactionHistoryCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            TransactionHistoryDataManager = serviceProviderInstance.Services.GetService<ITransactionHistoryDataManager>();
            TransactionHistoryRequest = request;
            TransactionHistoryResponse = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this.TransactionHistoryDataManager.GetAllTransactions(TransactionHistoryRequest, new TransactionHistoryCallback(this));
           // this.TransactionHistoryDataManager.ValidateUserLogin(TransactionHistoryRequest, new TransactionHistoryCallback(this));
        }

        public class TransactionHistoryCallback : ZResponse<TransactionHistoryResponse>
        {
            private TransactionHistoryUseCase transactionHistory;
            public TransactionHistoryCallback(TransactionHistoryUseCase transactionHistory)
            {
                this.transactionHistory = transactionHistory;
            }
            public string Response { get; set; }

            public void OnResponseError(String response)
            {
                transactionHistory.TransactionHistoryResponse?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<TransactionHistoryResponse> response)
            {
                transactionHistory.TransactionHistoryResponse?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<TransactionHistoryResponse> response)
            {
                transactionHistory.TransactionHistoryResponse?.OnSuccess(response);

            }
        }
    }
}
