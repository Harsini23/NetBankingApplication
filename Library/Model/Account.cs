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

       // public string UserId { get; set; }
        [PrimaryKey]
        public string AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public double TotalBalance { get; set; }
        public string AvailableBalanceAsOn { get; set; }
        public string BId { get; set; }

        public Currency Currency { get; set; }

        public Account(string accountNumber, AccountType accountType, double totalBalance,string TotalBalanceAsOn,string bId)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            TotalBalance = totalBalance;
            AvailableBalanceAsOn = TotalBalanceAsOn;
            BId = bId;
        }
        public Account() { }

    }

    public class AccountVobj
    {
        public string AccountType { get; set; }
        public string Balance { get; set; }
        public string Currency { get; set; }
        public string Branch { get; set; }
        public AccountVobj(string accountType, string balance, string currency, string branch)
        {
            AccountType = accountType;
            Balance = balance;
            Currency = currency;
            Branch = branch;
        }
    }
    public class AccountBalance
    {
        public string AccountNumber { get; set; }
        public double TotalBalance { get; set; }
    }

}

