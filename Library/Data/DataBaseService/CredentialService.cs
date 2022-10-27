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
            var query = connection.Table<Credentials>().Where(s => s.UserId == userId ).FirstOrDefault(); ;
            if (query != null) return true;
            return false;
        }

        public bool CheckUserCredential(string userId,string password)
        {
            //fetches record from db
            //pass to datamanager
            var query = connection.Table<Credentials>().Where(s => s.UserId == userId && s.Password==password).FirstOrDefault();
            if(query!=null) return true;

            return false;
        }

        public bool CheckIfAdmin(string userId)
        {
            var query = connection.Table<Credentials>().Where(s => s.UserId == userId && s.IsAdmin==true).FirstOrDefault(); ;
            if (query != null) return true;

            return false;
        }
        public bool CheckIfNewUser(string userId)
        {
            var query = connection.Table<Credentials>().Where(s => s.UserId == userId && s.NewUser==true).FirstOrDefault(); ;
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

        //public void GetData<User>()
        //{
        //   //get data from db and return in type of user
        //}
    }
}
