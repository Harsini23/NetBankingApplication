using Microsoft.Data.Sqlite;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;


namespace Library.Data
{
 
    public class DataBasePath
    {

        //get the string from local folder--> save the value in local folder if not present
        //private static DataBasePath _instance;

        static string _databasename ;
        private static string _databasePath;
        public string GetConnection()
        {
            _databasename = "Banking.db";
         
             _databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, _databasename);
            return _databasePath;

        }
    }
}
