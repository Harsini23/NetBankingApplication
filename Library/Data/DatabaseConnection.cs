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
        //private static DatabaseConnection _instance;

        public static string DbPath;
        static SQLiteConnection DbConnection;
        DataBasePath dbPathConnection;
        public DatabaseConnection(DataBasePath _dbpath)
        {
            //setting connection
            //var dbPathConnection = DataBasePath.GetInstance();
            dbPathConnection = _dbpath;
            EstablishConnection();
            //var sp = ServiceProvider.GetInstance().services.BuildServiceProvider();
            //var dbPathConnectionString = ServiceProvider.GetInstance().Services.GetService(DataBasePath);
            //var dbPathConnection = dbPathConnectionString.GetConnection();

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
        //public static DatabaseConnection GetInstance()
        //{
        //    if (_instance == null)
        //    {
        //        _instance = new DatabaseConnection();
        //    }
        //    return _instance;
        //}
    }
}
