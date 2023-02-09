using Library.Data.DataManager;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetAllPayee;

namespace Library.Domain.UseCase
{
    public interface IGetAllPayeeDataManager
    {
        void GetAllPayee(GetAllPayeeRequest request, GetAllPayeeCallback response);//call back
    }

    public class GetAllPayeeRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public GetAllPayeeRequest(string userId,CancellationTokenSource cts)
        {
            UserId = userId;
            CtsSource=cts;
        }
    }

    public interface IPresenterGetAllPayeeCallback: IResponseCallbackBaseCase<GetAllPayeeResponse>
    {
    }
    public class GetAllPayee:UseCaseBase<GetAllPayeeResponse>
    {

        private IGetAllPayeeDataManager GetAllPayeeDataManager;
        private GetAllPayeeRequest GetAllPayeeRequest;
        IPresenterGetAllPayeeCallback GetAllPayeeResponse;
        public GetAllPayee(GetAllPayeeRequest request, IPresenterGetAllPayeeCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            GetAllPayeeDataManager = serviceProviderInstance.Services.GetService<IGetAllPayeeDataManager>();
            GetAllPayeeRequest = request;
            GetAllPayeeResponse = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this.GetAllPayeeDataManager.GetAllPayee(GetAllPayeeRequest, new GetAllPayeeCallback(this));
            // this.GetAllPayeeDataManager.ValidateUserLogin(GetAllPayeeRequest, new GetAllPayeeCallback(this));
        }

        public class GetAllPayeeCallback : ZResponse<GetAllPayeeResponse>
        {
            private GetAllPayee GetAllPayee;
            public GetAllPayeeCallback(GetAllPayee GetAllPayee)
            {
                this.GetAllPayee = GetAllPayee;
            }
            public string Response { get; set; }

            public void OnResponseError(String response)
            {
                GetAllPayee.GetAllPayeeResponse.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetAllPayeeResponse> response)
            {
                GetAllPayee.GetAllPayeeResponse.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetAllPayeeResponse> response)
            {
                GetAllPayee.GetAllPayeeResponse.OnSuccess(response);

            }
        }
    }
}
