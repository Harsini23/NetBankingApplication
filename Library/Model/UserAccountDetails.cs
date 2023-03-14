using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class UserAccountDetails
    {
        public string UserName { get; set; }
        public long MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string AccountNumber { get; set; }
        public AccountType AccountType { get; set; }
        public double TotalBalance { get; set; }
        public Currency Currency { get; set; }
        public string BId { get; set; }

        public string PAN { get; set; }

        public UserAccountDetails(string userName, long mobileNumber, string emailId, string accountNumber, AccountType accountType, double totalBalance, Currency currency, string bId, string PAN)
        {

            UserName = userName;
            MobileNumber = mobileNumber;
            EmailId = emailId;
            AccountNumber = accountNumber;
            AccountType = accountType;
            TotalBalance = totalBalance;
            Currency = currency;
            BId = bId;
            PAN = PAN;
        }
        public UserAccountDetails()
        {
                
        }
    }

    public class AccountBObj
    {
        public string UserId { get; set; }
        public AccountType AccountType { get; set; }
        public double TotalBalance { get; set; }
        public Currency Currency { get; set; }
        public string BId { get; set; }
        public AccountBObj(string userId, AccountType accountType, double totalBalance, Currency currency, string bId)
        {
            UserId = userId;
            AccountType = accountType;
            TotalBalance = totalBalance;
            Currency = currency;
            BId = bId;
        }
        public AccountBObj()
        {

        }
    }
}
