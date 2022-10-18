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

        public void ValidateUserLogin(UserLoginRequest request, Login.UserLoginResponse response)
        {
            var password = EncryptPassword(request.Password);
            var result = credentialService.CheckUserCredential(request.UserId, request.Password);
            User user=null;
            string responseStatus="";
            LoginResponse loginResponse = new LoginResponse();

            if (result)
            {
                user = userService.GetUser(request.UserId);
                responseStatus = "Sucessfully Loged in!";
                loginResponse.currentUser = user;
                loginResponse.Response = responseStatus;
                response.OnResponseSuccess(loginResponse);
            }
            else
            {
                if (AccessDeniedCount <= 0)
                {
                    responseStatus = "Too many invalid attempts! Try again after sometime or block account";
                    loginResponse.Response = responseStatus;
                    response.OnResponseFailure(loginResponse);
                }
                else
                {
                    responseStatus = "Failed Try Again";
                    loginResponse.Response = responseStatus;
                    response.OnResponseFailure(loginResponse);
                }
                AccessDeniedCount--;
            }

            //if the user exists and userid and password matches
            // user = userService.GetUser(request.UserId);
           // user = new User("Harsh2002", " Harsini", " 4632GS42S3", "KI4F632G2S", 3456787542, "emailId");
             //   responseStatus = "Successfully reached datamanager";
        }
    }

    public class LoginResponse : IResponseType<LoginResponse>
    {
        public User currentUser;
        public string Response { get; set; }

    }
}
