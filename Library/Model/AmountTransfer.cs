using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class AmountTransfer
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string FromAccount { get; set; }
        public string ToAccount { get; set; }
        public string Remark { get; set; }
        public double Amount { get; set; }

        public AmountTransfer(string userId, string name, string fromAccount, string toAccount, string remark, double amount)
        {
            UserId = userId;
            Name = name;
            FromAccount = fromAccount;
            ToAccount = toAccount;
            Remark = remark;
            Amount = amount;
        }
        public AmountTransfer()
        {

        }
    }
}
