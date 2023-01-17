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
        public bool HasSingleAccount { get; set; } 
        
        public string PAN { get; set; }

        public User(string userId, string userName, long mobileNumber, string emailId, bool hasSingleAccount, string pan)
        {
            UserId = userId;
            UserName = userName;
            MobileNumber = mobileNumber;
            EmailId = emailId;
            HasSingleAccount = hasSingleAccount;
            PAN = pan;
        }
        public User() { }
    }
}
