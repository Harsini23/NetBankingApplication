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
        private IGetFDDetailsDataManager _getFDDetailsDataManager;
        IPresenterGetFDDetailsCallback _fDDetailsResponseCallback;
        GetFDDetailsRequest _request;
        public GetFDDetails(GetFDDetailsRequest request, IPresenterGetFDDetailsCallback responseCallback)
        {
           
            _getFDDetailsDataManager = ServiceProvider.GetInstance().Services.GetService<IGetFDDetailsDataManager>();
            _request = request;
            _fDDetailsResponseCallback = responseCallback;
        }

        public override void Action()
        {
            //use call back
            this._getFDDetailsDataManager.GetFDDetails(_request, new GetFDDetailsCallback(this));
        }

        public class GetFDDetailsCallback : IUsecaseCallbackBaseCase<FDAccount>
        {
            private GetFDDetails _getFDDetails;

            public GetFDDetailsCallback(GetFDDetails getFDDetails)
            {
                this._getFDDetails = getFDDetails;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _getFDDetails._fDDetailsResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<FDAccount> response)
            {
                _getFDDetails._fDDetailsResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<FDAccount> response)
            {
                _getFDDetails._fDDetailsResponseCallback?.OnSuccessAsync(response);

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
