using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.TransferAmountUseCase;

namespace Library.Domain.UseCase
{

    public interface ITransferAmountDataManager
    {
        void AddTransaction(TransferAmountRequest request, IUsecaseCallbackBaseCase<TransferAmountResponse> response);//call back
    }
    public class TransferAmountRequest : IRequest
    {
        public AmountTransfer Transaction { get; set; }
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }

        public TransferAmountRequest(AmountTransfer transaction, string userId,CancellationTokenSource cts)
        {
            Transaction = transaction;
            UserId = userId;
            CtsSource = cts;
        }
    }

    public interface IPresenterTransferAmountCallback: IResponseCallbackBaseCase<TransferAmountResponse>
    {
    }
    public class TransferAmountUseCase : UseCaseBase<TransferAmountResponse>
    {


        private ITransferAmountDataManager _transferAmountDataManager;
        private TransferAmountRequest _transferAmountRequest;
        IPresenterTransferAmountCallback _transferAmountResponseCallback;
        public TransferAmountUseCase(TransferAmountRequest request, IPresenterTransferAmountCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            _transferAmountDataManager = serviceProviderInstance.Services.GetService<ITransferAmountDataManager>();
            _transferAmountRequest = request;
            _transferAmountResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this._transferAmountDataManager.AddTransaction(_transferAmountRequest, new TransferAmountCallback(this));
        }
        public class TransferAmountCallback : IUsecaseCallbackBaseCase<TransferAmountResponse>
        {
            private TransferAmountUseCase _transferAmountUseCase;
            public TransferAmountCallback(TransferAmountUseCase transferAmountUseCase)
            {
                this._transferAmountUseCase = transferAmountUseCase;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _transferAmountUseCase._transferAmountResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<TransferAmountResponse> response)
            {
                _transferAmountUseCase._transferAmountResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<TransferAmountResponse> response)
            {
                _transferAmountUseCase._transferAmountResponseCallback?.OnSuccessAsync(response);
            }
        }

        public class TransferAmountResponse : ZResponse<AmountTransaction>
        {
            public AmountTransaction Transaction;
            public string Status { get; set; }
        }
    }
}
