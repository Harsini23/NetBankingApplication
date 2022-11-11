using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
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

        public ResetPasswordDataManager() : base(new DbHandler(), new NetHandler())
        {
          
        }
        public void ResetPassword(ResetPasswordRequest request,ResetPassword.ResetPasswordCallback callback)
        {
            ZResponse<bool> Response = new ZResponse<bool>();

             byte[] EncryptPassword(string str)
            {
                SHA256 sha256 = SHA256Managed.Create();
                byte[] hashValue;
                UTF8Encoding objUtf8 = new UTF8Encoding();
                hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));
                return hashValue;
            }

             string BytesToString(byte[] bytes)
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    using (StreamReader streamReader = new StreamReader(stream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
            var IsAdmin = DbHandler.CheckIfAdmin(request.UserId);
            var result= DbHandler.ResetPassword(request.UserId,BytesToString(EncryptPassword( request.NewPassword)),IsAdmin);
            Response.Data = result;
            if (result)
            {
                //password reset successfully
                Response.Response = "Reset password successfully";
                callback.OnResponseSuccess(Response);
            }
            else
            {
                //error in password resetting
                Response.Response = "Failed to resetPassword";// add error in future if needed
                callback.OnResponseFailure(Response);
            }
        }
    }
  

}
