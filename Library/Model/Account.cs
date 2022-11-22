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
        public AccountType AccountType { get; set; }
        public string TotalBalance { get; set; }
        public string AvailableBalanceAsOn { get; set; }
        public string BId { get; set; }

        public Currency Currency { get; set; }

        public Account(string accountNumber, string ifscCode, AccountType accountType, string totalBalance, string cardNumber)
        {
            AccountNumber = accountNumber;
            AccountType = accountType;
            TotalBalance = totalBalance;
         
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

