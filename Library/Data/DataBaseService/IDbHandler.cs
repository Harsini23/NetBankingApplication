using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataBaseService
{
    public interface IDbHandler: IUserDbHandler, ICredentialDbHandler, IAccountDbHandler, IAdminDbHandler, ITransactiondbHandler, IPayeeDbHandler, IUserAccountDbHandler,IBranchDbHandler, IFDRateHandler
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
        double GetBalance(string accountNumber);
        void CloseFD(string userId,FDAccount fDAccount);

    }

    public interface IAdminDbHandler
    {
        bool CheckIfAdminExists(string userId);
    }

    public interface ITransactiondbHandler
    {
        bool AddTransaction(AmountTransaction transaction);
        List<AmountTransaction> GetAllTransactions(string userId, bool fetchRecentTransactions);
        List<AmountTransaction> GetTransactionsForAccount(string accountNumber);
        double GetTotalIncome(UserTransactionType userTransactionType);
        double GetTotalExpense(UserTransactionType userTransactionType);
        List<AmountTransaction> GetCurrentMonthIncome(UserTransactionType userTransactionType);
        List<AmountTransaction> GetCurrentMonthExpense(UserTransactionType userTransactionType);

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
        List<Account> GetAllAccountsForUser(string userId,bool getOnlyTransferAccounts);
    }
    public interface IBranchDbHandler
    {
        Branch GetBranchDetails(String bId);
        void InsertBankBranchDetails(List<Branch> branches);
        List<Branch> GetAllBranches();
    }

    public interface IFDRateHandler
    {
        void InsertDefaultFDRates(List<FDRates> fDRates);
        double GetFDRate(int tenureDuration);
        void AddFDAccount(FDAccount fDAccount);
        FDAccount FetchFDDetails(string accountNumber);
    }

}
