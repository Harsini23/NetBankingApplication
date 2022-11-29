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
        public string TotalBalance { get; set; }
        public string AvailableBalanceAsOn { get; set; }
        public string BId { get; set; }

        public Currency Currency { get; set; }

        public Account(string accountNumber, AccountType accountType, string totalBalance,string TotalBalanceAsOn,string bId)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            TotalBalance = totalBalance;
            AvailableBalanceAsOn = TotalBalanceAsOn;
            BId = bId;
        }
        public Account() { }


        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<User>()
        //        .Property(e => e.Type)
        //        .HasConversion<string>();
        //}
    }
}

