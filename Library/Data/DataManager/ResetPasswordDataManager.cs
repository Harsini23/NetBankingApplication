using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Library.Data.DataManager
{
    public class ResetPasswordDataManager :BankingDataManager, IResetPasswordDataManager
    {   

        public ResetPasswordDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
          
        }
        public void ResetPassword(ResetPasswordRequest request,ResetPassword.ResetPasswordCallback callback)
        {
            ZResponse<bool> response = new ZResponse<bool>();

            var isAdmin = DbHandler.CheckIfAdmin(request.UserId);
            Credentials updatedCredential = new Credentials(request.UserId, PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(request.NewPassword)), false, isAdmin);
            var result= DbHandler.ResetPassword(updatedCredential);
            response.Data = result;
            if (result)
            {
                //password reset successfully
                response.Response = "Password updated successfully!";
                callback.OnResponseSuccess(response);
            }
            else
            {
                //error in password resetting
                response.Response = "Failed to reset Password";// add error in future if needed
                callback.OnResponseFailure(response);
            }
        }
    }
  

}
