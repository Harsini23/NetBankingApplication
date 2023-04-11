using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class GetFDRateDataManager : BankingDataManager, IGetFDRateDataManager
    {
        public GetFDRateDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {

        }
        public void GetFDRate(FDRateRequest request, GetFdRate.GetFDRateCallback callback)
        {
            double rate = 0.0;
            try
            {
                rate = DbHandler.GetFDRate(request.TenureDuration);
                GetFDRateResponse getFDRateResponse = new GetFDRateResponse();
                getFDRateResponse.Data = rate;
                ZResponse<GetFDRateResponse> response = new ZResponse<GetFDRateResponse>();
                response.Data = getFDRateResponse;
                response.Response = "Successfully got FD rate";
                callback.OnResponseSuccess(response);
            }
            catch (Exception ex)
            {
                callback.OnResponseError(new BException { exceptionMessage = "FD must be mininum of 7 days maximum of 10 years" });
            }
            
       

        }
    }
}
