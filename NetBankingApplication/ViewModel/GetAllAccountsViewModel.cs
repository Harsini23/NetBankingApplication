using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class GetAllAccountsViewModel : GetAllAccountsBaseViewModel
    {
        GetAllAccounts getAllAccounts;
        public override void GetAllAccounts(string userId)
        {
            getAllAccounts = new GetAllAccounts(new GetAllAccountsRequest(userId),  new PresenterGetAllAccountsCallback(this));
            getAllAccounts.Execute();
        }
    }

    public class PresenterGetAllAccountsCallback : IPresenterGetAllAccountsCallback
    {
        private GetAllAccountsViewModel GetAllAccountsViewModel;
        public PresenterGetAllAccountsCallback()
        {

        }
        public PresenterGetAllAccountsCallback(GetAllAccountsViewModel getAllAccountsViewModel)
        {
            this.GetAllAccountsViewModel = getAllAccountsViewModel;
        }

        public void OnError(ZResponse<GetAllAccountsResponse> response)
        {
        }

        public void OnFailure(ZResponse<GetAllAccountsResponse> response)
        {

        }

        public void OnSuccess(ZResponse<GetAllAccountsResponse> response)
        {
            var allAccounts = response.Data.allAccount;
            GetAllAccountsViewModel.AllAccounts.Clear();
            GetAllAccountsViewModel.AllAccountNumbers.Clear();

            GetAllAccountsViewModel.AllAccounts = allAccounts;
            foreach(var i in response.Data.allAccount)
            {
                GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
              //  GetAllAccountsViewModel.AllAccounts.Add(i);
            
            }
        }
    }
    public abstract class GetAllAccountsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetAllAccounts(string userId);
        public List<Account> AllAccounts = new List<Account>();
        public List<String> AllAccountNumbers = new List<String>();
    }
}
