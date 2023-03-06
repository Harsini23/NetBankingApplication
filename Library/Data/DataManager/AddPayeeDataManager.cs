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
    public class AddPayeeDataManager : BankingDataManager, IAddPayeeDataManager
    {
        public AddPayeeDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void AddNewPayee(AddPayeeRequest request, IUsecaseCallbackBaseCase<String> response)
        {
            ZResponse<String> Response = new ZResponse<String>();

            var res = DbHandler.AddNewPayee(request.NewPayee);
            if (res)
            {
                Response.Response = "Sucessfully added payee";
                Response.Data = null;
                response?.OnResponseSuccess(Response);

            }

            //format and send response
        }
    }

   
}
