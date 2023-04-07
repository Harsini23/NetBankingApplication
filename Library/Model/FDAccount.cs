using Library.Model.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class FDAccount
    {
        [PrimaryKey]
        public string AccountNumber { get; set; }
        public string TenureDate { get; set; }
        public bool AutoRenual { get; set; }
        public FDType FDType { get; set; } 
        public CustomerType CustomerType { get; set; }
        public double MaturityAmount { get; set; }
        public string FromAccount { get; set; }
        public double Rate { get; set; }
        public double Principle { get; set; }
        public FDAccount(string accountNumber, string tenureDate, bool autoRenual, FDType fDType, CustomerType customerType, string fromAccount, double maturityAmount, double rate, double principle)
        {
            AccountNumber = accountNumber;
            TenureDate = tenureDate;
            AutoRenual = autoRenual;
            FDType = fDType;
            CustomerType = customerType;
            FromAccount = fromAccount;
            MaturityAmount = Math.Round(maturityAmount,2);
            Rate = rate;
            Principle = Math.Round(principle,2);
        }
        public FDAccount(string tenureDate, bool autoRenual, FDType fDType, CustomerType customerType, string fromAccount, double maturityAmount, double rate, double principle)
        {
            TenureDate = tenureDate;
            AutoRenual = autoRenual;
            FDType = fDType;
            CustomerType = customerType;
            FromAccount = fromAccount;
            MaturityAmount = Math.Round(maturityAmount, 2);
            Rate = rate;
            Principle = Math.Round(principle, 2);
        }
        public FDAccount()
        {

        }
    }

    public class FDRates
    {
        [PrimaryKey, AutoIncrement]
        public int Sno { get; set; }
        public int MinDuration { get; set; }
        public int MaxDuration { get; set; }
        public double Rate { get; set; }
        public FDRates(int sno, int minDuration, int maxDuration, double rate)
        {
            Sno = sno;
            MinDuration = minDuration;
            MaxDuration = maxDuration;
            Rate = rate;
        }
        public FDRates()
        {

        }
    }
    public class FDAccountBObj: FDAccount
    {
        public string UserID { get; set; }
        public string Username { get; set; }
        public double InterestAmount { get; set; }
        public FDAccountBObj(string userId,double principal, double interest, double rate, string tenureDate, FDType fDType, CustomerType customerType, string fromAccount, double maturityAmount, string username) : base(tenureDate, false, fDType, customerType, fromAccount, maturityAmount,principal,rate)
        {
            UserID = userId;
            InterestAmount = interest;
            Username = username;
        }
        public FDAccountBObj()
        {

        }
    }

    public class FDBObj
    {
        public string UserID { get; set; }
        public string TenureDate { get; set; }
        public double PrincipalAmount { get; set; }
        public FDType FDType { get; set; }
        public CustomerType CustomerType{get; set;}
        public string FromAccountNumber { get; set; }
        public FDBObj(string tenureDate, double amount, FDType fDType, CustomerType customerType, string fromAccountNumber, string userID)
        {
            TenureDate = tenureDate;
            PrincipalAmount = amount;
            FDType = fDType;
            CustomerType = customerType;
            FromAccountNumber = fromAccountNumber;
            UserID = userID;
        }
        public FDBObj()
        {

        }
    }

    public class FDCalculatedVobj
    {
        public double InterestAmount { get; set; }
        public double Rate { get; set; }
        public string MaturityDate { get; set; }
        public double MaturityAmount { get; set; }
        public FDCalculatedVobj()
        {

        }
    }

    
    

}
