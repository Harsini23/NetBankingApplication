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

namespace Library.Data.DataManager
{
    internal class LoginDataManager : ILoginDataManager
    {
       private int AccessDeniedCount = 5;
       CredentialService credentialService;
       UserService userService;
        public LoginDataManager()
        {
           credentialService = CredentialService.GetInstance();
           userService = UserService.GetInstance();
        }
        private void InvalidLogin()
        {
            //decrement count_
            //call back implementation to view model
        }

        private string EncryptPassword(string password)
        {
            return "";
        }

        public void ValidateUserLogin(UserLoginRequest request, Login.UserLoginCallback response)
        {
            var password = EncryptPassword(request.Password);
            var result = credentialService.CheckUserCredential(request.UserId, request.Password);
           
            User user=null;
            string responseStatus="";
            LoginResponse loginResponse = new LoginResponse();
            ZResponse<LoginResponse> Response = new ZResponse<LoginResponse>();
            if (result)
            {
                user = userService.GetUser(request.UserId);
                responseStatus = "Sucessfully Loged in!";
                 loginResponse.currentUser = user;
               
                Response.Data=loginResponse;
                Response.Response = responseStatus;
                response.OnResponseSuccess(Response);
                AccessDeniedCount = 5;
            }
            else
            {
                if (AccessDeniedCount <= 0)
                {
                    responseStatus = "Too many invalid attempts! Try again after sometime or account is blocked";
                    Response.Response = responseStatus;
                    Response.Data = null;
                    response.OnResponseFailure(Response);
                }
                else
                {
                    responseStatus = "Failed Try Again";
                    Response.Response = responseStatus;
                    Response.Data = null;
                    response.OnResponseFailure(Response);
                }
                AccessDeniedCount--;
            }
        }
    }

    public class LoginResponse :ZResponse<User>
    {
        public User currentUser;

    }
}
