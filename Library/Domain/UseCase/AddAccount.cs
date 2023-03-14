using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.UseCase
{
    public interface IAddAccountDataManager
    {
        void AddAccount(AddAccountRequest request, IUsecaseCallbackBaseCase<bool> response);
    }
    public class AddAccount : UseCaseBase<bool>
    {
        private IAddAccountDataManager AddAccountDataManager;
        private AddAccountRequest addAccountRequest;
        IPresenterAddAccountCallback presenterAddAccountCallback;
        public AddAccount(AddAccountRequest request,IPresenterAddAccountCallback responseCallback)
        {
            AddAccountDataManager = ServiceProvider.GetInstance().Services.GetService<IAddAccountDataManager>();
            addAccountRequest = request;
            presenterAddAccountCallback = responseCallback;
        }
        public override void Action()
        {
            this.AddAccountDataManager.AddAccount(addAccountRequest, new AddUserCallback(this));
        }
        public class AddUserCallback : IUsecaseCallbackBaseCase<bool>
        {
            private AddAccount addAccount;

            public AddUserCallback(AddAccount addAccount)
            {
                this.addAccount = addAccount;
            }

            public void OnResponseError(BException response)
            {
                addAccount.presenterAddAccountCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                addAccount.presenterAddAccountCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                addAccount.presenterAddAccountCallback?.OnSuccessAsync(response);
            }
        }
    }

    public class AddAccountRequest
    {
        public AccountBObj newAccount { get; set; }
        public AddAccountRequest(AccountBObj NewAccount, string userId)
        {
            newAccount = NewAccount;
        }
    }
    public interface IPresenterAddAccountCallback : IResponseCallbackBaseCase<bool>
    {
    }


   
}
