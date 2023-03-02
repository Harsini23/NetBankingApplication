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
        public bool CheckUser(string userId)
        {
            var user = connection.Table<User>().Where(i => i.UserId == userId);
            if (user.Count() > 0) return true;
            return false;
        }

        public User GetUser(string userId)
        {
            var query = connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault();
            User user = query;
            return user;
        }

        public void BlockAccount(string userId)
        {
            var query = connection.Table<User>().Where(i => i.UserId == userId).FirstOrDefault();
            User user = query;
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
            if (query != null) return true;
            return false;
        }
        public bool CheckUserCredential(string userId, string password)
        {
            //fetches record from db
            //pass to datamanager
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.Password == password).FirstOrDefault();
            if (query != null) return true;

            return false;
        }

        public bool CheckIfAdmin(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.IsAdmin).FirstOrDefault(); ;
            if (query != null) return true;

            return false;
        }
        public bool CheckIfNewUser(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.NewUser).FirstOrDefault();
            if (query != null) return true;

            return false;
        }

        public void AddRecord(String userId, string password, bool isAdmin)
        {
            bool isNewUser = true;
            if (isAdmin == true)
            {
                isNewUser = false;
            }
            var credentials = new Credentials()
            {
                UserId = userId,
                Password = password,
                IsAdmin = isAdmin,
                NewUser = isNewUser
            };
            connection.Insert(credentials);
        }

        public bool ResetPassword(string userId, string password, bool IsAdmin)
        {
            var credentials = new Credentials()
            {
                UserId = userId,
                Password = password,
                NewUser = false,
                IsAdmin = IsAdmin
            };
            connection.InsertOrReplace(credentials);
            var ReCheckingquery = connection.Table<Credentials>().Where(c => c.UserId == userId && c.Password == password).FirstOrDefault();
            if (ReCheckingquery != null) return true;
            return false;
        }
        public void AddCredential(string userId, string password)
        {
            var newCredential = new Credentials()
            {
                UserId = userId,
                Password = password,
                NewUser = true,
                IsAdmin = false
            };
            connection.Insert(newCredential);
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

            connection.InsertOrReplace(account);
            var result = connection.Table<Account>().Where(c => c.AccountNumber == account.AccountNumber && c.TotalBalance == account.TotalBalance).FirstOrDefault();
            if (result != null) return true; else return false;
        }

        public void AddAccount(Account account)
        {
            connection.Insert(account);
        }

        #endregion

        #region "Transactions"
        public Transaction AddTransaction(Transaction transaction)
        {
            connection.Insert(transaction);
            var ReCheckingquery = connection.Table<Transaction>().Where(i => i.UserId == transaction.UserId && i.TransactionId == transaction.TransactionId).FirstOrDefault();
            if (ReCheckingquery != null && transaction != null) return ReCheckingquery;
            return ReCheckingquery;
            //for(int i = 2; i < 10; i++)
            //{
            //    var transaction = new Transaction()
            //    {
            //        UserId = "Harsh",
            //        TransactionId = "T0000"+i,
            //        Date = "21-11-2022",
            //        TransactionType = (Model.Enum.TransactionType)1,
            //        Remark = "Outing",
            //        TransactionAmout = "2000"+i*200,
            //        FromAccount = "89036457389231",
            //        ToAccount = "89036457389234",
            //        Status = true

            //    };
            //}
        }

        public List<Transaction> GetAllTransactions(string userId)
        {
            List<Transaction> allTransactions = new List<Transaction>();
            var AllTransactions = connection.Table<Transaction>().Where(c => c.UserId == userId);
            foreach (var i in AllTransactions)
            {
                allTransactions.Add(i);
            }
            return allTransactions;

        }

        public List<Transaction> GetTransactionsForAccount(string accountNumber)
        {
            List<Transaction> allTransactions = new List<Transaction>();
            var AllTransactions = connection.Table<Transaction>().Where(c => c.FromAccount == accountNumber || c.ToAccount== accountNumber);
            foreach (var i in AllTransactions)
            {
                allTransactions.Add(i);
            }
            return allTransactions;
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
            var AllCurrentPayee = connection.Table<Payee>().Where(i => i.UserID == userId).ToList();
            return AllCurrentPayee;
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
            var query = connection.Table<UserAccounts>().Where(c => c.UserId == userId)
                          .Select(c => c.AccountNumber).ToList();
            return query;
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

        #endregion


        #region "Brach table"
        public Branch GetBranchDetails(String BId)
        {
            return connection.Table<Branch>().Where(i => i.BId == BId).FirstOrDefault();
        }
        #endregion


       public void CreateDefaultAdminIfNotExists(Credentials credential)
        {
            var user = new User
            {
                UserId = credential.UserId,
                UserName = "Admin",
                MobileNumber = 0,
                EmailId = "-",
                IsBlocked = false,
                PAN = "NA"
            };
            connection.Insert(user);
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
        public void InsertBankBranchDetails(List<Branch> branches)
        {
            connection.InsertAll(branches);
        }

        public List<Branch> GetAllBranches()
        {
            return connection.Table<Branch>().ToList();
        }
        #endregion
    }
}