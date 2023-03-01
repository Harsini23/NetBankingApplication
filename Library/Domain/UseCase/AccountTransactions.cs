using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.AccountTransactions;

namespace Library.Domain.UseCase
{
    public interface IAccountTransactionsDataManager
    {
        void GetAllTransactions(AccountTransactionsRequest request, IUsecaseCallbackBaseCase<AccountTransactionsResponse> response);//call back
    }

    public class AccountTransactionsRequest : IRequest
    {
        public string AccountNumber { get; set; }
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public AccountTransactionsRequest(string accountNumber,string userId,CancellationTokenSource cancellationTokenSource)
        {
            AccountNumber = accountNumber;
            UserId = userId;
            CtsSource = cancellationTokenSource;
        }
    }

    public interface IPresenterAccountTransactionsCallback : IResponseCallbackBaseCase<AccountTransactionsResponse>
    {
       // void ExectueInMainThread(ZResponse<AccountTransactionsResponse> response);
    }
    public class AccountTransactions: UseCaseBase<AccountTransactionsResponse>
    {


        private IAccountTransactionsDataManager AccountTransactionsDataManager;
        private AccountTransactionsRequest AccountTransactionsRequest;
        IPresenterAccountTransactionsCallback AccountTransactionsResponse;
        public AccountTransactions(AccountTransactionsRequest request, IPresenterAccountTransactionsCallback responseCallback)
        {
            AccountTransactionsDataManager = ServiceProvider.GetInstance().Services.GetService<IAccountTransactionsDataManager>();
            AccountTransactionsRequest = request;
            AccountTransactionsResponse = responseCallback;
        }
        public override void Action()
        {
            //use call back
       
            this.AccountTransactionsDataManager.GetAllTransactions(AccountTransactionsRequest, new AccountTransactionsCallback(this));
            // this.AccountTransactionsDataManager.ValidateUserLogin(AccountTransactionsRequest, new AccountTransactionsCallback(this));
        }

        public class AccountTransactionsCallback : IUsecaseCallbackBaseCase<AccountTransactionsResponse>
        {
            private AccountTransactions AccountTransactions;
            public AccountTransactionsCallback(AccountTransactions AccountTransactions)
            {
                this.AccountTransactions = AccountTransactions;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                AccountTransactions.AccountTransactionsResponse?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<AccountTransactionsResponse> response)
            {
                AccountTransactions.AccountTransactionsResponse?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<AccountTransactionsResponse> response)
            {
               AccountTransactions.AccountTransactionsResponse?.OnSuccessAsync(response);
              //  AccountTransactions.AccountTransactionsResponse?.ExectueInMainThread(response);

            }
        }
    }


    public class AccountTransactionsResponse : ZResponse<AccountTransactionBObj>
    {
        public List<AccountTransactionBObj> allTransactions;
        public Account account;
    }
}
