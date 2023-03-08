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
    public class UpdateUserDataManager : BankingDataManager, IUpdateUserDataManager
    {
        public UpdateUserDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void UpdateUser(UpdateUserRequest request, IUsecaseCallbackBaseCase<User> response)
        {
            ZResponse<User> Response = new ZResponse<User>();
            var res = DbHandler.UpdateUser(request.UpdatedUser);
            if (res)
            {
                Response.Response = "Sucessfully updated user";
                Response.Data = request.UpdatedUser;
                response?.OnResponseSuccess(Response);

            }
        }
    }
}
