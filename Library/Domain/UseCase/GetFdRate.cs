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
        private IGetFDRateDataManager GetRateDataManager;
        private FDRateRequest GetFDRateRequest;
        IPresenterGetFDRateCallback GetFDRateResponseCallback;
        public GetFdRate(FDRateRequest request, IPresenterGetFDRateCallback responseCallback)
        {
            GetRateDataManager = ServiceProvider.GetInstance().Services.GetService<IGetFDRateDataManager>();
            GetFDRateRequest = request;
            GetFDRateResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this.GetRateDataManager.GetFDRate(GetFDRateRequest, new GetFDRateCallback(this));
        }

         FDCalculatedVobj CalculateFD(double rate)
        {
            FDFactory FdCalculation = new FDFactory();
            var CalculatedFD = FdCalculation.CreateFDCalculation(GetFDRateRequest.FDAccount.CustomerType);
            return CalculatedFD.calculate(GetFDRateRequest.FDAccount.PrincipalAmount, rate, GetFDRateRequest.TenureDuration);
        }


        public class GetFDRateCallback : ZResponse<String>
        {
            GetFdRate GetFDRate;
            public GetFDRateCallback(GetFdRate getFdRate)
            {
                GetFDRate = getFdRate;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                GetFDRate.GetFDRateResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetFDRateResponse> response)
            {
                GetFDRate.GetFDRateResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetFDRateResponse> response)
            {
                //calculate FD accoring to rate and other conditions and populate response
                //FDFactory FdCalculation = new FDFactory();
                //var CalculatedFD = FdCalculation.CreateFDCalculation(GetFDRate.GetFDRateRequest.FDAccount.CustomerType);
                //response.Data.FDDetails = CalculatedFD.calculate(GetFDRate.GetFDRateRequest.FDAccount.Amount, response.Data.Data,GetFDRate.GetFDRateRequest.TenureDuration );
              
                    response.Data.FDDetails = GetFDRate.CalculateFD(response.Data.Data);
                    GetFDRate.GetFDRateResponseCallback?.OnSuccessAsync(response);

            }
        }
    }

    public class GetFDRateResponse : ZResponse<double>
    {
        public FDCalculatedVobj FDDetails;
    }
}

