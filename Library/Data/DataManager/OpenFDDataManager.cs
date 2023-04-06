using Library.Data.DataBaseService;
using Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class OpenFDDataManager : BankingDataManager, IOpenFDDataManager
    {
        public OpenFDDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void OpenFD(OpenFDRequest request, OpenFD.OpenFDCallback response)
        {
            //Rate = DbHandler.GetFDRate(request.TenureDuration);
            //GetFDRateResponse getFDRateResponse = new GetFDRateResponse();
            //getFDRateResponse.Data = Rate;
            //ZResponse<GetFDRateResponse> zResponse = new ZResponse<GetFDRateResponse>();
            //zResponse.Data = getFDRateResponse;
            //zResponse.Response = "Successfully got FD rate";
            //response.OnResponseSuccess(zResponse);


            //populate in FDAccount table
           // var response = DbHandler.AddFDAccount(request.FDBObj);
            //populate in Account table - call that DM?  so that it creates account user account and transaction 
            //send AddAccountRequest with accountBObj and userId

        }
    }
}
