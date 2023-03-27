using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataBaseService
{
    public interface IDbHandler: IUserDbHandler, ICredentialDbHandler, IAccountDbHandler, IAdminDbHandler, ITransactiondbHandler, IPayeeDbHandler, IUserAccountDbHandler,IBranchDbHandler
    {
     
       
    }
    public interface INetHandler
    {

    }
    public interface IUserDbHandler
    {
        List<User> GetAllUsers();
        bool CheckUser(string userId);
        User GetUser(string userId);
        void BlockAccount(string userId);
        void UnBlockAccount(string userId);
        void AddUser(User user);
        string GetUserName(String userId);
        bool UpdateUser(User updatedUser);
        bool IfUserAlreadyExists(string email, long mobileNo, string Pan);
        string GetProfile(string userId);
    }

    public interface ICredentialDbHandler
    {
        bool CheckIfUserExists(string userId);
        bool CheckUserCredential(string userId, string password);
        bool CheckIfAdmin(string userId);
        bool CheckIfNewUser(string userId);
        bool ResetPassword(Credentials newCredential);
        void CreateCredential(Credentials cred);
        void CreateDefaultAdminIfNotExists(Credentials credentials);

    }

    public interface IAccountDbHandler
    {
        void AddAccount(Account account);
        Account GetAccount(string accountNumber);
        bool UpdateBalance(Account account);
        double GetTotalBalanceOfUser(string userId);
        Dictionary<String, double> GetAllAccountBalance(string userId);
    }

    public interface IAdminDbHandler
    {
        bool CheckIfAdminExists(string userId);
    }

    public interface ITransactiondbHandler
    {
        bool AddTransaction(Transaction transaction);
        List<Transaction> GetAllTransactions(string userId, bool fetchRecentTransactions);
        List<Transaction> GetTransactionsForAccount(string accountNumber);
        double GetTotalIncome(string userId);
        double GetTotalExpense(string userId);
        List<Transaction> GetCurrentMonthIncome(string userId);
        List<Transaction> GetCurrentMonthExpense(string userId);

    }
    public interface IPayeeDbHandler
    {
        bool AddNewPayee(Payee newPayee);
        List<Payee> GetAllPayee(string userId);
        void DeletePayee(Payee payee);
        bool EditPayee(Payee payee);
    }
    public interface IUserAccountDbHandler
    {
        void AddAccountForUser(UserAccounts userAccounts);
        List<UserAccounts> GetAllAccountsForUser(string userId);
    }
    public interface IBranchDbHandler
    {
        Branch GetBranchDetails(String BId);
        void InsertBankBranchDetails(List<Branch> branches);
        List<Branch> GetAllBranches();
    }

}
