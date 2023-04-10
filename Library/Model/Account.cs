using Library.Model.Enum;
using Library.Util;
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
        public string AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public double TotalBalance { get; set; }
        public string AvailableBalanceAsOn { get; set; }
        public string BId { get; set; }
        public Currency Currency { get; set; }

        public Account(AccountType accountType, double totalBalance,string TotalBalanceAsOn,string bId, string accountNumber = "")
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            TotalBalance = Math.Round(totalBalance,2);
            AvailableBalanceAsOn = TotalBalanceAsOn;
            BId = bId;
        }
        public Account() { }

    }

    public class AccountBObj : Account
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public Currency Currency { get; set; }
        public AccountBObj(string userId, AccountType accountType, double totalBalance, Currency currency, string bId, string name) : base(accountType, totalBalance, CurrentDateTime.GetCurrentDate(), bId)
        {
            UserId = userId;
            Name = name;
        }
        public AccountBObj()
        {

        }
    }

    public class AccountVobj
    {
        public AccountType AccountType { get; set; }
        public string Balance { get; set; }
        public string Currency { get; set; }
        public string Branch { get; set; }
        public AccountVobj(AccountType accountType, string balance, string currency, string branch)
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

    public class UserTransactionType
    {
        public string UserId { get; set; }
        public TransactionType TransactionType { get; set; }    
    }


}

