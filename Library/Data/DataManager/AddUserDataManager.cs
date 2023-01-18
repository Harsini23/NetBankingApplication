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
        public static string UserID {get;set;}
        public AddUserDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        public void AddNewUser(AddUserRequest request, AddUser.AddUserCallback response)
        {
            var credentials= CreateUserCredentials();
            var user =CreateUser(request.newUser.UserName,request.newUser.MobileNumber,request.newUser.EmailId,request.newUser.PAN);
            var account= CreateAccount(request.newUser.AccountNumber,request.newUser.AccountType,request.newUser.TotalBalance,request.newUser.BId,request.newUser.Currency);
            var userAccount= CreateUserAccounts(request.newUser.AccountNumber);

            DbHandler.AddUser(user);
            DbHandler.AddAccount(account);
            DbHandler.AddAccountForUser(userAccount);
            DbHandler.CreateCredential(credentials);
            Debug.WriteLine("Created and added new account and user details");
        }

        private Credentials CreateUserCredentials()
        {
            UserID = GenerateUniqueId.GetUniqueId("UID");
            var password = GenerateUniqueId.GeneratePassword();
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

    public class AddUserResponse : ZResponse<User>
    {
        public User user;
        public Credentials credentials;
        public Account account;
    }
}
