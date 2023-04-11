using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class CheckPasswordDataManager : BankingDataManager, ICheckPasswordDataManager
    {
        public CheckPasswordDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void VerifyPassword(CheckPasswordRequest request, IUsecaseCallbackBaseCase<bool> callback)
        {
            ZResponse<bool> response = new ZResponse<bool>();
            var password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(request.CheckCredential.Password));
            response.Data = DbHandler.CheckUserCredential(request.CheckCredential.UserId, password);
            if (response.Data){
                response.Response = "Credential Validated";
            }
            else
            {
                response.Response = "Password mismatch!";
            }
            callback?.OnResponseSuccess(response);
           
        }
    }
}
