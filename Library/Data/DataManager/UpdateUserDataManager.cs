using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Util;
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
        public void UpdateUser(UpdateUserRequest request, IUsecaseCallbackBaseCase<User> callback)
        {
            ZResponse<User> response = new ZResponse<User>();
            if (EmailValidation.ValidateEmail(request.UpdatedUser.EmailId))
            {
                var res = DbHandler.UpdateUser(request.UpdatedUser);
                if (res)
                {
                    response.Response = "Sucessfully updated user";
                    response.Data = request.UpdatedUser;
                    callback?.OnResponseSuccess(response);
                }
            }
            else
            {
                callback.OnResponseError(new BException
                {
                    exceptionMessage = "Invalid Email, try again!"
                });
            }
            
        }
    }
}
