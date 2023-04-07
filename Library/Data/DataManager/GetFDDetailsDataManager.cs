using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    internal class GetFDDetailsDataManager : BankingDataManager, IGetFDDetailsDataManager
    {
        public GetFDDetailsDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void GetFDDetails(GetFDDetailsRequest request, IUsecaseCallbackBaseCase<FDAccount> response)
        {
            try
            {
                ZResponse<FDAccount> zResponse = new ZResponse<FDAccount>();
                zResponse.Data = DbHandler.FetchFDDetails(request.AccountNumber);
                response.OnResponseSuccess(zResponse);
            }
            catch(Exception ex)
            {
                response.OnResponseError(new BException(ex));
            }
        }
    }
}
