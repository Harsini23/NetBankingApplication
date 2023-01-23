using Library.Data.DataManager;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetBranchDetails;

namespace Library.Domain.UseCase
{

    public interface IGetBranchDetailsDataManager
    {
        void GetBranchDetails(String request, GetBranchDetailsCallback response);
    }

    public interface IPresenterGetBranchDetailsCallback
    {
        void OnSuccess(ZResponse<GetBranchDetailsResponse> response);
        void OnError(ZResponse<GetBranchDetailsResponse> response);
        void OnFailure(ZResponse<GetBranchDetailsResponse> response);
    }
    public class GetBranchDetails : UseCaseBase
    {
        private IGetBranchDetailsDataManager BranchDetailsDataManager;
        IPresenterGetBranchDetailsCallback BranchDetailsResponse;
        String Request;
        public GetBranchDetails(String request, IPresenterGetBranchDetailsCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            BranchDetailsDataManager = serviceProviderInstance.Services.GetService<IGetBranchDetailsDataManager>();
            Request = request;
            BranchDetailsResponse = responseCallback;
        }

        public override void Action()
        {
            //use call back
            this.BranchDetailsDataManager.GetBranchDetails(Request, new GetBranchDetailsCallback(this));
        }

        public class GetBranchDetailsCallback : ZResponse<GetBranchDetailsResponse>
        {
            private GetBranchDetails GetBranchDetails;
            public GetBranchDetailsCallback(GetBranchDetails GetBranchDetails)
            {
                this.GetBranchDetails = GetBranchDetails;
            }
            public string Response { get; set; }

            public void OnResponseError(ZResponse<GetBranchDetailsResponse> response)
            {
                GetBranchDetails.BranchDetailsResponse.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetBranchDetailsResponse> response)
            {
                GetBranchDetails.BranchDetailsResponse.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetBranchDetailsResponse> response)
            {
                GetBranchDetails.BranchDetailsResponse.OnSuccess(response);

            }
        }
    }
}
