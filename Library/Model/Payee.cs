using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Payee
    {
        
        public string UserID { get; set; }
        public string AccountHolderName { get; set; }
        [PrimaryKey]
        public string AccountNumber { get; set; }
        public string IfscCode { get; set; }
        public string BankName { get; set; }
        public string PayeeName { get; set; }

        public Payee(string accountHolderName, string accountNumber, string ifscCode, string bankName, string payeeName, string userID)
        {
            AccountHolderName = accountHolderName;
            AccountNumber = accountNumber;
            IfscCode = ifscCode;
            BankName = bankName;
            PayeeName = payeeName;
            UserID = userID;
          
        }

        public Payee()
        {

        }
    }
}
