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

        static string databasename ;
        private static string databasePath;

        //public string GetDatabasePath()
        //{
        //    return databasePath;
        //}
        //private DataBasePath()
        //{
        //    GetConnection();
        //}
        //public static DataBasePath GetInstance()
        //{
        //    if (_instance == null)
        //    {
        //        _instance = new DataBasePath();
        //    }
        //    return _instance;
        //}

        public string GetConnection()
        {
            databasename = "Banking.db";
            //folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //databasePath = System.IO.Path.Combine(folderPath, databasename);
         //   await ApplicationData.Current.LocalFolder.CreateFileAsync(databasename, CreationCollisionOption.OpenIfExists);
             //databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, databasename);

            //  //fetch from app local storage and make fetch singleton?
            //  await ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.OpenIfExists);
            //   dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, FileName);
            ////  Connection = new SqliteConnection($"Filename={dbPath}");
            ///
             databasePath = Path.Combine(ApplicationData.Current.LocalFolder.Path, databasename);
            return databasePath;

        }
    }
}
