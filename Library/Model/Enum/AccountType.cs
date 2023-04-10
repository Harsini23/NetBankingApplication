using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model.Enum
{
 
        public enum AccountType
        {
            None,
            SalaryAccount,
            SavingsAccount,
            FDAccount,
            JointAccount,
            PensionAccount
        }

    public enum BasicInitialUserAccountType
    {
        SalaryAccount,
        SavingsAccount
    }

    public static class EnumToStringConversion
    {
        public static readonly string SalaryAccount = "Salary Account";
        public static readonly string SavingsAccount = "Savings Account";
        public static readonly string FDAccount = "FD Account";
        public static readonly string JointAccount = "Joint Account";
        public static readonly string PensionAccount = "Pension Account";
    }
    
}
