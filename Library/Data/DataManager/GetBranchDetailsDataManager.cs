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
    public class GetBranchDetailsDataManager : BankingDataManager, IGetBranchDetailsDataManager
    {
        public GetBranchDetailsDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

    
        public void GetBranchDetails(string request, GetBranchDetails.GetBranchDetailsCallback response)
        {
            var result =DbHandler.GetBranchDetails(request);
            GetBranchDetailsResponse getBranchDetailsResponse = new GetBranchDetailsResponse();
            getBranchDetailsResponse.Data = result;
            ZResponse<GetBranchDetailsResponse> zResponse = new ZResponse<GetBranchDetailsResponse>();
            zResponse.Data = getBranchDetailsResponse;
            zResponse.Response = "Successfully got branch details";
            response.OnResponseSuccess(zResponse);

        }
    }

    public class GetBranchDetailsResponse : ZResponse<Branch>
    {
        
    }
}
