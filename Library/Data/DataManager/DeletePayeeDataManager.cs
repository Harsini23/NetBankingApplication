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
        public DeletePayeeDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void DeletePayee(DeletePayeeRequest request, DeletePayee.DeletePayeeCallback callback)
        {
            try
            {
                DbHandler.DeletePayee(request.PayeeToDelete);
                ZResponse<String> response = new ZResponse<String>();
                response.Response = "Deleted successfully";
                response.Data = "Payee Deleted";
                BankingNotification.BankingNotification.NotifyPayeeDeleted(request.PayeeToDelete);
                callback.OnResponseSuccess(response);
            }
            catch (Exception ex)
            {

            }
        }

    }


}
