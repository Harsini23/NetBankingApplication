using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class AddUserDataManager : BankingDataManager, IAddUserDataManager
    {
        private string password;
        public static string UserID {get;set;}
        public AddUserDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void AddNewUser(AddUserRequest request, IUsecaseCallbackBaseCase<AddUserResponse> response)
        {
            ZResponse<AddUserResponse> Response = new ZResponse<AddUserResponse>();
            AddUserResponse addUserResponse = new AddUserResponse();

            var credentials= CreateUserCredentials();
            var user =CreateUser(request.newUser.UserName,request.newUser.MobileNumber,request.newUser.EmailId,request.newUser.PAN);
            var GeneratedAccountNumber = GenerateUniqueId.RandomNumber(100000000, 1000000000).ToString();
            var account= CreateAccount(GeneratedAccountNumber,request.newUser.AccountType,request.newUser.TotalBalance,request.newUser.BId,request.newUser.Currency);
            var userAccount= CreateUserAccounts(GeneratedAccountNumber);

            DbHandler.AddUser(user);
            DbHandler.AddAccount(account);
            DbHandler.AddAccountForUser(userAccount);
            DbHandler.CreateCredential(credentials);
            Debug.WriteLine("Created and added new account and user details");

            addUserResponse.credentials = new Credentials
            {
                UserId = credentials.UserId,
                Password=password,
                IsAdmin=credentials.IsAdmin,
                NewUser=credentials.NewUser,
            };
            addUserResponse.user=user;
            addUserResponse.account = account;
            Response.Data = addUserResponse;
            var responseStatus = "Successfull got all data";
            Response.Response = responseStatus;

            response.OnResponseSuccess(Response);
        }

        private Credentials CreateUserCredentials()
        {
            UserID = GenerateUniqueId.GetUniqueId("UID");
            
            password = GenerateUniqueId.GeneratePassword();
            Debug.WriteLine("Password for the created account: ",password);
            var GeneratedPassword = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(password));

            return new Credentials
            {
                UserId = UserID,
                Password= GeneratedPassword,
                IsAdmin=false,
                NewUser=true,
            };
        }
        private User CreateUser(string username,long mobile,string email,string PAN)
        {
            return new User
            {
                UserId = UserID,
                UserName = username,
                MobileNumber = mobile,
                EmailId = email,
                IsBlocked = false,
                PAN=PAN
            };
        }
        private Account CreateAccount(string accNo,AccountType accType,double TotalBalance,string BId,Currency currency)
        {
            return new Account
            {
                AccountType = accType,
                TotalBalance = TotalBalance,
                AccountNumber = accNo,
                BId = BId,
                Currency = currency,
                AvailableBalanceAsOn=CurrentDateTime.GetCurrentDate()
            };
        }
        private UserAccounts CreateUserAccounts(string accno)
        {
            return new UserAccounts
            {
                UserId = UserID,
                AccountNumber = accno,
            };
        }
    }


}
