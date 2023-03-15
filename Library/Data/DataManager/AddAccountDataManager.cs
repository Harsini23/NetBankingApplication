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
        public void AddAccount(AddAccountRequest request, IUsecaseCallbackBaseCase<bool> response)
        {
            ZResponse<bool> Response = new ZResponse<bool>();
            var GeneratedAccountNumber = GenerateUniqueId.RandomNumber(100000000, 999999999).ToString() + GenerateUniqueId.RandomNumber(100, 999).ToString();
            var account = CreateAccount(request.newAccount,GeneratedAccountNumber);
            var userAccount = CreateUserAccount(request.newAccount.UserId,GeneratedAccountNumber);
            var currentTransaction = CreateTransaction(request.newAccount,GeneratedAccountNumber);
            DbHandler.AddAccount(account);
            DbHandler.AddAccountForUser(userAccount);
            DbHandler.AddTransaction(currentTransaction);

            Response.Response = "Sucessfully added account";
            Response.Data = true;
            response?.OnResponseSuccess(Response);

        }

        private Account CreateAccount(AccountBObj account,string accountNo)
        {
            return new Account
            {
                AccountType = account.AccountType,
                TotalBalance = account.TotalBalance,
                AccountNumber = accountNo,
                BId = account.BId,
                Currency = account.Currency,
                AvailableBalanceAsOn = CurrentDateTime.GetCurrentDate()
            };
        }
        private UserAccounts CreateUserAccount(string userId,string AccountNo)
        {
            return new UserAccounts
            {
                UserId = userId,
                AccountNumber = AccountNo,
            };
        }
        private Transaction CreateTransaction(AccountBObj account,string accNo)
        {
            return  new Transaction
            {
                UserId = account.UserId,
                Name = account.Name,
                TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                Date = CurrentDateTime.GetCurrentDate(),
                TransactionType = TransactionType.Credited,
                Remark = "Initial deposit",
                Amount = account.TotalBalance,
                FromAccount = "-",
                ToAccount = accNo,
                Status = true,
            };
         

        }
    }
}
