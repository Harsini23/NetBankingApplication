using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model.Enum
{
    public enum TransactionType
    {
        Credited,
        Debited,
        Rejected,
        FDTransation
    }

    public enum TransactionDateType
    {
        Today,
        Yesterday,
        Last7Days,
        EarlierThisMonth,
        PreviousTransactions
    }

}
