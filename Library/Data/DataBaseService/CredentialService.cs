using Library.Model;
using SQLite;
using System;
using System.Collections.Generic;
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
            connection.CreateTable<Credentials>();
        }
        public static CredentialService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CredentialService();
            }
            return _instance;
        }

        public bool CheckUserCredential(string userId,string password)
        {
            //fetches record from db
            //pass to datamanager
            var query = connection.Table<Credentials>().Where(s => s.UserId == userId && s.Password==password);
            if(query.Count() > 0) return true;
            return false;
        }

        //public void AddRecord(String userId,string password)
        //{
        //    var credentials = new Credentials()
        //    {
        //        UserId = userId,
        //        Password = password
        //    };
        //    connection.Insert(credentials);
        //}

        //public void GetData<User>()
        //{
        //   //get data from db and return in type of user
        //}
    }
}
