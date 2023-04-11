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
        public TransactionHistoryDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void GetAllTransactions(TransactionHistoryRequest request, IUsecaseCallbackBaseCase<TransactionHistoryResponse> callback)
        {
            //get it frm db
            ZResponse<TransactionHistoryResponse> response = new ZResponse<TransactionHistoryResponse>();
            
            var userId = request.UserId;
            List<AmountTransaction> allTransactions= DbHandler.GetAllTransactions(userId,request.ShowOnlyRecentTransactions);
           
            TransactionHistoryResponse transactionHistoryResponse = new TransactionHistoryResponse();
            transactionHistoryResponse.AllTransactions = allTransactions;
            response.Data = transactionHistoryResponse;
            var responseStatus = "Successfull got all transactions";
            response.Response = responseStatus;

            callback.OnResponseSuccess(response);

        }
    }

   
}
