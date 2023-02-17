using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataBaseService
{
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

        Branch GetBranchDetails(String BId);

        double GetTotalBalnceOfUser(string userId);

        Dictionary<String, double> GetAllAccountBalance(string userId);

        void CreateDefaultAdminIfNotExists(Credentials credentials);
    }

    public interface INetHandler
    {

    }
}
