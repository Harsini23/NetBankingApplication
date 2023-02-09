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
        void GetAllAccounts(GetAllAccountsRequest request, GetAllAccountsCallback response);//call back
    }

    public class GetAllAccountsRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }

        public GetAllAccountsRequest(string userId,CancellationTokenSource cts)
        {
            UserId = userId;
            CtsSource = cts;
        }
    }

    public interface IPresenterGetAllAccountsCallback: IResponseCallbackBaseCase<GetAllAccountsResponse>
    {
    }
    public class GetAllAccounts:UseCaseBase<GetAllAccountsResponse>
    {
     
        private IGetAllAccountsDataManager GetAllAccountsDataManager;
        private GetAllAccountsRequest GetAllAccountsRequest;
        IPresenterGetAllAccountsCallback GetAllAccountsResponse;
        public GetAllAccounts(GetAllAccountsRequest request, IPresenterGetAllAccountsCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            GetAllAccountsDataManager = serviceProviderInstance.Services.GetService<IGetAllAccountsDataManager>();
            GetAllAccountsRequest = request;
            GetAllAccountsResponse = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this.GetAllAccountsDataManager.GetAllAccounts(GetAllAccountsRequest, new GetAllAccountsCallback(this));
        }

        public class GetAllAccountsCallback : ZResponse<GetAllAccountsResponse>
        {
            private GetAllAccounts GetAllAccounts;
            public GetAllAccountsCallback(GetAllAccounts GetAllAccounts)
            {
                this.GetAllAccounts = GetAllAccounts;
            }
            public string Response { get; set; }

            public void OnResponseError(String response)
            {
                GetAllAccounts.GetAllAccountsResponse.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetAllAccountsResponse> response)
            {
                GetAllAccounts.GetAllAccountsResponse.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetAllAccountsResponse> response)
            {
                GetAllAccounts.GetAllAccountsResponse.OnSuccess(response);

            }
        }
    }
}
