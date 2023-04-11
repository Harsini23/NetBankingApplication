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

        public bool ValidateCurrentAccountAndDeductBalance(Account currentAccount, double Amount)
        {
            if (currentAccount != null)
            {
                var Account = DbHandler.GetAccount(currentAccount.AccountNumber);
                var AccountBalance = Account.TotalBalance;
                double amount =Amount;
                if (AccountBalance >= amount)
                {
                    double deductedValue = AccountBalance - amount;
                    //deduct amount from account
                    Account account = new Account
                    {
                        //UserId = currentAccount.UserId,
                        AccountNumber = Account.AccountNumber,
                        AccountType = Account.AccountType,//fd account or existing account
                        AvailableBalanceAsOn = CurrentDateTime.GetCurrentDate(),
                        BId = Account.BId,
                        Currency = Account.Currency,
                        TotalBalance = deductedValue
                    };
                    var res = DbHandler.UpdateBalance(account);
                    return res;
                }

            }
            return false;
        }

        public void AddTransaction(TransferAmountRequest request, IUsecaseCallbackBaseCase<TransferAmountResponse> callback)
        {
            ZResponse<TransferAmountResponse> response = new ZResponse<TransferAmountResponse>();
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
            AmountTransaction currentTransaction = new AmountTransaction
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
            AmountTransaction responseTransactions =new AmountTransaction();
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
                transferAmountResponse.Transaction = currentTransaction;
                transferAmountResponse.Data = responseTransactions;
                var responseStatus = "Transaction Processed";
                response.Response = responseStatus;
                response.Data = transferAmountResponse;
                callback.OnResponseSuccess(response);
            }
            else
            {
                transferAmountResponse.Status = "Transaction failed, added transactions";
                transferAmountResponse.Transaction = currentTransaction;
                transferAmountResponse.Data = responseTransactions;
                var responseStatus = "Transaction failed due to insuficient balance";
                response.Response = responseStatus;
                response.Data = transferAmountResponse;
                callback.OnResponseSuccess(response);
            }

        }
    }

   
}