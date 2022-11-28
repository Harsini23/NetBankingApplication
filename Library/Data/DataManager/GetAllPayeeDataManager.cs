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
    public class GetAllPayeeDataManager : BankingDataManager, IGetAllPayeeDataManager
    {
        public GetAllPayeeDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        void IGetAllPayeeDataManager.GetAllPayee(GetAllPayeeRequest request, GetAllPayee.GetAllPayeeCallback response)
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

    public class GetAllPayeeResponse : ZResponse<Payee>
    {
        public List<Payee> allRecipients;
    }
}
