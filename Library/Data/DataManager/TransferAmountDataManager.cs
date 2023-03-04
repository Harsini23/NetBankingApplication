using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.TransferAmountUseCase;

namespace Library.Data.DataManager
{
    public class TransferAmountDataManager : BankingDataManager, ITransferAmountDataManager
    {
        public TransferAmountDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        private TransactionType transactionType;

        private bool ValidateCurrentAccountAndDeductBalance(Account currentAccount, double Amount)
        {
            if (currentAccount != null)
            {
                double AccountBalance = currentAccount.TotalBalance;
                double amount =Amount;
                if (AccountBalance >= amount)
                {
                    double deductedValue = AccountBalance - amount;
                    //deduct amount from account
                    Account account = new Account
                    {
                        //UserId = currentAccount.UserId,
                        AccountNumber = currentAccount.AccountNumber,
                        AccountType = currentAccount.AccountType,
                        AvailableBalanceAsOn = CurrentDateTime.GetCurrentDate(),
                        BId = currentAccount.BId,
                        Currency = currentAccount.Currency,
                        TotalBalance = deductedValue
                    };
                    var res = DbHandler.UpdateBalance(account);
                    return res;
                }

            }
            return false;
        }

        public void AddTransaction(TransferAmountRequest request, IUsecaseCallbackBaseCase<TransferAmountResponse> response)
        {
            ZResponse<TransferAmountResponse> Response = new ZResponse<TransferAmountResponse>();
            //conversion of amountTransfer to transaction
            //check for balance  before transaction
            var account = DbHandler.GetAccount(request.Transaction.FromAccount);
            var status = ValidateCurrentAccountAndDeductBalance(account, request.Transaction.Amount);
            if (status)
            {
                transactionType = Model.Enum.TransactionType.Debited;
            }
            else
            {
                transactionType = Model.Enum.TransactionType.Rejected;
            }
            Transaction currentTransaction = new Transaction
            {
                UserId = request.UserId,
                Name = request.Transaction.Name,
                TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                Date = CurrentDateTime.GetCurrentDate(),
                TransactionType = transactionType,
                Remark = request.Transaction.Remark,
                Amount = request.Transaction.Amount,
                FromAccount = request.Transaction.FromAccount,
                ToAccount = request.Transaction.ToAccount,
                Status = status,
            };
            Transaction responseTransactions=new Transaction();
            var TresponseStatus = DbHandler.AddTransaction(currentTransaction);
            if (TresponseStatus)
            {
                responseTransactions = currentTransaction;
            }
            // DeductBalance(request.Transaction.Amount);
            TransferAmountResponse transferAmountResponse = new TransferAmountResponse();
            if (status)
            {
                transferAmountResponse.Status = "Successfully added transactions";
                transferAmountResponse.transaction = currentTransaction;
                transferAmountResponse.Data = responseTransactions;
                var responseStatus = "Transaction Processed";
                Response.Response = responseStatus;
                Response.Data = transferAmountResponse;
                response.OnResponseSuccess(Response);
            }
            else
            {
                transferAmountResponse.Status = "Transaction failed, added transactions";
                transferAmountResponse.transaction = currentTransaction;
                transferAmountResponse.Data = responseTransactions;
                var responseStatus = "Transaction failed due to insuficient balance";
                Response.Response = responseStatus;
                Response.Data = transferAmountResponse;
                response.OnResponseSuccess(Response);
            }

        }
    }

   
}