using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class TransferAmountDataManager : BankingDataManager, ITransferAmountDataManager
    {
        public TransferAmountDataManager() : base(new DbHandler(),new NetHandler())
        {
        }

        public void AddTransaction(TransferAmountRequest request, TransferAmountUseCase.TransferAmountCallback response)
        {
            ZResponse<TransferAmountResponse> Response = new ZResponse<TransferAmountResponse>();

            //var userId = request.UserId;
            var responseTransactions = DbHandler.AddTransaction(request.Transaction);
            TransferAmountResponse transferAmountResponse = new TransferAmountResponse();
            transferAmountResponse.Status = "Successfully added transactions";
            transferAmountResponse.Data= responseTransactions;
            var responseStatus = "Successfull got back Trnasaction";
            Response.Response = responseStatus;
            response.OnResponseSuccess(Response);
        }
    }

    public class TransferAmountResponse : ZResponse<Transaction>
    {
        public string Status { get; set; }
    }
}
