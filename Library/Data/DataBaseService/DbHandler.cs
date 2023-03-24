using Library.Data.DataManager;
using Library.Data.DbAdapter;
using Library.Model;
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

        IDbAdapter adapter;
        public DbHandler()
        {
            adapter = new SqliteDbAdapter();
        }

        #region "User Table operations"
        public void AddUser(User user)
        {
            adapter.Update(user);
        }
        public List<User> GetAllUsers()
        {   
            return adapter.GetAll(new User()).ToList();
        }

        public bool CheckUser(string userId)
        {
            var user = adapter.GetAll(new User()).Where(i => i.UserId == userId);
            return (user.Count() > 0);
        }
        public bool CheckPassword(UserPasswordBObj credential)
        {
            var check = adapter.GetAll(new Credentials()).Where(i => i.UserId == credential.UserId && i.Password == credential.Password).FirstOrDefault();
            return check != null;
        }

        public bool UpdateUser(User UpdatedUser)
        {
            int check = adapter.Update(UpdatedUser);
            return (check != 0);
        }

        public User GetUser(string userId)
        {
            User user = adapter.GetAll(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            return user;
        }

        public void BlockAccount(string userId)
        {
            User user = adapter.GetAll(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            user.IsBlocked = true;
            adapter.Update(user);
        }

        public void UnBlockAccount(string userId)
        {
            var query = adapter.GetAll(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            query.IsBlocked = false;
            adapter.Update(query);
        }

      

        public string GetUserName(String userId)
        {
            var query = adapter.GetAll(new User()).Where(i => i.UserId == userId).FirstOrDefault();
            return query.UserName;
        }
       
        #endregion

        #region "Credential Table operations"

        public bool CheckIfUserExists(string userId)
        {
            var query = adapter.GetAll(new Credentials()).Where(c => c.UserId == userId).FirstOrDefault(); ;
            return (query != null);
        }   
        public bool CheckIfAdminExists(string Id)
        {
            var query = adapter.GetAll(new Admin()).Where(c => c.EmployeeId == Id).FirstOrDefault(); ;
            return (query != null);
        }
        public bool CheckUserCredential(string userId, string password)
        {
            var query = adapter.GetAll(new Credentials()).Where(c => c.UserId == userId && c.Password == password).FirstOrDefault();
            return (query != null);
        }

        public bool CheckIfAdmin(string userId)
        {
            var query = adapter.GetAll(new Credentials()).Where(c => c.UserId == userId && c.IsAdmin).FirstOrDefault(); ;
            return (query != null);
        }
        public bool CheckIfNewUser(string userId)
        {
            var query = adapter.GetAll(new Credentials()).Where(c => c.UserId == userId && c.NewUser).FirstOrDefault();
            return (query != null);
        }


        public bool ResetPassword(Credentials newCredential)
        {
            int check=adapter.Update(newCredential);
            return (check != 0);
        }

        public void CreateCredential(Credentials cred)
        {
            adapter.Update(cred);
        }
     

        #endregion

        #region "Account Table operations"
        public Account GetAccount(string accountNumber)
        {
            var account = adapter.GetAll(new Account()).Where(i => i.AccountNumber == accountNumber).FirstOrDefault();
            return account;
        }

        public bool UpdateBalance(Account account)
        {

           int check= adapter.Update(account);
            return (check != 0) ; 
        }

        public void AddAccount(Account account)
        {
            adapter.Update(account);
        }

        #endregion

        #region "Transactions"
        public bool AddTransaction(Transaction transaction)
        {
           var check= adapter.Update(transaction);
            return (check != 0) ;
        }

        public List<Transaction> GetAllTransactions(string userId,bool getRecentTransactions)
        {
            if (getRecentTransactions)
            {
                //return only recent 10 transactions
                var AllTransactions = adapter.GetAll(new Transaction()).Where(c => c.UserId == userId).ToList();
                return  AllTransactions.OrderByDescending(c => DateTimeOffset.Parse(c.Date)).Take(10).ToList();
            }
            else
            {
                //return all transactions
                return adapter.GetAll(new Transaction()).Where(c => c.UserId == userId).ToList();
            }
        }

        public List<Transaction> GetTransactionsForAccount(string accountNumber)
        {
            return adapter.GetAll(new Transaction()).Where(c => c.FromAccount == accountNumber || c.ToAccount == accountNumber).ToList();
        }

        #endregion

        #region "Payee"

        public bool AddNewPayee(Payee newPayee)
        {
            adapter.Update(newPayee);
            var ReCheckingquery = adapter.GetAll(new Payee()).Where(i => i.UserID == newPayee.UserID && i.AccountNumber == newPayee.AccountNumber).FirstOrDefault();
            if (ReCheckingquery != null && newPayee != null) return true;
            return false;
        }

        public List<Payee> GetAllPayee(string userId)
        {
           return adapter.GetAll(new Payee()).Where(i => i.UserID == userId).ToList();
        }

        public void DeletePayee(Payee payee)
        {
            adapter.Delete(payee);
            var DeletedPayee = adapter.GetAll(new Payee()).Where(i => i.UserID == payee.UserID && i.AccountNumber == payee.AccountNumber);
        }
        #endregion

        #region "UserAccounts"
        public void AddAccountForUser(UserAccounts userAccounts)
        {
            adapter.Update(userAccounts);
        }

        public List<String> GetAllAccountsForUser(string userId)
        {
            return adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId)
                          .Select(c => c.AccountNumber).ToList();
        }

       public double GetTotalBalanceOfUser(string userId)
        {
            double total = 0;  
            var AllAccounts = adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId).Select(c=>c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var res = adapter.GetAll(new Account()).Where(j => j.AccountNumber == i).FirstOrDefault();
                total += res.TotalBalance;
            }
            return total;
        }

        public Dictionary<String, double> GetAllAccountBalance(string userId)
        {
            var allAccountBalance = new Dictionary<String,double>();
            var AllAccounts = adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId).Select(c=>c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var res = adapter.GetAll(new Account()).Where(j => j.AccountNumber == i).FirstOrDefault();
                allAccountBalance.Add(i, res.TotalBalance);
            }
            return allAccountBalance;
        }


        public bool EditPayee(Payee payee)
        {
            adapter.Update(payee);
            var ReCheckingquery = adapter.GetAll(new Payee()).Where(c => c.AccountNumber == payee.AccountNumber && c.AccountHolderName == payee.AccountHolderName).FirstOrDefault();
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
            adapter.Update(admin);
            adapter.Update(credential);
        }

        #region "Overview"
        public double GetTotalIncome(string userId)
        {
            double income=0.0;
            var AllAccounts = adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                double totalIncome = adapter.GetAll(new Transaction()).Where(c => c.UserId == userId && c.ToAccount == i).Sum(c => c.Amount);
                income += totalIncome;
            }
            return income;
        }

        public double GetTotalExpense(string userId)
        {
            double income = 0.0;
            var AllAccounts = adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var singleAccountExpense = adapter.GetAll(new Transaction()).Where(c => c.UserId == userId && c.FromAccount == i).Sum(c => c.Amount);
                income += singleAccountExpense;
            }
            return income;
        }

        public List<Transaction> GetCurrentMonthIncome(string userId)
        {
           
            var AllAccounts = adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            List<Transaction> monthlyincome=new List<Transaction>();
            foreach (var i in AllAccounts)
            {
                var singleAccountTransaction = adapter.GetAll(new Transaction()).Where(c => c.UserId == userId && c.ToAccount == i).ToList();
                foreach (var j in singleAccountTransaction)
                {
                    monthlyincome.Add(j);
                }
            }
            return monthlyincome;
        }

        public List<Transaction> GetCurrentMonthExpense(string userId)
        {
            var AllAccounts = adapter.GetAll(new UserAccounts()).Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            List<Transaction> monthlyexpense = new List<Transaction>();
            foreach (var i in AllAccounts)
            {
                var singleAccountTransaction = adapter.GetAll(new Transaction()).Where(c => c.UserId == userId && c.FromAccount == i).ToList();
                foreach (var j in singleAccountTransaction)
                {
                    monthlyexpense.Add(j);
                }
            }
            return monthlyexpense;
        }
        #endregion


        #region "branches"
        public Branch GetBranchDetails(String BId)
        {
            return adapter.GetAll(new Branch()).Where(i => i.BId == BId).FirstOrDefault();
        }
        public void InsertBankBranchDetails(List<Branch> branches)
        {
            adapter.Update(branches);
        }

        public List<Branch> GetAllBranches()
        {
            return adapter.GetAll(new Branch()).ToList();
        }

        public bool IfUserAlreadyExists(string email, long mobileNo, string Pan)
        {
            var query = adapter.GetAll(new User()).Where(i => i.EmailId == email || i.MobileNumber == mobileNo || i.PAN == Pan).FirstOrDefault();
            return (query != null);
        }

        public string GetProfile(string userId)
        {
            return adapter.GetAll(new User()).Where(i => i.UserId == userId).FirstOrDefault().ProfilePath;
        }




        #endregion
    }
}