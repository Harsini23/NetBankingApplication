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
        public void GetFDRate(FDRateRequest request, GetFdRate.GetFDRateCallback response)
        {
            double Rate = 0.0;
            Rate = DbHandler.GetFDRate(request.TenureDuration);
            GetFDRateResponse getFDRateResponse = new GetFDRateResponse();
            getFDRateResponse.Data = Rate;
            ZResponse<GetFDRateResponse> zResponse = new ZResponse<GetFDRateResponse>();
            zResponse.Data = getFDRateResponse;
            zResponse.Response = "Successfully got FD rate";
            response.OnResponseSuccess(zResponse);

        }
    }
}
