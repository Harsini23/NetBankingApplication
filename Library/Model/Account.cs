using Library.Model.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Account
    {
        [PrimaryKey]
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public AccountType AccountType { get; set; }
        public string TotalBalance { get; set; }
       

        public Account(string accountNumber, string ifscCode, AccountType accountType, string totalBalance)
        {
            AccountNumber = accountNumber;
            IfscCode = ifscCode;
            AccountType = accountType;
            TotalBalance = totalBalance;
           
        }
        public Account() { }
    }
}
