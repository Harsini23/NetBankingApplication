﻿using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Transaction
    {
        public int UserId { get; set; }
        public int TransactionId { get; set; }
        public DateTime Time { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Remark { get; set; }
        public double TransactionAmout { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public bool Status { get; set; }

        public Transaction(int transactionId, DateTime time, TransactionType transactionType, string remark, double transactionAmout, string fromAccount, string toAccount, bool status)
        {
            TransactionId = transactionId;
            Time = time;
            TransactionType = transactionType;
            Remark = remark;
            TransactionAmout = transactionAmout;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Status = status;
        }
    }
}