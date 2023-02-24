using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.TransactionHistoryUseCase;

namespace Library.Data.DataManager
{
    public class TransactionHistoryDataManager : BankingDataManager, ITransactionHistoryDataManager
    {
        public TransactionHistoryDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        public void GetAllTransactions(TransactionHistoryRequest request, IUsecaseCallbackBaseCase<TransactionHistoryResponse> response)
        {
            //get it frm db
            ZResponse<TransactionHistoryResponse> Response = new ZResponse<TransactionHistoryResponse>();

            var userId = request.UserId;
            var allTransactions = DbHandler.GetAllTransactions(userId);
            TransactionHistoryResponse transactionHistoryResponse = new TransactionHistoryResponse();
            transactionHistoryResponse.allTransactions = allTransactions;
            Response.Data = transactionHistoryResponse;
            var responseStatus = "Successfull got all transactions";
            Response.Response = responseStatus;

            response.OnResponseSuccess(Response);

        }
    }

   
}
