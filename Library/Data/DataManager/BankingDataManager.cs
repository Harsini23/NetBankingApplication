using Library.Data.DataBaseService;
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

    //singleton class
    //public class BankingDataManager
    //{
    //    protected static IDbHandler DbHandler;
    //    protected static INetHandler NetHandler;

    //    private static BankingDataManager bankingDataManager = null;

    //    private static DbHandler dbhandlerInstance = null;
    //    private static NetHandler nethandlerInstance = null;
    //    private BankingDataManager()
    //    {
    //        dbhandlerInstance = new DbHandler();
    //        nethandlerInstance = new NetHandler();

    //    }
    //    public static BankingDataManager GetInstance()
    //    {
    //        if(bankingDataManager == null)
    //        {
    //            bankingDataManager = new BankingDataManager();
    //        }
    //         return bankingDataManager;
    //    }

    //    //public static DbHandler GetDbHandlerInstance()
    //    //{
    //    //    if (dbhandlerInstance == null)
    //    //    {
    //    //        dbhandlerInstance = new DbHandler();
    //    //    }
    //    //    return dbhandlerInstance;
    //    //}
    //    //public static NetHandler GetNetHandlerInstance()
    //    //{
    //    //    if (nethandlerInstance == null)
    //    //    {
    //    //        nethandlerInstance= new NetHandler();
    //    //    }
    //    //    return nethandlerInstance;
    //    //}
    //    //public BankingDataManager(IDbHandler dbHandler, INetHandler netHandler)
    //    //{
    //    //    DbHandler = dbHandler;
    //    //    NetHandler = netHandler;
    //    //}
    //}

    public interface IDbHandler
    {
        bool CheckUser(string userId);
        User GetUser(string userId);
        void BlockAccount(string userId);
        void UnBlockAccount(string userId);
        void AddUser(User user);
        bool CheckIfUserExists(string userId);
        bool CheckUserCredential(string userId, string password);
        bool CheckIfAdmin(string userId);
        bool CheckIfNewUser(string userId);
        void AddRecord(String userId, string password, bool isAdmin);
        bool ResetPassword(string userId, string password, bool IsAdmin);
        void AddCredential(string userId, string password);
        //bool GetAccount(string userId);
        Transaction AddTransaction(Transaction transaction);
        void AddAccount(Account account);
        List<Transaction> GetAllTransactions(string userId);
        bool AddNewPayee(Payee newPayee);
        void AddAccountForUser(UserAccounts userAccounts);
        List<Payee> GetAllPayee(string userId);
       // List<Account> GetAllAccounts(string accountnummber);
        Account GetAccount(string accountNumber);

        bool UpdateBalance(Account account);
        void DeletePayee(Payee payee);

        List<String> GetAllAccountsForUser(string userId);

        List<Transaction> GetTransactionsForAccount(string accountNumber);

        string GetUserName(String userId);
        void CreateCredential(Credentials cred);

    }
    public interface INetHandler
    {

    }
}
