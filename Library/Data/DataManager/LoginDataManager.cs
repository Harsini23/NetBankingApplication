using Library.Domain;
using Library.Model;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using SQLite;
using Library.Data.DataBaseService;
using System.Security.Cryptography;
using System.Diagnostics;
using Library.Util;
using Library.Model.Enum;
using static Library.Domain.Login;

namespace Library.Data.DataManager
{
    public class LoginDataManager : BankingDataManager,ILoginDataManager
    {
       private int _accessDeniedCount = 5;
 
        public LoginDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        
        }
     

        public void ValidateUserLogin(UserLoginRequest request, IUsecaseCallbackBaseCase<LoginResponse> callback)
        {
          
            var userId= request.UserId;
            User user = null;
            bool _isNewUser = false ;
            string responseStatus = "";
            LoginResponse loginResponse = new LoginResponse();
            ZResponse<LoginResponse> response = new ZResponse<LoginResponse>();
            var password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(request.Password));

            try {

                if (_accessDeniedCount <= 0 && DbHandler.CheckUser(userId))
                {
                    responseStatus = "Too many invalid attempts! Account is blocked";
                    response.Response = responseStatus;
                    response.Data = null;
                    callback.OnResponseFailure(response);
                    DbHandler.BlockAccount(userId);
                    return;
                }
                var result = DbHandler.CheckUserCredential(userId, password);
                if (result)
                {
                    //check if admin
                    var IsAdmin = DbHandler.CheckIfAdmin(userId);
                    if (IsAdmin)
                    {
                        responseStatus = "Sucessfully Loged in, Welcome Admin!";
                        loginResponse.IsAdmin = true;
                    }
                    if (DbHandler.CheckIfNewUser(userId))
                    {
                        responseStatus = "Sucessfully Loged in as new User - reset password!";
                        _isNewUser = true;
                    }
                    else
                    {
                        responseStatus = "Sucessfully Loged in, Welcome User ";
                    }
                    user = DbHandler.GetUser(userId);

                    loginResponse.currentUser = user;
                    loginResponse.NewUser = _isNewUser;
                    response.Data = loginResponse;
                    response.Response = responseStatus;
                    callback.OnResponseSuccess(response);
                    _accessDeniedCount = 5;
                }
                else
                {

                    if (DbHandler.CheckUser(userId))
                    {
                        _accessDeniedCount--;
                        responseStatus = "Invalid password";
                        response.Response = responseStatus;
                        response.Data = null;
                        //response.OnResponseError(Response);
                        callback.OnResponseError(new BException { exceptionMessage= response.Response });
                    }
                    else
                    {
                        responseStatus = "Invalid User, Try Again";
                        response.Response = responseStatus;
                        response.Data = null;
                        callback.OnResponseFailure(response);
                    }

                }
            }
            catch (NoUserException ex)
            {
               var errObj = new BException(ex);
                callback?.OnResponseError(errObj);
            }

        }
    }

  
}
