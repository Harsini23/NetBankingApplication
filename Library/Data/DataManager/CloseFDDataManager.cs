using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    internal class CloseFDDataManager : BankingDataManager, ICloseFDDataManager
    {
        public CloseFDDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }
        public void CloseFD(CloseFDRequest request, IUsecaseCallbackBaseCase<bool> callback)
        {
            try
            {
                double amount = request.FDAccount.Principle;
                double interestAmount = 0.0;
                BankingNotification.BankingNotification.NotifyAccountDeleted(DbHandler.GetAccount(request.FDAccount.AccountNumber));

                DbHandler.CloseFD(request.UserId, request.FDAccount);
                if (DateTime.Parse(CurrentDateTime.GetCurrentDate())>= DateTime.Parse(request.FDAccount.TenureDate))
                {
                    interestAmount = request.FDAccount.MaturityAmount- request.FDAccount.Principle;
                    DbHandler.AddTransaction(new AmountTransaction
                    {
                        UserId = request.UserId,
                        Name = DbHandler.GetUserName(request.UserId),
                        TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                        Date = CurrentDateTime.GetCurrentDate(),
                        TransactionType = TransactionType.Credited,
                        Amount = interestAmount,
                        Remark = "FD Interest",
                        FromAccount = request.FDAccount.AccountNumber,
                        ToAccount = request.FDAccount.FromAccount,
                        Status = true,
                    });
                }
              
                DbHandler.AddTransaction(new AmountTransaction
                {
                    UserId = request.UserId,
                    Name = DbHandler.GetUserName(request.UserId),
                    TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                    Date = CurrentDateTime.GetCurrentDate(),
                    TransactionType = TransactionType.FDTransation,
                    Amount = request.FDAccount.Principle,
                    Remark="FD Closed Transaction",
                    FromAccount = request.FDAccount.AccountNumber,
                    ToAccount = request.FDAccount.FromAccount,
                    Status = true,
                });
                var account =DbHandler.GetAccount(request.FDAccount.FromAccount);
                if (account != null)
                {
                    account.TotalBalance+=amount;
                    account.TotalBalance+= interestAmount;
                    DbHandler.UpdateBalance(account);
                }
                BankingNotification.BankingNotification.NotifyAccountBalanceEdited(account);
                ZResponse<bool> response = new ZResponse<bool>();
                response.Response = "FD Closed successfully";
                response.Data = true;
                BankingNotification.BankingNotification.ValueChanged(request.FDAccount.FromAccount, request.UserId);

                callback?.OnResponseSuccess(response);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
