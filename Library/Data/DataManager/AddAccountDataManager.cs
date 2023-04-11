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
            var generatedAccountNumber = GenerateUniqueId.RandomNumber(100000000, 999999999).ToString() + GenerateUniqueId.RandomNumber(100, 999).ToString();
            var userAccount = CreateUserAccount(request.NewAccount.UserId, generatedAccountNumber);
            var currentTransaction = CreateTransaction(request.NewAccount, generatedAccountNumber,transactionType);
            request.NewAccount.AccountNumber = generatedAccountNumber;
            var account = request.NewAccount;
            DbHandler.AddAccount(account);
            BankingNotification.BankingNotification.NotifyAccountUpdated(account);

            DbHandler.AddAccountForUser(userAccount);
            DbHandler.AddTransaction(currentTransaction);
            return generatedAccountNumber;
        }

        public void AddAccount(AddAccountRequest request, IUsecaseCallbackBaseCase<bool> callback)
        {
            ZResponse<bool> response = new ZResponse<bool>();
            PopulateDataForNewAccountCreation(request,TransactionType.Credited);
            response.Response = "Sucessfully added account";
            response.Data = true;
            callback?.OnResponseSuccess(response);
        }

        private UserAccounts CreateUserAccount(string userId,string AccountNo)
        {
            return new UserAccounts
            {
                UserId = userId,
                AccountNumber = AccountNo,
            };
        }
        private AmountTransaction CreateTransaction(AccountBObj account,string accNo, TransactionType transactionType= TransactionType.Credited)
        {
            return  new AmountTransaction
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
