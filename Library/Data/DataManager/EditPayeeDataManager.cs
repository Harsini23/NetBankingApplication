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
    public class EditPayeeDataManager : BankingDataManager, IEditPayeeDataManager
    {
        public EditPayeeDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void EditPayee(EditPayeeRequest request, EditPayee.EditPayeeCallback response)
        {
            DbHandler.EditPayee(request.EditedPayee);
            ZResponse<String> Response = new ZResponse<String>();
            Response.Response = "Edited successfully";
            Response.Data = "Payee Edited";

            response.OnResponseSuccess(Response);
        }
    }
}
