using Library.Model.Enum;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class FDAccount:Account
    {
        [PrimaryKey]
        public string AccountNumber { get; set; }
        public string TenureDate { get; set; }
        public bool AutoRenual { get; set; }
        public FDType fDType { get; set; } 
        public CustomerType customerType { get; set; }
        public FDAccount(string accountNumber, string tenureDate, bool autoRenual, FDType fDType, CustomerType customerType)
        {
            AccountNumber = accountNumber;
            TenureDate = tenureDate;
            AutoRenual = autoRenual;
            this.fDType = fDType;
            this.customerType = customerType;
        }
        public FDAccount()
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

    public class FDBobj: FDAccount
    {
        public double InterestAmount { get; set; }
        public double Rate { get; set; }
        public string MaturityDate { get; set; }
        public double MaturityAmount { get; set; }
        public FDBobj(double interest, double rate, string maturityDate, double maturityAmount)
        {
            InterestAmount = interest;
            Rate = rate;
            MaturityDate = maturityDate;
            MaturityAmount = maturityAmount;
        }
        public FDBobj()
        {

        }
    }
    

}
