using Library.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    //call once when app is first initialized?
    public class CreateTables
    {
        private static CreateTables _instance;

        public SQLiteConnection connection;
        DatabaseConnection conn;
        private CreateTables()
        {
            conn = DatabaseConnection.GetInstance();
           // InstantiateAllTables();
        }
        public static CreateTables GetInstance()
        {
            if (_instance == null)
            {
                _instance = new CreateTables();
            }
            return _instance;
        }

        public void InstantiateAllTables()
        {
            connection = conn.DbConnection;
            connection.CreateTable<Credentials>();
            connection.CreateTable<User>();
            connection.CreateTable<Admin>();
            connection.CreateTable<Transaction>();
            connection.CreateTable<Branch>();
            connection.CreateTable<Card>();
        }

    }
}
