using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class UserAccounts
    {
        [PrimaryKey,AutoIncrement]
        public int Sno { get; set; }
        public string UserId { get; set; }
        public string AccountNumber { get; set;}
        public UserAccounts(string userId, string accountNumber)
        {
            UserId = userId;
            AccountNumber = accountNumber;
        }
        public UserAccounts()
        {

        }
    }
}
