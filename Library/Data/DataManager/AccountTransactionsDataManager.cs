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
    public class AccountTransactionsDataManager: BankingDataManager, IAccountTransactionsDataManager
    {
        public AccountTransactionsDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base( DbHandler,NetHandler)
        {
        }


        public void GetAllTransactions(AccountTransactionsRequest request, IUsecaseCallbackBaseCase<AccountTransactionsResponse> callback)
        {
            List<AccountTransactionBObj> allAccountTransactions = new List<AccountTransactionBObj>();
            var Alltransactions=DbHandler.GetTransactionsForAccount(request.AccountNumber);
            Dictionary<String,String> UserMapping = new Dictionary<String,String>();
            List<String> uniqueId = Alltransactions.Select(i=>i.UserId).Distinct().ToList();
         
            foreach(var i in uniqueId)
            {
                UserMapping.Add(i, DbHandler.GetUserName(i));
            }
            foreach(var i in Alltransactions)
            {
                string account;
                string picPath;
                if (i.TransactionType == Model.Enum.TransactionType.Credited)
                {
                    account = i.FromAccount;
                }
                else
                {
                    account = i.ToAccount;
                }
                var path= DbHandler.GetProfile(i.UserId);
                if (path != null)
                {
                    picPath=path;
                }
                AccountTransactionBObj trasaction = new AccountTransactionBObj
                {
                    UserName = UserMapping[i.UserId],
                    AccountNumber = account,
                    TransactionType = i.TransactionType,
                    Amount=i.Amount,
                    DateOfTransaction=i.Date,
                    Initial = UserMapping[i.UserId].Substring(0, 1).ToUpper(),
                    Transaction=i,
                    PersonPicPath=path
                };
                allAccountTransactions.Add(trasaction);
            }

            var accountDetails = DbHandler.GetAccount(request.AccountNumber);
            ZResponse<AccountTransactionsResponse> response = new ZResponse<AccountTransactionsResponse>();
            AccountTransactionsResponse transactions = new AccountTransactionsResponse();
            transactions.account = accountDetails;
            transactions.allTransactions = allAccountTransactions;
            response.Data = transactions;
            response.Response = "Added all transactions of that account";

            callback?.OnResponseSuccess(response);
        }
    }

    
}
