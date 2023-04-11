using Library.Data.DataManager;
using Library.Data.DbAdapter;
using Library.Domain;
using Library.Model;
using Library.Model.Enum;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataBaseService
{
    //make this and nethandler singleton
    public class DbHandler : IDbHandler
    {

       private IDbAdapter _adapter;
        public DbHandler()
        {
            _adapter = Domain.ServiceProvider.GetInstance().Services.GetService<IDbAdapter>();
        }

        #region "User Table operations"
        public void AddUser(User user)
        {
            _adapter.Update(user);
        }
        public List<User> GetAllUsers()
        {   
            return _adapter.Get(new User()).ToList();
        }

        public bool CheckUser(string userId)
        {
            var user = _adapter.Get(new User()).Where(i => i.UserId == userId);
            return (user.Count() > 0);
        }
        public bool CheckPassword(UserPasswordBObj credential)
        {
            var check = _adapter.Get(new Credentials()).Where(i => i.UserId == credential.UserId && i.Password == credential.Password).FirstOrDefault();
            return check != null;
        }

        public bool UpdateUser(User UpdatedUser)
        {
            int check = _adapter.Update(UpdatedUser);
            return (check != 0);
        }

        public User GetUser(string userId)
        {
            User user = _adapter.Get(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            return user;
        }

        public void BlockAccount(string userId)
        {
            User user = _adapter.Get(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            user.IsBlocked = true;
            _adapter.Update(user);
        }

        public void UnBlockAccount(string userId)
        {
            var query = _adapter.Get(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            query.IsBlocked = false;
            _adapter.Update(query);
        }

      

        public string GetUserName(String userId)
        {
            var query = _adapter.Get(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            return query.UserName;
        }
       
        #endregion

        #region "Credential Table operations"

        public bool CheckIfUserExists(string userId)
        {
            var query = _adapter.Get(new Credentials()).Where(c => c.UserId == userId).FirstOrDefault(); ;
            return (query != null);
        }   
        public bool CheckIfAdminExists(string Id)
        {
            var query = _adapter.Get(new Admin()).Where(c => c.EmployeeId == Id).FirstOrDefault(); ;
            return (query != null);
        }
        public bool CheckUserCredential(string userId, string password)
        {
            var query = _adapter.Get(new Credentials()).Where(c => c.UserId == userId && c.Password == password).FirstOrDefault();
            return (query != null);
        }

        public bool CheckIfAdmin(string userId)
        {
            var query = _adapter.Get(new Credentials()).Where(c => c.UserId == userId && c.IsAdmin).FirstOrDefault(); ;
            return (query != null);
        }
        public bool CheckIfNewUser(string userId)
        {
            var query = _adapter.Get(new Credentials()).Where(c => c.UserId == userId && c.NewUser).FirstOrDefault();
            return (query != null);
        }


        public bool ResetPassword(Credentials newCredential)
        {
            int check=_adapter.Update(newCredential);
            return (check != 0);
        }

        public void CreateCredential(Credentials cred)
        {
            _adapter.Update(cred);
        }
     

        #endregion

        #region "Account Table operations"
        public Account GetAccount(string accountNumber)
        {
            var account = _adapter.Get(new Account()).Where(i => i.AccountNumber == accountNumber).FirstOrDefault();
            return account;
        }

        public bool UpdateBalance(Account account)
        {

           int check= _adapter.Update(account);
            return (check != 0) ; 
        }

        public void AddAccount(Account account)
        {
            _adapter.Insert<Account>(account);
        }
        public double GetBalance(string AccountNumber)
        {
            return _adapter.Get(new Account()).Where(i => i.AccountNumber == AccountNumber).FirstOrDefault().TotalBalance;
        }

        #endregion

        #region "Transactions"
        public bool AddTransaction(AmountTransaction transaction)
        {
           var check= _adapter.Update(transaction);
            return (check != 0) ;
        }

        public List<AmountTransaction> GetAllTransactions(string userId,bool getRecentTransactions)
        {
            if (getRecentTransactions)
            {
                //return only recent 10 transactions
                var AllTransactions = _adapter.Get(new AmountTransaction()).Where(c => c.UserId == userId).ToList();
                return  AllTransactions.OrderByDescending(c => DateTimeOffset.Parse(c.Date)).Take(10).ToList();
            }
            else
            {
                //return all transactions
                return _adapter.Get(new AmountTransaction()).Where(c => c.UserId == userId).ToList();
            }
        }

        public List<AmountTransaction> GetTransactionsForAccount(string accountNumber)
        {
            return _adapter.Get(new AmountTransaction()).Where(c => c.FromAccount == accountNumber || c.ToAccount == accountNumber).ToList();
        }

        #endregion

        #region "Payee"

        public bool AddNewPayee(Payee newPayee)
        {
            _adapter.Update(newPayee);
            var ReCheckingquery = _adapter.Get(new Payee()).Where(i => i.UserID == newPayee.UserID && i.AccountNumber == newPayee.AccountNumber).FirstOrDefault();
            if (ReCheckingquery != null && newPayee != null) return true;
            return false;
        }

        public List<Payee> GetAllPayee(string userId)
        {
           return _adapter.Get(new Payee()).Where(i => i.UserID == userId).ToList();
        }

        public void DeletePayee(Payee payee)
        {
            _adapter.Delete(payee);
            var DeletedPayee = _adapter.Get(new Payee()).Where(i => i.UserID == payee.UserID && i.AccountNumber == payee.AccountNumber);
        }
        #endregion

        #region "UserAccounts"
        public void AddAccountForUser(UserAccounts userAccounts)
        {
            _adapter.Insert(userAccounts);
        }

        public List<Account> GetAllAccountsForUser(string userId, bool getOnlyTransferAccounts)
        {
            //return _adapter.Get(new UserAccounts()).Where(c => c.UserId == userId).ToList();
            String query = "";
            if (getOnlyTransferAccounts)
            {
                query = "SELECT * FROM Account account JOIN UserAccounts userAccounts ON account.AccountNumber = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserID AND account.AccountType!=@AccountType;";
            }
            else
            {
                query = "SELECT * FROM Account account JOIN UserAccounts userAccounts ON account.AccountNumber = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserID;";
            }
            return _adapter.GetFromQuery<Account>(query, userId,AccountType.FDAccount);
  
        }

       public double GetTotalBalanceOfUser(string userId)
        {

            var query = "SELECT account.TotalBalance FROM Account account JOIN UserAccounts userAccounts ON account.AccountNumber = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserID;";
           var results= _adapter.GetFromQuery<Account>(query,userId);
            return results.Sum(i => i.TotalBalance);
          
        }

        public Dictionary<String, double> GetAllAccountBalance(string userId)
        {
            var allAccountBalance = new Dictionary<String,double>();
            var AllAccounts = _adapter.Get(new UserAccounts()).Where(c => c.UserId == userId).Select(c=>c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var res = _adapter.Get(new Account()).Where(j => j.AccountNumber == i && j.AccountType!=AccountType.FDAccount).FirstOrDefault();
                if(res!=null)
                allAccountBalance.Add(i, res.TotalBalance);
            }
            return allAccountBalance;
        }


        public bool EditPayee(Payee payee)
        {
            _adapter.Update(payee);
            var ReCheckingquery = _adapter.Get(new Payee()).Where(c => c.AccountNumber == payee.AccountNumber && c.AccountHolderName == payee.AccountHolderName).FirstOrDefault();
            if (ReCheckingquery != null) return true;
            return false;

        }
        #endregion


      


       public void CreateDefaultAdminIfNotExists(Credentials credential)
        {
            var admin = new Admin
            {
                EmployeeId = credential.UserId,
                Name = "Admin",
                MobileNumber = 0,
                EmaiId = "-",
                BranchId = "B001",
            };
            _adapter.Update(admin);
            _adapter.Update(credential);
        }

        #region "Overview"
        public double GetTotalIncome(UserTransactionType userTransactionType)
        {
            var query= "SELECT amountTransaction.Amount FROM AmountTransaction amountTransaction JOIN UserAccounts userAccounts ON amountTransaction.ToAccount = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserId and amountTransaction.TransactionType=@TransactionType;";
            var results = _adapter.GetFromQuery<AmountTransaction>(query, userTransactionType.UserId,userTransactionType.TransactionType);
            return Math.Round(results.Sum(i => i.Amount),2);

        }

        public double GetTotalExpense(UserTransactionType userTransactionType)
        {
            var query = "SELECT amountTransaction.Amount FROM AmountTransaction amountTransaction JOIN UserAccounts userAccounts ON amountTransaction.FromAccount = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserId and amountTransaction.TransactionType=@TransactionType;";
            var results = _adapter.GetFromQuery<AmountTransaction>(query, userTransactionType.UserId, userTransactionType.TransactionType);
            return Math.Round(results.Sum(i => i.Amount), 2);
        }

        public List<AmountTransaction> GetCurrentMonthIncome(UserTransactionType userTransactionType)
        {
            var query = "SELECT * FROM AmountTransaction amountTransaction JOIN UserAccounts userAccounts ON amountTransaction.ToAccount = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserId and amountTransaction.TransactionType=@TransactionType;";
            return  _adapter.GetFromQuery<AmountTransaction>(query, userTransactionType.UserId, userTransactionType.TransactionType);
        }

        public List<AmountTransaction> GetCurrentMonthExpense(UserTransactionType userTransactionType)
        {
          
            var query = "SELECT * FROM AmountTransaction amountTransaction JOIN UserAccounts userAccounts ON amountTransaction.FromAccount = userAccounts.AccountNumber WHERE userAccounts.UserId = @UserId and amountTransaction.TransactionType=@TransactionType;";
            return _adapter.GetFromQuery<AmountTransaction>(query, userTransactionType.UserId, userTransactionType.TransactionType);
        }
        #endregion


        #region "branches"
        public Branch GetBranchDetails(String BId)
        {
            return _adapter.Get(new Branch()).Where(i => i.BId == BId).FirstOrDefault();
        }
        public void InsertBankBranchDetails(List<Branch> branches)
        {
            _adapter.InsertAll(branches);
        }

        public List<Branch> GetAllBranches()
        {
            return _adapter.Get(new Branch()).ToList();
        }

        public bool IfUserAlreadyExists(string email, long mobileNo, string Pan)
        {
            var query = _adapter.Get(new User()).Where(i => i.EmailId == email || i.MobileNumber == mobileNo || i.PAN == Pan).FirstOrDefault();
            return (query != null);
        }

        public string GetProfile(string userId)
        {
            return _adapter.Get(new User()).Where(i => i.UserId == userId).FirstOrDefault().ProfilePath;
        }

        #endregion

        #region
        public void InsertDefaultFDRates(List<FDRates> fDRates)
        {
            _adapter.InsertAll(fDRates);
        }

        public double GetFDRate(int tenureDuration)
        {
           return _adapter.Get(new FDRates()).Where(i => tenureDuration >= i.MinDuration && tenureDuration <= i.MaxDuration).FirstOrDefault().Rate;
        }

        public void AddFDAccount(FDAccount fDAccount)
        {
            _adapter.Insert<FDAccount>(fDAccount);
        }

        public FDAccount FetchFDDetails(string AccountNumber)
        {
            return _adapter.Get(new FDAccount()).Where(account => account.AccountNumber == AccountNumber).FirstOrDefault();
        }

        public void CloseFD(string userId, FDAccount fDAccount)
        {
            _adapter.Delete<FDAccount>(fDAccount);
           var result= _adapter.Get(new UserAccounts()).Where(userAccount => userAccount.AccountNumber == fDAccount.AccountNumber && userAccount.UserId== userId).FirstOrDefault();
            if (result != null)
            {
                _adapter.Delete(result);
            }
            var Account = _adapter.Get(new Account()).Where(account => account.AccountNumber == fDAccount.AccountNumber).FirstOrDefault();
            if(Account != null)
            {
                _adapter.Delete(Account);
            }
        }
        #endregion
    }
}