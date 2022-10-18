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
    internal class UserService //: IService
    {

        private static UserService _instance;

        public SQLiteConnection connection;
        private UserService()
        {
            var conn = DatabaseConnection.GetInstance();
            connection = conn.DbConnection;
            connection.CreateTable<User>();
        }
        public static UserService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new UserService();
            }
            return _instance;
        }

        public void InitializeTable()
        {
            connection.CreateTable<User>();
        }

        public bool CheckUser(string userId)
        {
            var user = connection.Table<User>().Where(i=> i.UserId==userId);
            if(user.Count() > 0) return true;
            return false;
        }

        public User GetUser(string userId)
        {
            var query = connection.Table<User>().Where(i=>i.UserId==userId).FirstOrDefault();
            Debug.WriteLine("------------------------------------");
            Debug.WriteLine(query.UserName);
            User user=query;
           Debug.WriteLine(query.UserId);
            
            // var users= new User() { UserId=userId,UserName="Harsh" };
            return user;
        }

        //public void AddUser()
        //{
        //    var user = new User()
        //    {
        //        UserId = "Harsh2002",
        //        UserName="Harsini",
        //        AccountNumber="129194203812380048",
        //        PAN="KW329DG912",
        //        MobileNumber=9026745564,
        //        EmailId="harsh@gamil.com",
        //        IsBlocked=false
        //    };
        //    connection.Insert(user);
        //}
    }
}
