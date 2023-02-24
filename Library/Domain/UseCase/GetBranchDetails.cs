using Library.Data.DataManager;
using Library.Model;
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
        void GetBranchDetails(String request, IUsecaseCallbackBaseCase<GetBranchDetailsResponse> response);
    }

    public interface IPresenterGetBranchDetailsCallback: IResponseCallbackBaseCase<GetBranchDetailsResponse>
    {
    }
    public class GetBranchDetails : UseCaseBase<GetBranchDetailsResponse>
    {
        private IGetBranchDetailsDataManager BranchDetailsDataManager;
        IPresenterGetBranchDetailsCallback BranchDetailsResponseCallback;
        String Request;
        public GetBranchDetails(String request, IPresenterGetBranchDetailsCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            BranchDetailsDataManager = serviceProviderInstance.Services.GetService<IGetBranchDetailsDataManager>();
            Request = request;
            BranchDetailsResponseCallback = responseCallback;
        }

        public override void Action()
        {
            //use call back
            this.BranchDetailsDataManager.GetBranchDetails(Request, new GetBranchDetailsCallback(this));
        }

        public class GetBranchDetailsCallback : IUsecaseCallbackBaseCase<GetBranchDetailsResponse>
        {
            private GetBranchDetails GetBranchDetails;
            public GetBranchDetailsCallback(GetBranchDetails GetBranchDetails)
            {
                this.GetBranchDetails = GetBranchDetails;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                GetBranchDetails.BranchDetailsResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetBranchDetailsResponse> response)
            {
                GetBranchDetails.BranchDetailsResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetBranchDetailsResponse> response)
            {
                GetBranchDetails.BranchDetailsResponseCallback?.OnSuccessAsync(response);

            }
        }

        public class GetBranchDetailsResponse : ZResponse<Branch>
        {

        }
    }
}
