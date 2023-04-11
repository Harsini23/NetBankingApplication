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

        void IGetAllPayeeDataManager.GetAllPayee(GetAllPayeeRequest request, IUsecaseCallbackBaseCase<GetAllPayeeResponse> callback)
        {
            //get it frm db
            ZResponse<GetAllPayeeResponse> response = new ZResponse<GetAllPayeeResponse>();
            GetAllPayeeResponse getAllPayeeResponse = new GetAllPayeeResponse();

            var userId = request.UserId;
            var allRecipients = DbHandler.GetAllPayee(userId);
            getAllPayeeResponse.AllRecipients = allRecipients;
            response.Data = getAllPayeeResponse;
            var responseStatus = "Successfull got all recipients";
            response.Response = responseStatus;

            callback.OnResponseSuccess(response);

        }
    }

   
}
