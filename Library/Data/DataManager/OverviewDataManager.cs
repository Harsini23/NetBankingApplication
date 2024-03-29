﻿using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.Overview;

namespace Library.Data.DataManager
{
    public class OverviewDataManager : BankingDataManager,IOverviewDataManager
    {
        public OverviewDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void GetOverviewData(OverviewRequest request, IUsecaseCallbackBaseCase<OverviewResponse> callback)
        {
            //validate and get records
            try
            {
                ZResponse<OverviewResponse> response = new ZResponse<OverviewResponse>();
                OverviewResponse overViewResponse = new OverviewResponse();

                var userId = request.UserId;
               
                overViewResponse.Balance = DbHandler.GetTotalBalanceOfUser(userId).ToString("0.00");
               overViewResponse.Income= DbHandler.GetTotalIncome(new UserTransactionType { UserId=userId,TransactionType=TransactionType.Credited}).ToString("0.00");
               overViewResponse.Expense= DbHandler.GetTotalExpense(new UserTransactionType { UserId = userId, TransactionType = TransactionType.Debited }).ToString("0.00");

                //------------------------------
                var incomeTransactionList = DbHandler.GetCurrentMonthIncome(new UserTransactionType { UserId=userId,TransactionType=TransactionType.Credited});
                var expenseTransactionList = DbHandler.GetCurrentMonthExpense(new UserTransactionType { UserId = userId, TransactionType = TransactionType.Debited });
                double monthlyIncome = 0.0, monthlyExpense = 0.0;
                foreach(var i in incomeTransactionList)
                {
                    if(DateTime.Parse(i.Date).Month == DateTime.Now.Month)
                    monthlyIncome += i.Amount;
                }
                foreach (var i in expenseTransactionList)
                {
                    if (DateTime.Parse(i.Date).Month == DateTime.Now.Month)
                        monthlyExpense += i.Amount;
                }
                overViewResponse.CurrentMonthIncome = monthlyIncome.ToString("0.00");
                overViewResponse.CurrentMonthExpense = monthlyExpense.ToString("0.00");
                overViewResponse.CurrentMonth = DateTime.Now.ToString("MMMM yyyy");

                response.Data = overViewResponse;
                var responseStatus = "Successfull";
                response.Response = responseStatus;

                callback.OnResponseSuccess(response);
            }
            catch
            {
                //throw no such user exception
            }
        }
    
    }

  
}
