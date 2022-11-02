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

namespace Library.Data.DataManager
{
    internal class LoginDataManager : ILoginDataManager
    {
       private int AccessDeniedCount = 5;
       CredentialService credentialService;
       UserService userService;
       CreateTables createTableInstance;
        public LoginDataManager()
        {
            if (createTableInstance==null)
            {
                createTableInstance = CreateTables.GetInstance();
                createTableInstance.InstantiateAllTables();
            }
           credentialService = CredentialService.GetInstance();
           userService = UserService.GetInstance();
        }
        private void InvalidLogin()
        {
            //decrement count_
            //call back implementation to view model
        }

       
        private byte[] EncryptPassword(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] hashValue;
            UTF8Encoding objUtf8 = new UTF8Encoding();
            hashValue = sha256.ComputeHash(objUtf8.GetBytes(str));
            return hashValue;
        }

        static string BytesToString(byte[] bytes)
        {
            using (MemoryStream stream = new MemoryStream(bytes))
            {
                using (StreamReader streamReader = new StreamReader(stream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }

        public void ValidateUserLogin(UserLoginRequest request, Login.UserLoginCallback response)
        {
          
            var UserId= request.UserId;
            User user = null;
            bool IsNewUser = false ;
            string responseStatus = "";
            LoginResponse loginResponse = new LoginResponse();
            ZResponse<LoginResponse> Response = new ZResponse<LoginResponse>();
            var password = BytesToString(EncryptPassword(request.Password));
         // credentialService.AddRecord(request.UserId, password,false);

            if (AccessDeniedCount <= 0 && credentialService.CheckUser(UserId))
            {
                responseStatus = "Too many invalid attempts! Account is blocked";
                Response.Response = responseStatus;
                Response.Data = null;
                response.OnResponseFailure(Response);
                userService.BlockAccount(UserId);
                return;
            }
          
        
            var result = credentialService.CheckUserCredential(UserId, password);
            //var result = credentialService.CheckUserCredential(UserId, request.Password);

           
            if (result)
            {
                //check if admin
                var IsAdmin = credentialService.CheckIfAdmin(UserId);
                if (IsAdmin)
                {
                    responseStatus = "Sucessfully Loged in, Welcome Admin!";
                }
                else if(credentialService.CheckIfNewUser(UserId))
                {
                    responseStatus = "Sucessfully Loged in as new User - reset password!";
                    IsNewUser = true;

                }
                else
                {
                    responseStatus = "Sucessfully Loged in, Welcome User ";
                }
                user = userService.GetUser(UserId);
                loginResponse.currentUser = user;
                loginResponse.NewUser = IsNewUser;
                Response.Data=loginResponse;
                Response.Response = responseStatus;
                response.OnResponseSuccess(Response);
                AccessDeniedCount = 5;
            }
            else
            {
               
                if (credentialService.CheckUser(UserId))
                {
                    AccessDeniedCount--;
                    responseStatus = "Invalid password";
                    Response.Response = responseStatus;
                    Response.Data = null;
                    response.OnResponseFailure(Response);
                   
                }
                else
                {
                    responseStatus = "Invalid User, Try Again";
                    Response.Response = responseStatus;
                    Response.Data = null;
                    response.OnResponseFailure(Response);
                }
             
            }
        }
    }

    public class LoginResponse :ZResponse<User>
    {
        public User currentUser;
        public bool NewUser;

    }
}
