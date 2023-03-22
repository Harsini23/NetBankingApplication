using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model.Enum
{
    public enum Currency
    {
        INR,
        DLR,
    }

    public static class CurrencyValues
    {
        public static readonly String INR = "₹";
        public static readonly String DLR = "$";
    }
}
