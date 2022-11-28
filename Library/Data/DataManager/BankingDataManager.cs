using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class BankingDataManager
    {
        protected static IDbHandler DbHandler;
        protected static INetHandler NetHandler;

        public BankingDataManager(IDbHandler dbHandler, INetHandler netHandler)
        {
            DbHandler = dbHandler;
            NetHandler = netHandler;
        }
    }

    public interface IDbHandler
    {


        bool CheckUser(string userId);
        User GetUser(string userId);
        void BlockAccount(string userId);
        void UnBlockAccount(string userId);
        void AddUser();
        bool CheckIfUserExists(string userId);
        bool CheckUserCredential(string userId, string password);
        bool CheckIfAdmin(string userId);
        bool CheckIfNewUser(string userId);
        void AddRecord(String userId, string password, bool isAdmin);
        bool ResetPassword(string userId, string password, bool IsAdmin);
        void AddCredential(string userId, string password);
        //bool GetAccount(string userId);
        Transaction AddTransaction(Transaction transaction);
        void AddAccount();
        List<Transaction> GetAllTransactions(string userId);
        bool AddNewPayee(Payee newPayee);
        List<Payee> GetAllPayee(string userId);
        List<Account> GetAllAccounts(string userId);
        Account GetAccount(string accountNumber, string userId);

        bool UpdateBalance(Account account);
        void DeletePayee(Payee payee);


    }
    public interface INetHandler
    {

    }
}
