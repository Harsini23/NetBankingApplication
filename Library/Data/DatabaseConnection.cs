using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
   
    internal class DatabaseConnection
    {
        private static DatabaseConnection _instance;

        public static string DbPath;
       public SQLiteConnection DbConnection;

        private DatabaseConnection()
        {
            //setting connection
            var dbPathConnection = DataBasePath.GetInstance();
            DbPath = dbPathConnection.GetDatabasePath();
            DbConnection = new SQLiteConnection(DbPath);
        }
        public static DatabaseConnection GetInstance()
        {
            if (_instance == null)
            {
                _instance = new DatabaseConnection();
            }
            return _instance;
        }
    }
}
