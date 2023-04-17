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
        public static event Action<Account> AccountDeleted;
        internal static void NotifyAccountDeleted(Account account)
        {
            AccountDeleted?.Invoke(account);
        }
        public static event Action<Account> AccountBalanceEdited;
        internal static void NotifyAccountBalanceEdited(Account account)
        {
            AccountBalanceEdited?.Invoke(account);
        }

        public static event Action<User> UserUpdated;
        internal static void NotifyUserUpdated(User user)
        {
            UserUpdated?.Invoke(user);
        }

        public static event Action<string,string> ValueChangedEventHandler ;

        internal static void ValueChanged(String acc,string userId)
        {
            ValueChangedEventHandler?.Invoke(acc,userId);
        }
    }
}
