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
       private int AccessDeniedCount = 5;
   
       //BankingDataManager bankingDataManager;
     //  CreateTables createTableInstance;
      
        // BankingDataManager Handler;
        public LoginDataManager():base(new DbHandler(),new NetHandler())
        {
           
            //  Handler = new BankingDataManager(dbHandler, netHandler);
            //if (createTableInstance==null)
            //{
            //    createTableInstance = CreateTables.GetInstance();
            //    createTableInstance.InstantiateAllTables();
              
            //}
       
        }
        private void InvalidLogin()
        {
            //decrement count_
            //call back implementation to view model
        }

        public void ValidateUserLogin(UserLoginRequest request, IUsecaseCallbackBaseCase<LoginResponse> response)
        {
          
            var UserId= request.UserId;
            User user = null;
            bool IsNewUser = false ;
            string responseStatus = "";
            LoginResponse loginResponse = new LoginResponse();
            ZResponse<LoginResponse> Response = new ZResponse<LoginResponse>();
            var password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(request.Password));
            // credentialService.AddRecord(request.UserId, password,false);

            try {

                if (AccessDeniedCount <= 0 && DbHandler.CheckUser(UserId))
                {
                    responseStatus = "Too many invalid attempts! Account is blocked";
                    Response.Response = responseStatus;
                    Response.Data = null;
                    response.OnResponseFailure(Response);
                    DbHandler.BlockAccount(UserId);
                    return;
                }


                var result = DbHandler.CheckUserCredential(UserId, password);
                //DbHandler.AddAccountForUser();
                //DbHandler.GetAllTransactions("Harsh");
                //  DbHandler.AddAccount();
                //var result = credentialService.CheckUserCredential(UserId, request.Password);


                if (result)
                {
                    //check if admin
                    var IsAdmin = DbHandler.CheckIfAdmin(UserId);
                    if (IsAdmin)
                    {
                        responseStatus = "Sucessfully Loged in, Welcome Admin!";
                        loginResponse.IsAdmin = true;
                    }
                    if (DbHandler.CheckIfNewUser(UserId))
                    {
                        responseStatus = "Sucessfully Loged in as new User - reset password!";
                        IsNewUser = true;
                    }
                    else
                    {
                        responseStatus = "Sucessfully Loged in, Welcome User ";
                    }
                    user = DbHandler.GetUser(UserId);

                    //DbHandler.AddTransaction(new Transaction
                    //{
                    //    TransactionId = "TIDc1f45b57",
                    //    Date = "17-1-2023 12:38 AM",
                    //    TransactionType = TransactionType.Credited,
                    //Remark = "gift",
                    //Amount = 900.90,
                    //FromAccount = "1234567098",
                    //ToAccount = "390102938345",
                    //Status = true,
                    //Name = "Enid",
                    //UserId= "UIDaf913875"
                    //});

                    loginResponse.currentUser = user;
                    loginResponse.NewUser = IsNewUser;
                    Response.Data = loginResponse;
                    Response.Response = responseStatus;
                    response.OnResponseSuccess(Response);
                    AccessDeniedCount = 5;
                }
                else
                {

                    if (DbHandler.CheckUser(UserId))
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
            catch (NoUserException ex)
            {
               var errObj = new BException(ex);
                response?.OnResponseError(errObj);
            }

        }
    }

  
}
