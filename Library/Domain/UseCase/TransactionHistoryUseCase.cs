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


        private ITransactionHistoryDataManager _transactionHistoryDataManager;
        private TransactionHistoryRequest _transactionHistoryRequest;
        IPresenterTransactionHistoryCallback _transactionHistoryResponseCallback;
        public TransactionHistoryUseCase(TransactionHistoryRequest request, IPresenterTransactionHistoryCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            _transactionHistoryDataManager = serviceProviderInstance.Services.GetService<ITransactionHistoryDataManager>();
            _transactionHistoryRequest = request;
            _transactionHistoryResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._transactionHistoryDataManager.GetAllTransactions(_transactionHistoryRequest, new TransactionHistoryCallback(this));
           // this.TransactionHistoryDataManager.ValidateUserLogin(TransactionHistoryRequest, new TransactionHistoryCallback(this));
        }

        public class TransactionHistoryCallback : IUsecaseCallbackBaseCase<TransactionHistoryResponse>
        {
            private TransactionHistoryUseCase _transactionHistory;
            public TransactionHistoryCallback(TransactionHistoryUseCase transactionHistory)
            {
                this._transactionHistory = transactionHistory;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _transactionHistory._transactionHistoryResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<TransactionHistoryResponse> response)
            {
                _transactionHistory._transactionHistoryResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<TransactionHistoryResponse> response)
            {
                _transactionHistory._transactionHistoryResponseCallback?.OnSuccessAsync(response);

            }
        }

        public class TransactionHistoryResponse : ZResponse<AmountTransaction>
        {
            public List<AmountTransaction> AllTransactions;
        }
    }
}
