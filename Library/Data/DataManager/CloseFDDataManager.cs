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
        public void CloseFD(CloseFDRequest request, IUsecaseCallbackBaseCase<bool> response)
        {
            try
            {
                double Amount = request.FDAccount.Principle;
                double InterestAmount = 0.0;
                BankingNotification.BankingNotification.NotifyAccountDeleted(DbHandler.GetAccount(request.FDAccount.AccountNumber));

                DbHandler.CloseFD(request.UserId, request.FDAccount);
                if (DateTime.Parse(CurrentDateTime.GetCurrentDate())>= DateTime.Parse(request.FDAccount.TenureDate))
                {
                    InterestAmount = request.FDAccount.MaturityAmount- request.FDAccount.Principle;
                    DbHandler.AddTransaction(new AmountTransaction
                    {
                        UserId = request.UserId,
                        Name = DbHandler.GetUserName(request.UserId),
                        TransactionId = GenerateUniqueId.GetUniqueId("TID"),
                        Date = CurrentDateTime.GetCurrentDate(),
                        TransactionType = TransactionType.Credited,
                        Amount = InterestAmount,
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
                    account.TotalBalance+=Amount;
                    account.TotalBalance+= InterestAmount;
                    DbHandler.UpdateBalance(account);
                }
                BankingNotification.BankingNotification.NotifyAccountBalanceEdited(account);
                ZResponse<bool> Response = new ZResponse<bool>();
                Response.Response = "FD Closed successfully";
                Response.Data = true;
                response.OnResponseSuccess(Response);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
