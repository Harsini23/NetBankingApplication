using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using Library.Util;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class OpenFDDataManager : BankingDataManager, IOpenFDDataManager
    {
        AddAccountDataManager addAccountDataManager;
        TransferAmountDataManager transferAmountDataManager;
        public OpenFDDataManager(IDbHandler DbHandler, INetHandler NetHandler,AddAccountDataManager addAccountDataManager, TransferAmountDataManager transferAmountDataManager) : base(DbHandler, NetHandler)
        {
            addAccountDataManager = addAccountDataManager;
            transferAmountDataManager = transferAmountDataManager;
        }
        public void OpenFD(OpenFDRequest request, OpenFD.OpenFDCallback callback)
        {
            ZResponse<bool> response = new ZResponse<bool>();
            //populate in FDAccount table
            var account = new AccountBObj
            {
                UserId = request.FDAccountBObj.UserID,
                AccountType = AccountType.FDAccount,
                TotalBalance = request.FDAccountBObj.Principle,
                Currency = Currency.INR,
                BId = "B001",
                Name = DbHandler.GetUserName(request.FDAccountBObj.UserID),
                AccountNumber=request.FDAccountBObj.FromAccount,
                AvailableBalanceAsOn=CurrentDateTime.GetCurrentDate(),  
            };
            if (request.FDAccountBObj.Principle <= 0)
            {
                response.Response ="Enter valid amount";
                response.Data = false;
            }
            else if (transferAmountDataManager.ValidateCurrentAccountAndDeductBalance(account, request.FDAccountBObj.Principle)){
                AddAccountRequest addAccountRequest = new AddAccountRequest(account, request.FDAccountBObj.UserID);
                request.FDAccountBObj.AccountNumber = addAccountDataManager?.PopulateDataForNewAccountCreation(addAccountRequest, TransactionType.FDTransation);
                DbHandler.AddFDAccount(request.FDAccountBObj);
                response.Response = "Sucessfully added account";
                response.Data = true;
            }
            else
            {

                response.Response = "Insufficient balance";
                response.Data = false;
            }
            callback.OnResponseSuccess(response);
            //send AddAccountRequest with accountBObj and userId

        }
    }
}

//string userId, AccountType accountType, double totalBalance, Currency currency, string bId, string name)