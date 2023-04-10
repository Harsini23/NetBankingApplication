using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.BankingNotification
{
    public static class BankingNotification
    {
        public static event Action<Payee> PayeeUpdated;
        internal static void NotifyPayeeUpdated(Payee payee)
        {
            PayeeUpdated?.Invoke(payee);
        }

        public static event Action<Payee> PayeeDeleted;
        internal static void NotifyPayeeDeleted(Payee payee)
        {
            PayeeDeleted?.Invoke(payee);
        }

        public static event Action<Account> AccountUpdated;
        internal static void NotifyAccountUpdated(Account account)
        {
            AccountUpdated?.Invoke(account);
        }

    }
}
