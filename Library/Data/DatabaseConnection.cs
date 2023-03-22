using Library.Domain;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
   
    public class DatabaseConnection
    {
        public static string DbPath;
        static SQLiteConnection DbConnection;
        DataBasePath dbPathConnection;
        public DatabaseConnection(DataBasePath _dbpath)
        {
            dbPathConnection = _dbpath;
            EstablishConnection();
        }
        public void EstablishConnection()
        {
            DbPath = dbPathConnection.GetConnection();
            DbConnection = new SQLiteConnection(DbPath);
        }

        public SQLiteConnection GetDbConnection()
        {
            return DbConnection;
        }
      
    }
}
