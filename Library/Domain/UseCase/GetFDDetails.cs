using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.UseCase
{
    public interface IGetFDDetailsDataManager
    {
        void GetFDDetails(GetFDDetailsRequest request, IUsecaseCallbackBaseCase<FDAccount> response);
    }
    public interface IPresenterGetFDDetailsCallback : IResponseCallbackBaseCase<FDAccount>
    {

    }
    public class GetFDDetails : UseCaseBase<FDAccount>
    {
        private IGetFDDetailsDataManager GetFDDetailsDataManager;
        IPresenterGetFDDetailsCallback FDDetailsResponseCallback;
        GetFDDetailsRequest Request;
        public GetFDDetails(GetFDDetailsRequest request, IPresenterGetFDDetailsCallback responseCallback)
        {
            var serviceProviderInstance =
            GetFDDetailsDataManager = ServiceProvider.GetInstance().Services.GetService<IGetFDDetailsDataManager>();
            Request = request;
            FDDetailsResponseCallback = responseCallback;
        }

        public override void Action()
        {
            //use call back
            this.GetFDDetailsDataManager.GetFDDetails(Request, new GetFDDetailsCallback(this));
        }

        public class GetFDDetailsCallback : IUsecaseCallbackBaseCase<FDAccount>
        {
            private GetFDDetails getFDDetails;

            public GetFDDetailsCallback(GetFDDetails getFDDetails)
            {
                this.getFDDetails = getFDDetails;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                getFDDetails.FDDetailsResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<FDAccount> response)
            {
                getFDDetails.FDDetailsResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<FDAccount> response)
            {
                getFDDetails.FDDetailsResponseCallback?.OnSuccessAsync(response);

            }
        }
    }

        public class GetFDDetailsRequest
        {
            public string AccountNumber { get; set; }
            public CancellationTokenSource CtsSource { get; set; }

            public GetFDDetailsRequest(string accountNumber, CancellationTokenSource cts)
            {
                AccountNumber = accountNumber;
                CtsSource = cts;
            }

            public GetFDDetailsRequest()
            {
            }
        }
        
    }
