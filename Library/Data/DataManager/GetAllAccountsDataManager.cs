using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetAllAccounts;

namespace Library.Data.DataManager
{
    public class GetAllAccountsDataManager : BankingDataManager, IGetAllAccountsDataManager
    {
        public GetAllAccountsDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        void IGetAllAccountsDataManager.GetAllAccounts(GetAllAccountsRequest request, IUsecaseCallbackBaseCase<GetAllAccountsResponse> response)
        {
            //get it frm db
            ZResponse<GetAllAccountsResponse> Response = new ZResponse<GetAllAccountsResponse>();
            GetAllAccountsResponse GetAllAccountsResponse = new GetAllAccountsResponse();

            var userId = request.UserId;
            var currentAccountForUser = DbHandler.GetAllAccountsForUser(userId);
            var tempallAccountBalance = DbHandler.GetAllAccountBalance(userId);
            List<Account> allCurrentAccounts = new List<Account>();
            List<AccountBalance> allCurrentAccountsBalance = new List<AccountBalance>();
         
            foreach (var accNo in currentAccountForUser)
            {
                allCurrentAccounts.Add(DbHandler.GetAccount(accNo));
            }
            foreach(var i in tempallAccountBalance)
            {
                var temp = new AccountBalance
                {
                    AccountNumber = i.Key,
                    TotalBalance = i.Value
                };
                allCurrentAccountsBalance.Add(temp);

            }

            GetAllAccountsResponse.allAccountBalance=allCurrentAccountsBalance;
            //var allCurrentAccounts = DbHandler.GetAllAccounts(userId);

            GetAllAccountsResponse.allAccount = allCurrentAccounts;
            Response.Data = GetAllAccountsResponse;
            Response.Response = "Successfully got all accounts";
            response.OnResponseSuccess(Response);

        }
    }

}
