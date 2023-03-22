using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    [Table(name:"User")]
    public class User
    {
        [PrimaryKey,NotNull]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public long MobileNumber { get; set; }
        public string EmailId { get; set; }
        public bool IsBlocked { get; set; }
        public string PAN { get; set; }

        public string ProfilePath { get; set; }

        //public User(string userId, string userName, long mobileNumber, string emailId, string pan)
        //{
        //    UserId = userId;
        //    UserName = userName;
        //    MobileNumber = mobileNumber;
        //    EmailId = emailId;
        //    PAN = pan;
        //}
        public User(string userId, string userName, long mobileNumber, string emailId, string pan,string profilePath="")
        {
            UserId = userId;
            UserName = userName;
            MobileNumber = mobileNumber;
            EmailId = emailId;
            PAN = pan;
            ProfilePath = profilePath;
        }
        public User() { }
    }

    public class UpdateUserBObj
    {
        public string EmailId { get; set; }
        public long MobileNumber { get; set; }
        public string UserName { get; set; }
        public UpdateUserBObj(string emailId, long mobileNumber, string userName)
        {
            EmailId = emailId;
            MobileNumber = mobileNumber;
            UserName = userName;
        }
        public UpdateUserBObj()
        {

        }
    }
}
