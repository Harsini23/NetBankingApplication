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
        bool CheckIfAdminExists(string userId);
        bool CheckUserCredential(string userId, string password);
        bool CheckIfAdmin(string userId);
        bool CheckIfNewUser(string userId);
        bool ResetPassword(Credentials newCredential);
        bool AddTransaction(Transaction transaction);
        void AddAccount(Account account);
        List<Transaction> GetAllTransactions(string userId,bool fetchRecentTransactions);
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
        double GetTotalBalanceOfUser(string userId);
        Dictionary<String, double> GetAllAccountBalance(string userId);
        void CreateDefaultAdminIfNotExists(Credentials credentials);
        double GetTotalIncome(string userId);
        double GetTotalExpense(string userId);
        List<Transaction> GetCurrentMonthIncome(string userId);
        List<Transaction> GetCurrentMonthExpense(string userId);
        void InsertBankBranchDetails(List<Branch> branches);
        List<Branch> GetAllBranches();
        bool EditPayee(Payee payee);
        bool UpdateUser(User updatedUser);
        bool IfUserAlreadyExists(string email,long mobileNo,string Pan);
        List<User> GetAllUsers();
    }

    public interface INetHandler
    {

    }
}
