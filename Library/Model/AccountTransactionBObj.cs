using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace Library.Model
{
    public class AccountTransactionBObj
    {
        public string UserName { get; set; }
        public string AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public TransactionType TransactionType { get; set; }
        public double Amount { get; set; }
        public string DateOfTransaction { get; set; }
        public string Initial { get; set; }
        public Transaction Transaction { get; set; }
        public string PersonPicPath { get; set; }

        public TransactionDateType TransactionDateType { get; set; }

        public AccountTransactionBObj(string userName, string accountNumber, TransactionType transactionType, double amount, string dateOfTransaction, string initial, AccountType accountType)
        {
            UserName = userName;
            AccountNumber = accountNumber;
            TransactionType = transactionType;
            Amount = amount;
            DateOfTransaction = dateOfTransaction;
            Initial = initial;
            AccountType = accountType;
        }
        public AccountTransactionBObj(string userName, string accountNumber, TransactionType transactionType, double amount, string dateOfTransaction, string initial,Transaction transaction, AccountType accountType,string personPicPath="")
        {
            UserName = userName;
            AccountNumber = accountNumber;
            TransactionType = transactionType;
            Amount = amount;
            DateOfTransaction = dateOfTransaction;
            Initial = initial;
            Transaction = transaction;
            PersonPicPath = personPicPath;
            AccountType=accountType;
        }
        public AccountTransactionBObj()
        {

        }
    }

    public class AccountBobj
    {
        public string UserId { get; set; }
        public Account Account { get; set; }

        public AccountBobj()
        {

        }
    }
}
