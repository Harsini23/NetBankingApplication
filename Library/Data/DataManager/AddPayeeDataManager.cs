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
        public AddPayeeDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        public void AddNewPayee(AddPayeeRequest request, AddPayee.AddPayeeCallback response)
        {
            ZResponse<String> Response = new AddPayeeResponse();

            var res = DbHandler.AddNewPayee(request.NewPayee);
            if (res)
            {
                Response.Response = "Sucessfully added payee now you can make tranfers to this account";
                Response.Data = null;
                response?.OnResponseSuccess(Response);

            }

            //format and send response
        }
    }

    public class AddPayeeResponse : ZResponse<String>
    {

    }
}
