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

        public void AddNewPayee(AddPayeeRequest request, IUsecaseCallbackBaseCase<String> callback)
        {
            ZResponse<String> response = new ZResponse<String>();

            var res = DbHandler.AddNewPayee(request.NewPayee);
            if (res)
            {
                response.Response = "Sucessfully added payee";
                response.Data = null;
                callback?.OnResponseSuccess(response);

            }

            //format and send response
        }
    }

   
}
