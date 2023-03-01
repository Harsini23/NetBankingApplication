using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetAllPayee;

namespace Library.Data.DataManager
{
    public class GetAllPayeeDataManager : BankingDataManager, IGetAllPayeeDataManager
    {
        public GetAllPayeeDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        void IGetAllPayeeDataManager.GetAllPayee(GetAllPayeeRequest request, IUsecaseCallbackBaseCase<GetAllPayeeResponse> response)
        {
            //get it frm db
            ZResponse<GetAllPayeeResponse> Response = new ZResponse<GetAllPayeeResponse>();
            GetAllPayeeResponse GetAllPayeeResponse = new GetAllPayeeResponse();

            var userId = request.UserId;
            var allRecipients = DbHandler.GetAllPayee(userId);
            GetAllPayeeResponse.allRecipients = allRecipients;
            Response.Data = GetAllPayeeResponse;
            var responseStatus = "Successfull got all recipients";
            Response.Response = responseStatus;

            response.OnResponseSuccess(Response);

        }
    }

   
}
