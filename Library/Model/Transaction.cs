using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Transaction
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string TransactionId { get; set; }
        public string Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Remark { get; set; }
        public string TransactionAmout { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public bool Status { get; set; }

        public Transaction(string transactionId, string time, TransactionType transactionType, string remark, string transactionAmout, string fromAccount, string toAccount, bool status, string accountNumber,string name)
        {
            TransactionId = transactionId;
            Date = time;
            TransactionType = transactionType;
            Remark = remark;
            TransactionAmout = transactionAmout;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Status = status;
            Name = name;
        }

        public Transaction()
        {
        }
    }
}
