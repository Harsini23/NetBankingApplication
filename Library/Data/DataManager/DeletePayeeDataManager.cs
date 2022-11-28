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
    public class DeletePayeeDataManager : BankingDataManager, IDeletePayeeDataManager
    {
        public DeletePayeeDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        public void DeletePayee(DeletePayeeRequest request, DeletePayee.DeletePayeeCallback response)
        {
            DbHandler.DeletePayee(request.payeeToDelete);
            ZResponse<String> Response = new ZResponse<String>();
            Response.Response = "Deleted successfully";
            Response.Data = "Payee Deleted";

            response.OnResponseSuccess(Response);
        }
    }


}
