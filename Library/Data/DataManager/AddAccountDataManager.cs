using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class AddAccountDataManager : BankingDataManager, IAddAccountDataManager
    {
        public AddAccountDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {

        }

        public string PopulateDataForNewAccountCreation(AddAccountRequest request,TransactionType transactionType)
        {
            var GeneratedAccountNumber = GenerateUniqueId.RandomNumber(100000000, 999999999).ToString() + GenerateUniqueId.RandomNumber(100, 999).ToString();
            var userAccount = CreateUserAccount(request.newAccount.UserId, GeneratedAccountNumber);
            var currentTransaction = CreateTransaction(request.newAccount, GeneratedAccountNumber,transactionType);
            request.newAccount.AccountNumber = GeneratedAccountNumber;
            var account = request.newAccount;
            DbHandler.AddAccount(account);
            DbHandler.AddAccountForUser(userAccount);
            DbHandler.AddTransaction(currentTransaction);
            return GeneratedAccountNumber;
        }

        public void AddAccount(AddAccountRequest request, IUsecaseCallbackBaseCase<bool> response)
        {
            ZResponse<bool> Response = new ZResponse<bool>();
            PopulateDataForNewAccountCreation(request,TransactionType.Credited);
            Response.Response = "Sucessfully added account";
            Response.Data = true;
            response?.OnResponseSuccess(Response);
        }

        private UserAccounts CreateUserAccount(string userId,string AccountNo)
        {
            return new UserAccounts
            {
                UserId = userId,
                AccountNumber = AccountNo,
            };
        }
        private Transaction CreateTransaction(AccountBObj account,string accNo, TransactionType transactionType= TransactionType.Credited)
        {
            return  new Transaction
            {
                UserId = account.UserId,
                Name = account.Name,
                TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                Date = CurrentDateTime.GetCurrentDate(),
                TransactionType = transactionType,
                Remark = "Initial deposit",
                Amount = account.TotalBalance,
                FromAccount = "-",
                ToAccount = accNo,
                Status = true,
            };
         

        }
    }
}
