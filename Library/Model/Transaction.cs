using Library.Model.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class AmountTransaction : AmountTransfer
    {
        //public string UserId { get; set; }
        //public string Name { get; set; }

        [PrimaryKey]
        public string TransactionId { get; set; }
        public string Date { get; set; }
        public TransactionType TransactionType { get; set; }
        //public string Remark { get; set; }
        //public string Amount { get; set; }
        //public string FromAccount { get; set; }
        //public string ToAccount { get; set; }
        public bool Status { get; set; }

        public AmountTransaction(string transactionId, string time, TransactionType transactionType, double transactionAmout, string fromAccount, string toAccount, bool status, string accountNumber, string name, string remark="")
        {
            TransactionId = transactionId;
            Date = time;
            TransactionType = transactionType;
            Remark = remark;
            Amount = transactionAmout;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Status = status;
            Name = name;
        }

        public AmountTransaction()
        {
        }
    }



    public class TransactionBObj
    {
        public int Index { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string TransactionId { get; set; }
        public DateTime Date { get; set; }
        public TransactionType TransactionType { get; set; }
        public string Remark { get; set; }
        public string Amount { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public bool Status { get; set; }
        public string Time { get; set; }
        public TransactionDateType TransactionDateType { get; set; }
        public TransactionBObj(string transactionId, DateTime date, TransactionType transactionType, string remark, double transactionAmout, string fromAccount, string toAccount, bool status, string accountNumber, string name,int index,string time,TransactionDateType transactionDateType)
        {
            TransactionId = transactionId;
            Date = date;
            TransactionType = transactionType;
            Remark = remark;
            Amount = transactionAmout.ToString();
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Status = status;
            Name = name;
            Index = index;
            Time = time;
            TransactionDateType=transactionDateType;
        }
        public TransactionBObj()
        {

        }
    }

    public class GroupInfosList : ObservableCollection<TransactionBObj>
    {
        public string Key { get; set; }
        public int Count { get; set; }
    }
}
