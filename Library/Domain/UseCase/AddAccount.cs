using Library.Model;
using Library.Model.Enum;
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
        private IAddAccountDataManager _addAccountDataManager;
        private AddAccountRequest _addAccountRequest;
        IPresenterAddAccountCallback _presenterAddAccountCallback;
        public AddAccount(AddAccountRequest request,IPresenterAddAccountCallback responseCallback)
        {
            _addAccountDataManager = ServiceProvider.GetInstance().Services.GetService<IAddAccountDataManager>();
            _addAccountRequest = request;
            _presenterAddAccountCallback = responseCallback;
        }
        public override void Action()
        {
            this._addAccountDataManager.AddAccount(_addAccountRequest, new AddUserCallback(this));
        }
        public class AddUserCallback : IUsecaseCallbackBaseCase<bool>
        {
            private AddAccount _addAccount;

            public AddUserCallback(AddAccount addAccount)
            {
                this._addAccount = addAccount;
            }

            public void OnResponseError(BException response)
            {
                _addAccount._presenterAddAccountCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                _addAccount._presenterAddAccountCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                _addAccount._presenterAddAccountCallback?.OnSuccessAsync(response);
            }
        }
    }

    public class AddAccountRequest
    {
        public AccountBObj NewAccount { get; set; }
        public AddAccountRequest(AccountBObj newAccount, string userId)
        {
            NewAccount = newAccount;
        }
    }
    public interface IPresenterAddAccountCallback : IResponseCallbackBaseCase<bool>
    {
    }


   
}
