using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class ClassDummy
    {
        [PrimaryKey]
        public string UserId { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public bool NewUser { get; set; }

        public ClassDummy(string userId, string password, bool isAuthenticated, bool isAdmin)
        {
            UserId = userId;
            Password = password;
            NewUser = isAuthenticated;
            IsAdmin = isAdmin;
        }
        public ClassDummy() { }
    }
}
