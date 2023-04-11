using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetBranchDetails;

namespace Library.Domain.UseCase
{

    public interface IGetBranchDetailsDataManager
    {
        void GetBranchDetails(BranchDetailsRequest request, IUsecaseCallbackBaseCase<GetBranchDetailsResponse> response);
    }


    public interface IPresenterGetBranchDetailsCallback: IResponseCallbackBaseCase<GetBranchDetailsResponse>
    {
    }
    public class GetBranchDetails : UseCaseBase<GetBranchDetailsResponse>
    {
        private IGetBranchDetailsDataManager _branchDetailsDataManager;
        IPresenterGetBranchDetailsCallback _branchDetailsResponseCallback;
        BranchDetailsRequest _request;
        public GetBranchDetails(BranchDetailsRequest request, IPresenterGetBranchDetailsCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            _branchDetailsDataManager = serviceProviderInstance.Services.GetService<IGetBranchDetailsDataManager>();
            _request = request;
            _branchDetailsResponseCallback = responseCallback;
        }

        public override void Action()
        {
            //use call back
            this._branchDetailsDataManager.GetBranchDetails(_request, new GetBranchDetailsCallback(this));
        }

        public class GetBranchDetailsCallback : IUsecaseCallbackBaseCase<GetBranchDetailsResponse>
        {
            private GetBranchDetails _getBranchDetails;
            public GetBranchDetailsCallback(GetBranchDetails GetBranchDetails)
            {
                this._getBranchDetails = GetBranchDetails;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _getBranchDetails._branchDetailsResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetBranchDetailsResponse> response)
            {
                _getBranchDetails._branchDetailsResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetBranchDetailsResponse> response)
            {
                _getBranchDetails._branchDetailsResponseCallback?.OnSuccessAsync(response);

            }
        }

        public class BranchDetailsRequest : IRequest
        {
            public string UserId { get; set; }
            public string BranchId { get; set; }
            public CancellationTokenSource CtsSource { get; set; }

            public BranchDetailsRequest(string userId, string branchId, CancellationTokenSource cts)
            {
                UserId = userId;
                BranchId = branchId;
                CtsSource = cts;
            }

            public BranchDetailsRequest()
            {
            }
        }
        public class GetBranchDetailsResponse : ZResponse<Branch>
        {
          public  ObservableCollection<Branch> allBranchDetails { get; set; }
        }
    }
}
