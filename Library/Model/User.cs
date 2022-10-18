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
        public string AccountNumber { get; set; }
        public string PAN { get; set; }
        public long MobileNumber { get; set; }
        public string EmailId { get; set; }
        public bool IsBlocked { get; set; }
        public User(string userId, string userName, string accountNumber, string Pan, long mobileNumber, string emailId)
        {
            UserId = userId;
            UserName = userName;
            AccountNumber = accountNumber;
            PAN = Pan;
            MobileNumber = mobileNumber;
            EmailId = emailId;
          
        }
        public User() { }
    }
}
