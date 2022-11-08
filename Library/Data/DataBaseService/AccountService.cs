using Library.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataBaseService
{
    internal class AccountService
    {
        private static AccountService _instance;

        public SQLiteConnection connection;
        private AccountService()
        {
            var conn = DatabaseConnection.GetInstance();
            connection = conn.DbConnection;
            connection.CreateTable<Account>();
        }
        public static AccountService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AccountService();
            }
            return _instance;
        }

        public void InitializeTable()
        {
            connection.CreateTable<Account>();
        }

        public bool GetAccount(string userId)
        {
            var user = connection.Table<Account>().Where(i => i.UserId == userId);
            if (user.Count() > 0) return true;
            return false;
        }



    }
}
