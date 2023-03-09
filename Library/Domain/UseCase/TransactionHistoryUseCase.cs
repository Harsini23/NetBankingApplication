using Library.Data.DataManager;
using Library.Model;
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
        void GetAllTransactions(TransactionHistoryRequest request, IUsecaseCallbackBaseCase<TransactionHistoryResponse> response);//call back
    }

    public class TransactionHistoryRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }
        public bool ShowOnlyRecentTransactions { get; set; }

        public TransactionHistoryRequest(string userId,CancellationTokenSource cts, bool showOnlyRecentTransactions)
        {
            UserId = userId;
            CtsSource = cts;
            ShowOnlyRecentTransactions = showOnlyRecentTransactions;
        }
    }

    public interface IPresenterTransactionHistoryCallback: IResponseCallbackBaseCase<TransactionHistoryResponse>
    {
    }
    public class TransactionHistoryUseCase:UseCaseBase<TransactionHistoryResponse>
    {


        private ITransactionHistoryDataManager TransactionHistoryDataManager;
        private TransactionHistoryRequest TransactionHistoryRequest;
        IPresenterTransactionHistoryCallback TransactionHistoryResponseCallback;
        public TransactionHistoryUseCase(TransactionHistoryRequest request, IPresenterTransactionHistoryCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            TransactionHistoryDataManager = serviceProviderInstance.Services.GetService<ITransactionHistoryDataManager>();
            TransactionHistoryRequest = request;
            TransactionHistoryResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this.TransactionHistoryDataManager.GetAllTransactions(TransactionHistoryRequest, new TransactionHistoryCallback(this));
           // this.TransactionHistoryDataManager.ValidateUserLogin(TransactionHistoryRequest, new TransactionHistoryCallback(this));
        }

        public class TransactionHistoryCallback : IUsecaseCallbackBaseCase<TransactionHistoryResponse>
        {
            private TransactionHistoryUseCase transactionHistory;
            public TransactionHistoryCallback(TransactionHistoryUseCase transactionHistory)
            {
                this.transactionHistory = transactionHistory;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                transactionHistory.TransactionHistoryResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<TransactionHistoryResponse> response)
            {
                transactionHistory.TransactionHistoryResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<TransactionHistoryResponse> response)
            {
                transactionHistory.TransactionHistoryResponseCallback?.OnSuccessAsync(response);

            }
        }

        public class TransactionHistoryResponse : ZResponse<Transaction>
        {
            public List<Transaction> allTransactions;
        }
    }
}
