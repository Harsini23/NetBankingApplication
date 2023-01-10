using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class AccountTransactionBObj
    {
        public string UserName { get; set; }
        public string AccountNumber { get; set; }
        public TransactionType TransactionType { get; set; }
        public double Amount { get; set; }
        public string DateOfTransaction { get; set; }
        public string Initial { get; set; }

        public AccountTransactionBObj(string userName, string accountNumber, TransactionType transactionType, double amount, string dateOfTransaction, string initial)
        {
            UserName = userName;
            AccountNumber = accountNumber;
            TransactionType = transactionType;
            Amount = amount;
            DateOfTransaction = dateOfTransaction;
            Initial = initial;
        }
        public AccountTransactionBObj()
        {

        }
    }
}
