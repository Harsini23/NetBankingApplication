using Library.Data.DataManager;
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

        public static SQLiteConnection connection;

        public DbHandler(DatabaseConnection dbConn)
        {
            if (connection == null)
            {
                var conn = dbConn;
                connection = conn.GetDbConnection();
            }

        }


        #region "User Table operations"

       public List<User> GetAllUsers()
        {
            return connection.Table<User>().ToList();
        }

        public bool CheckUser(string userId)
        {
            var user = connection.Table<User>().Where(i => i.UserId == userId);
            return (user.Count() > 0);
        }
        public bool CheckPassword(UserPasswordBObj credential)
        {
            var check = connection.Table<Credentials>().Where(i => i.UserId == credential.UserId && i.Password == credential.Password).FirstOrDefault();
            return check != null;
        }

        public bool UpdateUser(User UpdatedUser)
        {
            int check = connection.InsertOrReplace(UpdatedUser);
            return (check != 0);
        }

        public User GetUser(string userId)
        {
            User user = connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault();
            return user;
        }

        public void BlockAccount(string userId)
        {
            User user = connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault();
            user.IsBlocked = true;
            connection.Update(user);
        }

        public void UnBlockAccount(string userId)
        {
            var query = connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault();
            query.IsBlocked = false;
            connection.Update(query);
        }

        public void AddUser(User user)
        {
            connection.Insert(user);
        }

        public string GetUserName(String userId)
        {
            var query = connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault();
            return query.UserName;
        }
       
        #endregion

        #region "Credential Table operations"

        public bool CheckIfUserExists(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId).FirstOrDefault(); ;
            return (query != null);
        }   
        public bool CheckIfAdminExists(string Id)
        {
            var query = connection.Table<Admin>().Where(c => c.EmployeeId == Id).FirstOrDefault(); ;
            return (query != null);
        }
        public bool CheckUserCredential(string userId, string password)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.Password == password).FirstOrDefault();
            return (query != null);
        }

        public bool CheckIfAdmin(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.IsAdmin).FirstOrDefault(); ;
            return (query != null);
        }
        public bool CheckIfNewUser(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.NewUser).FirstOrDefault();
            return (query != null);
        }


        public bool ResetPassword(Credentials newCredential)
        {
            int check=connection.InsertOrReplace(newCredential);
            return (check != 0);
        }

        public void CreateCredential(Credentials cred)
        {
            connection.Insert(cred);
        }
     

        #endregion

        #region "Account Table operations"
        public Account GetAccount(string accountNumber)
        {
            var account = connection.Table<Account>().Where(i => i.AccountNumber == accountNumber).FirstOrDefault();
            return account;
        }

        public bool UpdateBalance(Account account)
        {

           int check= connection.InsertOrReplace(account);
            return (check != 0) ; 
        }

        public void AddAccount(Account account)
        {
            connection.Insert(account);
        }

        #endregion

        #region "Transactions"
        public bool AddTransaction(Transaction transaction)
        {
           var check= connection.Insert(transaction);
            return (check != 0) ;
        }

        public List<Transaction> GetAllTransactions(string userId,bool getRecentTransactions)
        {
            if (getRecentTransactions)
            {
                //return only recent 10 transactions
                var AllTransactions = connection.Table<Transaction>().Where(c => c.UserId == userId).ToList();
                return  AllTransactions.OrderByDescending(c => DateTimeOffset.Parse(c.Date)).Take(10).ToList();
            }
            else
            {
                //return all transactions
                return connection.Table<Transaction>().Where(c => c.UserId == userId).ToList();
            }
        }

        public List<Transaction> GetTransactionsForAccount(string accountNumber)
        {
            return connection.Table<Transaction>().Where(c => c.FromAccount == accountNumber || c.ToAccount == accountNumber).ToList();
        }

        #endregion

        #region "Payee"

        public bool AddNewPayee(Payee newPayee)
        {
            connection.Insert(newPayee);
            var ReCheckingquery = connection.Table<Payee>().Where(i => i.UserID == newPayee.UserID && i.AccountNumber == newPayee.AccountNumber).FirstOrDefault();
            if (ReCheckingquery != null && newPayee != null) return true;
            return false;
        }

        public List<Payee> GetAllPayee(string userId)
        {
           return connection.Table<Payee>().Where(i => i.UserID == userId).ToList();
        }

        public void DeletePayee(Payee payee)
        {
            connection.Delete(payee);
            var DeletedPayee = connection.Table<Payee>().Where(i => i.UserID == payee.UserID && i.AccountNumber == payee.AccountNumber);
        }
        #endregion

        #region "UserAccounts"
        public void AddAccountForUser(UserAccounts userAccounts)
        {
            connection.Insert(userAccounts);
        }

        public List<String> GetAllAccountsForUser(string userId)
        {
            return connection.Table<UserAccounts>().Where(c => c.UserId == userId)
                          .Select(c => c.AccountNumber).ToList();
        }

       public double GetTotalBalanceOfUser(string userId)
        {
            double total = 0;  
            var AllAccounts = connection.Table<UserAccounts>().Where(c => c.UserId == userId).Select(c=>c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var res = connection.Table<Account>().Where(j => j.AccountNumber == i).FirstOrDefault();
                total += res.TotalBalance;
            }
            return total;
        }

        public Dictionary<String, double> GetAllAccountBalance(string userId)
        {
            var allAccountBalance = new Dictionary<String,double>();
            var AllAccounts = connection.Table<UserAccounts>().Where(c => c.UserId == userId).Select(c=>c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var res = connection.Table<Account>().Where(j => j.AccountNumber == i).FirstOrDefault();
                allAccountBalance.Add(i, res.TotalBalance);
            }
            return allAccountBalance;
        }


        public bool EditPayee(Payee payee)
        {
            connection.InsertOrReplace(payee);
            var ReCheckingquery = connection.Table<Payee>().Where(c => c.AccountNumber == payee.AccountNumber && c.AccountHolderName == payee.AccountHolderName).FirstOrDefault();
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
            connection.Insert(admin);
            connection.Insert(credential);
        }

        #region "Overview"
        public double GetTotalIncome(string userId)
        {
            double income=0.0;
            var AllAccounts = connection.Table<UserAccounts>().Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                double totalIncome = connection.Table<Transaction>().Where(c => c.UserId == userId && c.ToAccount == i).Sum(c => c.Amount);
                income += totalIncome;
            }
            return income;
        }

        public double GetTotalExpense(string userId)
        {
            double income = 0.0;
            var AllAccounts = connection.Table<UserAccounts>().Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            foreach (var i in AllAccounts)
            {
                var singleAccountExpense = connection.Table<Transaction>().Where(c => c.UserId == userId && c.FromAccount == i).Sum(c => c.Amount);
                income += singleAccountExpense;
            }
            return income;
        }

        public List<Transaction> GetCurrentMonthIncome(string userId)
        {
           
            var AllAccounts = connection.Table<UserAccounts>().Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            List<Transaction> monthlyincome=new List<Transaction>();
            foreach (var i in AllAccounts)
            {
                var singleAccountTransaction = connection.Table<Transaction>().Where(c => c.UserId == userId && c.ToAccount == i).ToList();
                foreach (var j in singleAccountTransaction)
                {
                    monthlyincome.Add(j);
                }
            }
            return monthlyincome;
        }

        public List<Transaction> GetCurrentMonthExpense(string userId)
        {
            var AllAccounts = connection.Table<UserAccounts>().Where(c => c.UserId == userId).Select(c => c.AccountNumber).ToList();
            List<Transaction> monthlyexpense = new List<Transaction>();
            foreach (var i in AllAccounts)
            {
                var singleAccountTransaction = connection.Table<Transaction>().Where(c => c.UserId == userId && c.FromAccount == i).ToList();
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
            return connection.Table<Branch>().Where(i => i.BId == BId).FirstOrDefault();
        }
        public void InsertBankBranchDetails(List<Branch> branches)
        {
            connection.InsertAll(branches);
        }

        public List<Branch> GetAllBranches()
        {
            return connection.Table<Branch>().ToList();
        }

        public bool IfUserAlreadyExists(string email, long mobileNo, string Pan)
        {
            var query = connection.Table<User>().Where(i => i.EmailId == email || i.MobileNumber == mobileNo || i.PAN == Pan).FirstOrDefault();
            return (query != null);
        }

        public string GetProfile(string userId)
        {
            return connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault().ProfilePath;
        }




        #endregion
    }
}