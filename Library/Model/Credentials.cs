using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Credentials
    {
        [PrimaryKey]
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsFirstUser { get; set; }

        public Credentials(string userId, string password, bool isAuthenticated)
        {
            UserId = userId;
            Password = password;
            IsFirstUser = isAuthenticated;
        }
        public Credentials() { }
    }
   
}
