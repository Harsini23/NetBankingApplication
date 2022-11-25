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
    public class DbHandler: IDbHandler
    {

        //private static DbHandler _instance;
        public static SQLiteConnection connection;

        public DbHandler()
        {
            if(connection == null)
            {
                var conn = DatabaseConnection.GetInstance();
                connection = conn.DbConnection;
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
            Debug.WriteLine("------------------------------------");
            //   Debug.WriteLine(query.UserName);
            User user = query;
            // Debug.WriteLine(query.UserId);

            // var users= new User() { UserId=userId,UserName="Harsh" };
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

        public void AddUser()
        {
            var user = new User()
            {
                UserId = "Harsh",
                UserName = "Harsini",
                MobileNumber = 9026745564,
                EmailId = "harsh@gmail.com",
                IsBlocked = false
            };
            connection.Insert(user);
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
            //var query = connection.Query<Credentials>("UPDATE Credentials SET Password=?, NewUser=false WHERE UserID=? ", password, userId);
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
        //public void GetData<User>()
        //{
        //   //get data from db and return in type of user
        //}

        #endregion

        #region "Account Table operations"
        //public bool GetAccount(string userId)
        //{
        //    var user = connection.Table<Account>().Where(i => i.UserId == userId);
        //    if (user.Count() > 0) return true;
        //    return false;
        //}

        public List<Account> GetAllAccounts(string userId)
        {
            List<Account> allAccounts = new List<Account>();
            var AllAccounts = connection.Table<Account>().Where(c => c.UserId == userId);
            foreach (var i in AllAccounts)
            {
                allAccounts.Add(i);
            }
            return allAccounts;
        }

        public Account GetAccount(string accountNumber,string userId)
        {

            var account = connection.Table<Account>().Where(i => i.UserId == userId &&i.AccountNumber==accountNumber).FirstOrDefault();
          return account;
        }

        public bool UpdateBalance(Account account)
        {
           
            connection.InsertOrReplace(account);
            var ReCheckingquery = connection.Table<Account>().Where(c => c.UserId == account.UserId && c.AccountNumber == account.AccountNumber && c.TotalBalance==account.TotalBalance).FirstOrDefault();
            if (ReCheckingquery != null) return true;
            return false;
        }

        #endregion


        #region "Add account explictly"
        public void AddAccount()
        {
            var account = new Account()
            {
                AccountNumber= "678987265323",
                UserId="Harsh",
                AccountType=0,
                AvailableBalanceAsOn="21-11-2022",
                TotalBalance= "1000000.00",
                BId="B001",
                Currency=0
            };
            connection.Insert(account);

            var account2 = new Account()
            {
                AccountNumber = "907625243788",
                UserId = "Harsh",
                AccountType = 0,
                AvailableBalanceAsOn = "24-11-2022",
                TotalBalance = "9000000.00",
                BId = "B002",
                Currency = 0
            };
            connection.Insert(account2);
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
            foreach(var i in AllTransactions)
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
            var ReCheckingquery = connection.Table<Payee>().Where(i=> i.UserID==newPayee.UserID && i.AccountNumber==newPayee.AccountNumber).FirstOrDefault();
            if (ReCheckingquery != null && newPayee!=null) return true;
            return false;
        }

        public List<Payee> GetAllPayee(string userId)
        {
            List<Payee> allPayee = new List<Payee>();
            var AllCurrentPayee = connection.Table<Payee>().Where(i =>i.UserID==userId);
            foreach(var i in AllCurrentPayee)
            {
                allPayee.Add(i);
            }
            return allPayee;
        }
        #endregion

    }
}
