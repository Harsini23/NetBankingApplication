using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DbAdapter
{
    public interface IDbAdapter
    {
        void Create<T>(T value) where T : new();
        int Update<T>(T value) where T : new();
        int Delete<T>(T value) where T : new();
        int Insert<T>(T value) where T : new();
        IEnumerable<T> Get<T>(T value) where T : new();
        int InsertAll<T>(List<T> values) where T : new();
        List<T> GetFromQuery<T>(string query, params object[] value) where T : new() ;

    }
    public class SqliteDbAdapter : IDbAdapter
    {
        //get and set specific sqlite connection string
        public static SQLiteConnection connection;

        public SqliteDbAdapter(DatabaseConnection dbConn)
        {
            if (connection == null)
            {
                var conn = dbConn;
                connection = conn.GetDbConnection();

                //check on create table conn process
                //initialize db
                LibraryInitialization libraryInitialization;
                libraryInitialization = LibraryInitialization.GetInstance();
                libraryInitialization.InitializeDb();
            }
        }

        public void Create<T>(T value) where T : new()
        {
             connection.CreateTable<T>();
        }

      
        public int Delete<T>(T value) where T : new()
        {
           return connection.Delete(value);
        }

        public IEnumerable<T> Get<T>(T value) where T : new()
        {
            return connection.Table<T>();
        }


        public int Update<T>(T value) where T : new()
        {
            return connection.InsertOrReplace(value);
        }

        public int Insert<T>(T value) where T: new()
        {
            return connection.Insert(value, typeof(T));
        }

        public int InsertAll<T>(List<T> values) where T : new()
        {
            return values.Sum(value => connection.Insert(value));
        }
        public List<T> GetFromQuery<T>(string query, params object[] value) where T : new() 
        {
            return connection.Query<T>(query, value);
        }
    }
}
