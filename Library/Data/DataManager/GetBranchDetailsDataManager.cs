using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetBranchDetails;

namespace Library.Data.DataManager
{
    public class GetBranchDetailsDataManager : BankingDataManager, IGetBranchDetailsDataManager
    {
        public GetBranchDetailsDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

    
        public void GetBranchDetails(BranchDetailsRequest request, IUsecaseCallbackBaseCase<GetBranchDetailsResponse> callback)
        {
            Branch singleBranch=null;
            List<Branch> allBranches=null;
            if (request.BranchId != null)
            {
                singleBranch = DbHandler.GetBranchDetails(request.BranchId);

            }
            else
            {
                allBranches = DbHandler.GetAllBranches();
            }
            GetBranchDetailsResponse getBranchDetailsResponse = new GetBranchDetailsResponse();
            getBranchDetailsResponse.Data = singleBranch;
            if(allBranches!=null)
            getBranchDetailsResponse.allBranchDetails = new ObservableCollection<Branch>(allBranches);
            ZResponse<GetBranchDetailsResponse> response = new ZResponse<GetBranchDetailsResponse>();
            response.Data = getBranchDetailsResponse;
            response.Response = "Successfully got branch details";
            callback.OnResponseSuccess(response);

        }
    }

 
}
