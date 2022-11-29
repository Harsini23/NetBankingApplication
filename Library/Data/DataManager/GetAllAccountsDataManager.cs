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
    public class GetAllAccountsDataManager : BankingDataManager, IGetAllAccountsDataManager
    {
        public GetAllAccountsDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        void IGetAllAccountsDataManager.GetAllAccounts(GetAllAccountsRequest request, GetAllAccounts.GetAllAccountsCallback response)
        {
            //get it frm db
            ZResponse<GetAllAccountsResponse> Response = new ZResponse<GetAllAccountsResponse>();
            GetAllAccountsResponse GetAllAccountsResponse = new GetAllAccountsResponse();

            var userId = request.UserId;
            var currentAccountForUser = DbHandler.GetAllAccountsForUser(userId);
            List<Account> allCurrentAccounts = new List<Account>();
            foreach (var accNo in currentAccountForUser)
            {
                allCurrentAccounts.Add(DbHandler.GetAccount(accNo));
            }

            //var allCurrentAccounts = DbHandler.GetAllAccounts(userId);

            GetAllAccountsResponse.allAccount = allCurrentAccounts;
            Response.Data = GetAllAccountsResponse;
            Response.Response = "Successfully got all accounts";
            response.OnResponseSuccess(Response);

        }
    }



    public class GetAllAccountsResponse : ZResponse<Account>
    {
        public List<Account> allAccount;
    }
}
