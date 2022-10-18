using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Account
    {
        public string UserId { get; set; }
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public AccountType AccountType { get; set; }
        private double TotalBalance { get; set; }
        public Card CardDetails { get; set; }

        public Account(string accountNumber, string ifscCode, AccountType accountType, double totalBalance, Card cardDetails)
        {
            AccountNumber = accountNumber;
            IfscCode = ifscCode;
            AccountType = accountType;
            TotalBalance = totalBalance;
            CardDetails = cardDetails;
        }
    }
}
