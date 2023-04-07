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
        AddAccountDataManager AddAccountDataManager;
        TransferAmountDataManager TransferAmountDataManager;
        public OpenFDDataManager(IDbHandler DbHandler, INetHandler NetHandler,AddAccountDataManager addAccountDataManager, TransferAmountDataManager transferAmountDataManager) : base(DbHandler, NetHandler)
        {
            AddAccountDataManager = addAccountDataManager;
            TransferAmountDataManager = transferAmountDataManager;
        }
        public void OpenFD(OpenFDRequest request, OpenFD.OpenFDCallback response)
        {
            ZResponse<bool> Response = new ZResponse<bool>();
            //populate in FDAccount table
            var account = new AccountBObj
            {
                UserId = request.FDAccountBObj.UserID,
                AccountType = AccountType.FDAccount,
                TotalBalance = request.FDAccountBObj.Principle,
                Currency = Currency.INR,
                BId = "B001",
                Name = "FD Transaction",
                AccountNumber=request.FDAccountBObj.FromAccount,
                AvailableBalanceAsOn=CurrentDateTime.GetCurrentDate(),  
            };
            if (request.FDAccountBObj.Principle <= 0)
            {
                Response.Response ="Enter valid amount";
                Response.Data = false;
            }
            else if (TransferAmountDataManager.ValidateCurrentAccountAndDeductBalance(account, request.FDAccountBObj.Principle)){
                AddAccountRequest addAccountRequest = new AddAccountRequest(account, request.FDAccountBObj.UserID);
                request.FDAccountBObj.AccountNumber = AddAccountDataManager?.PopulateDataForNewAccountCreation(addAccountRequest, TransactionType.FDTransation);
                DbHandler.AddFDAccount(request.FDAccountBObj);
                Response.Response = "Sucessfully added account";
                Response.Data = true;
            }
            else
            {
               
                Response.Response = "Insufficient balance";
                Response.Data = false;
            }
            response.OnResponseSuccess(Response);

            //populate in Account table - call that DM?  so that it creates account user account and transaction 
            //send AddAccountRequest with accountBObj and userId

        }
    }
}

//string userId, AccountType accountType, double totalBalance, Currency currency, string bId, string name)