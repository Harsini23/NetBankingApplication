using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetAllAccounts;

namespace Library.Domain.UseCase
{

    public interface IGetAllAccountsDataManager
    {
        void GetAllAccounts(GetAllAccountsRequest request, IUsecaseCallbackBaseCase<GetAllAccountsResponse> response);//call back
    }

    public class GetAllAccountsRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }
        public bool GetOnlyTransferAccounts { get; set; }
        public GetAllAccountsRequest(string userId,bool isTransferAccount,CancellationTokenSource cts)
        {
            UserId = userId;
            GetOnlyTransferAccounts=isTransferAccount;
            CtsSource = cts;
        }
    }

    public interface IPresenterGetAllAccountsCallback: IResponseCallbackBaseCase<GetAllAccountsResponse>
    {
    }
    public class GetAllAccounts:UseCaseBase<GetAllAccountsResponse>
    {
     
        private IGetAllAccountsDataManager _getAllAccountsDataManager;
        private GetAllAccountsRequest _getAllAccountsRequest;
        IPresenterGetAllAccountsCallback _getAllAccountsResponseCallback;
        public GetAllAccounts(GetAllAccountsRequest request, IPresenterGetAllAccountsCallback responseCallback)
        {
            _getAllAccountsDataManager = ServiceProvider.GetInstance().Services.GetService<IGetAllAccountsDataManager>();
            _getAllAccountsRequest = request;
            _getAllAccountsResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._getAllAccountsDataManager.GetAllAccounts(_getAllAccountsRequest, new GetAllAccountsCallback(this));
        }

        public class GetAllAccountsCallback : IUsecaseCallbackBaseCase<GetAllAccountsResponse>
        {
            private GetAllAccounts _getAllAccounts;
            public GetAllAccountsCallback(GetAllAccounts GetAllAccounts)
            {
                this._getAllAccounts = GetAllAccounts;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _getAllAccounts._getAllAccountsResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetAllAccountsResponse> response)
            {
                _getAllAccounts._getAllAccountsResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetAllAccountsResponse> response)
            {
                _getAllAccounts._getAllAccountsResponseCallback?.OnSuccessAsync(response);

            }
        }
        public class GetAllAccountsResponse : ZResponse<Account>
        {
            public List<Account> AllAccount;
            public List<AccountBalance> AllAccountBalance = new List<AccountBalance>();
        }
    }
}
