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
    internal class CredentialService//: IService
    {
        private static CredentialService _instance;

        public SQLiteConnection connection;

        private CredentialService()
        {
            var conn = DatabaseConnection.GetInstance();
            connection = conn.DbConnection;
        }
        public static CredentialService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CredentialService();
            }
            return _instance;
        }

        public bool CheckUser(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId ).FirstOrDefault(); ;
            if (query != null) return true;
            return false;
        }

        public bool CheckUserCredential(string userId,string password)
        {
            //fetches record from db
            //pass to datamanager
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.Password==password).FirstOrDefault();
            if(query!=null) return true;

            return false;
        }

        public bool CheckIfAdmin(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.IsAdmin==true).FirstOrDefault(); ;
            if (query != null) return true;

            return false;
        }
        public bool CheckIfNewUser(string userId)
        {
            var query = connection.Table<Credentials>().Where(c => c.UserId == userId && c.NewUser==true).FirstOrDefault(); 
            if (query != null) return true;

            return false;
        }

        public void AddRecord(String userId, string password, bool isAdmin)
        {
            bool isNewUser=true;
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

        public bool ResetPassword(string userId, string password)
        {
            var query = connection.Query<Credentials>("UPDATE Credentials SET Password=?, NewUser=false WHERE UserID=? ", password,userId);
            
            var ReCheckingquery = connection.Table<Credentials>().Where(c => c.UserId == userId && c.Password == password).FirstOrDefault();
            if (ReCheckingquery != null) return true;
            return false;
        }
        public void AddCredential(string userId,string password)
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
    }
}
