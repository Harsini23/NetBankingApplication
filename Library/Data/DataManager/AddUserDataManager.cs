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
        private string _password;
        public static string UserID {get;set;}
        public AddUserDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void AddNewUser(AddUserRequest request, IUsecaseCallbackBaseCase<AddUserResponse> callback)
        {
            ZResponse<AddUserResponse> response = new ZResponse<AddUserResponse>();
            AddUserResponse addUserResponse = new AddUserResponse();

            var credentials= CreateUserCredentials();
            User user= new User();
            if (EmailValidation.ValidateEmail(request.NewUser.EmailId))
            {
                user = CreateUser(request.NewUser.UserName, request.NewUser.MobileNumber, request.NewUser.EmailId, request.NewUser.PAN);
            }
            else
            {
                callback.OnResponseError(new BException
                {
                    exceptionMessage = "Invalid Email, try again!"
                });
            }
          
            var generatedAccountNumber = GenerateUniqueId.RandomNumber(100000000, 999999999).ToString()+ GenerateUniqueId.RandomNumber(100, 999).ToString();
            var account= CreateAccount(generatedAccountNumber,request.NewUser.AccountType,request.NewUser.TotalBalance,request.NewUser.BId,request.NewUser.Currency);
            var userAccount= CreateUserAccounts(generatedAccountNumber);
            AmountTransaction currentTransaction = AddInitialTransaction(user.UserId, user.UserName, userAccount.AccountNumber, request.NewUser.TotalBalance);

            var checkForExistingAccount = CheckPreviousUsers(request.NewUser.EmailId,request.NewUser.MobileNumber,request.NewUser.PAN);
            String responseStatus="";
            if (!checkForExistingAccount)
            {
                DbHandler.AddUser(user);
                DbHandler.AddAccount(account);
                DbHandler.AddAccountForUser(userAccount);
                DbHandler.CreateCredential(credentials);
                DbHandler.AddTransaction(currentTransaction);
                //Debug.WriteLine("Created and added new account and user details");

                addUserResponse.Credentials = new Credentials
                {
                    UserId = credentials.UserId,
                    Password = _password,
                    IsAdmin = credentials.IsAdmin,
                    NewUser = credentials.NewUser,
                };
                addUserResponse.User = user;
                addUserResponse.Account = account;
                 responseStatus = "Successfull added user";
            }
            else
            {
                addUserResponse.UserExists = true;
                responseStatus = "Ouch, User already exists!";
            }

            response.Data = addUserResponse;
            response.Response = responseStatus;
            callback.OnResponseSuccess(response);
        }

        private Credentials CreateUserCredentials()
        {
            UserID = GenerateUniqueId.GetUniqueId("UID");
            
            _password = GenerateUniqueId.GeneratePassword();
            Debug.WriteLine("Password for the created account: ",_password);
            var GeneratedPassword = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword(_password));

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

        private AmountTransaction AddInitialTransaction(string userId,string name,string ToAcc,double balance)
        {
            AmountTransaction currentTransaction = new AmountTransaction
            {
                UserId = userId,
                Name = name,
                TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                Date = CurrentDateTime.GetCurrentDate(),
                TransactionType = TransactionType.Credited,
                Remark ="Initial deposit",
                Amount =balance ,
                FromAccount = "-",
                ToAccount = ToAcc,
                Status = true,
            };
            return currentTransaction;

        }


        private bool CheckPreviousUsers(string email,long mobileNo,string Pan)
        {
            return DbHandler.IfUserAlreadyExists(email,mobileNo,Pan);
        }
    }


}
