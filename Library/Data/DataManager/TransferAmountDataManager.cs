using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Util;
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

        private bool ValidateCurrentAccount(Account currentAccount,string Amount)
        {
            if (currentAccount != null)
            {
             
                double AccountBalance = Double.Parse(currentAccount.TotalBalance);
                double amount = Double.Parse(Amount);
                if(AccountBalance>=amount)
                {
                    return true;
                }
            }
            return false;
        }

        public void AddTransaction(TransferAmountRequest request, TransferAmountUseCase.TransferAmountCallback response)
        {
            ZResponse<TransferAmountResponse> Response = new ZResponse<TransferAmountResponse>();

            //conversion of amountTransfer to transaction
            //check for balance  before transaction
            var account = DbHandler.ValidationBeforeTransaction(request.Transaction.FromAccount, request.UserId);
            var status= ValidateCurrentAccount(account,request.Transaction.Amount);
            Transaction currentTransaction = new Transaction
            {
                UserId = request.UserId,
                Name =request.Transaction.Name,
                TransactionId =GenerateUniqueId.GetUniqueId("TID"),
                Date =CurrentDateTime.GetCurrentDate(),
                TransactionType = Model.Enum.TransactionType.Debited,
                Remark =request.Transaction.Remark,
                TransactionAmout =request.Transaction.Amount,
                FromAccount =request.Transaction.FromAccount,
                ToAccount =request.Transaction.ToAccount,
                Status =status,
            };

            var responseTransactions = DbHandler.AddTransaction(currentTransaction);
           // DeductBalance(request.Transaction.Amount);
            TransferAmountResponse transferAmountResponse = new TransferAmountResponse();
            transferAmountResponse.Status = "Successfully added transactions";
            transferAmountResponse.transaction = currentTransaction;
            transferAmountResponse.Data = responseTransactions;
            var responseStatus = "Transaction Processed";
            Response.Response = responseStatus;
            Response.Data = transferAmountResponse;
            response.OnResponseSuccess(Response);
        }
    }

    public class TransferAmountResponse : ZResponse<Transaction>
    {
        public Transaction transaction;
        public string Status { get; set; }
    }
}