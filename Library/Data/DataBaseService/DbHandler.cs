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
        public bool GetAccount(string userId)
        {
            var user = connection.Table<Account>().Where(i => i.UserId == userId);
            if (user.Count() > 0) return true;
            return false;
        }
        #endregion
    }
}
