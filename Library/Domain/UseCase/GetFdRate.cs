using Library.Model;
using Library.Util.FDCalculator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetFdRate;

namespace Library.Domain.UseCase
{
    public interface IGetFDRateDataManager
    {
        void GetFDRate(FDRateRequest request, GetFDRateCallback response);//call back
    }

    public class FDRateRequest 
    {
        public FDBObj FDAccount;
        public int TenureDuration;
        public CancellationTokenSource CtsSource { get; set; }
        public FDRateRequest(FDBObj fDAccount, CancellationTokenSource ctsSource)
        {
            FDAccount = fDAccount;
            TimeSpan timeDifference = DateTime.Parse(fDAccount.TenureDate) - DateTime.Now;
            TenureDuration = timeDifference.Days;
            CtsSource = ctsSource;
        }
    }

    public interface IPresenterGetFDRateCallback : IResponseCallbackBaseCase<GetFDRateResponse>
    {
    }
    public class GetFdRate : UseCaseBase<String>
    {
        private IGetFDRateDataManager _getRateDataManager;
        private FDRateRequest _getFDRateRequest;
        IPresenterGetFDRateCallback _getFDRateResponseCallback;
        public GetFdRate(FDRateRequest request, IPresenterGetFDRateCallback responseCallback)
        {
            _getRateDataManager = ServiceProvider.GetInstance().Services.GetService<IGetFDRateDataManager>();
            _getFDRateRequest = request;
            _getFDRateResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this._getRateDataManager.GetFDRate(_getFDRateRequest, new GetFDRateCallback(this));
        }

         FDCalculatedVobj CalculateFD(double rate)
        {
            FDFactory FdCalculation = new FDFactory();
            var CalculatedFD = FdCalculation.CreateFDCalculation(_getFDRateRequest.FDAccount.CustomerType);
            return CalculatedFD.Calculate(_getFDRateRequest.FDAccount.PrincipalAmount, rate, _getFDRateRequest.TenureDuration);
        }


        public class GetFDRateCallback : ZResponse<String>
        {
            GetFdRate _getFDRate;
            public GetFDRateCallback(GetFdRate getFdRate)
            {
                _getFDRate = getFdRate;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _getFDRate._getFDRateResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetFDRateResponse> response)
            {
                _getFDRate._getFDRateResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetFDRateResponse> response)
            {
                //calculate FD accoring to rate and other conditions and populate response
                //FDFactory FdCalculation = new FDFactory();
                //var CalculatedFD = FdCalculation.CreateFDCalculation(GetFDRate.GetFDRateRequest.FDAccount.CustomerType);
                //response.Data.FDDetails = CalculatedFD.calculate(GetFDRate.GetFDRateRequest.FDAccount.Amount, response.Data.Data,GetFDRate.GetFDRateRequest.TenureDuration );
              
                    response.Data.FDDetails = _getFDRate.CalculateFD(response.Data.Data);
                _getFDRate._getFDRateResponseCallback?.OnSuccessAsync(response);

            }
        }
    }

    public class GetFDRateResponse : ZResponse<double>
    {
        public FDCalculatedVobj FDDetails;
    }
}

