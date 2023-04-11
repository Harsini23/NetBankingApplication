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

        void IGetAllAccountsDataManager.GetAllAccounts(GetAllAccountsRequest request, IUsecaseCallbackBaseCase<GetAllAccountsResponse> callback)
        {
            //get it frm db
            ZResponse<GetAllAccountsResponse> response = new ZResponse<GetAllAccountsResponse>();
            GetAllAccountsResponse getAllAccountsResponse = new GetAllAccountsResponse();

            var userId = request.UserId;
            var allCurrentAccounts = DbHandler.GetAllAccountsForUser(userId,request.GetOnlyTransferAccounts);
            var tempallAccountBalance = DbHandler.GetAllAccountBalance(userId);
            List<AccountBalance> allCurrentAccountsBalance = tempallAccountBalance.Select(accountBalance => new AccountBalance { AccountNumber=accountBalance.Key,TotalBalance=accountBalance.Value}).ToList();

            getAllAccountsResponse.AllAccountBalance=allCurrentAccountsBalance;

            getAllAccountsResponse.AllAccount = allCurrentAccounts;
            response.Data = getAllAccountsResponse;
            response.Response = "Successfully got all accounts";
            callback.OnResponseSuccess(response);

        }
    }

}
