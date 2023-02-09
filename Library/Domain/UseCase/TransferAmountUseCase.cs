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
        void AddTransaction(TransferAmountRequest request, TransferAmountCallback response);//call back
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


        private ITransferAmountDataManager TransferAmountDataManager;
        private TransferAmountRequest TransferAmountRequest;
        IPresenterTransferAmountCallback TransferAmountResponse;
        public TransferAmountUseCase(TransferAmountRequest request, IPresenterTransferAmountCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            TransferAmountDataManager = serviceProviderInstance.Services.GetService<ITransferAmountDataManager>();
            TransferAmountRequest = request;
            TransferAmountResponse = responseCallback;
        }
        public override void Action()
        {
            this.TransferAmountDataManager.AddTransaction(TransferAmountRequest, new TransferAmountCallback(this));
        }
        public class TransferAmountCallback : ZResponse<TransferAmountResponse>
        {
            private TransferAmountUseCase transferAmountUseCase;
            public TransferAmountCallback(TransferAmountUseCase transferAmountUseCase)
            {
                this.transferAmountUseCase = transferAmountUseCase;
            }
            public string Response { get; set; }

            public void OnResponseError(String response)
            {
                transferAmountUseCase.TransferAmountResponse?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<TransferAmountResponse> response)
            {
                transferAmountUseCase.TransferAmountResponse?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<TransferAmountResponse> response)
            {
                transferAmountUseCase.TransferAmountResponse?.OnSuccess(response);
            }
        }
    }
}
