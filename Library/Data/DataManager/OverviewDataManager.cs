using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class OverviewDataManager : BankingDataManager,IOverviewDataManager
    {
        public OverviewDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        public void GetOverviewData(OverviewRequest request, Overview.OverviewCallback response)
        {
            //validate and get records
            try
            {
                ZResponse<OverviewResponse> Response = new ZResponse<OverviewResponse>();
                OverviewResponse overViewResponse = new OverviewResponse();

                var userId = request.UserId;
               
                overViewResponse.Balance = DbHandler.GetTotalBalanceOfUser(userId).ToString("0.00");
               overViewResponse.Income= DbHandler.GetTotalIncome(userId).ToString("0.00");
               overViewResponse.Expense= DbHandler.GetTotalExpense(userId).ToString("0.00");

                //------------------------------
                var IncomeTransactionList = DbHandler.GetCurrentMonthIncome(userId);
                var ExpenseTransactionList = DbHandler.GetCurrentMonthExpense(userId);
                double monthlyIncome = 0.0, monthlyExpense = 0.0;
                foreach(var i in IncomeTransactionList)
                {
                    if(DateTime.Parse(i.Date).Month == DateTime.Now.Month)
                    monthlyIncome += i.Amount;
                }
                foreach (var i in ExpenseTransactionList)
                {
                    if (DateTime.Parse(i.Date).Month == DateTime.Now.Month)
                        monthlyExpense += i.Amount;
                }
                overViewResponse.CurrentMonthIncome = monthlyIncome.ToString("0.00");
                overViewResponse.CurrentMonthExpense = monthlyExpense.ToString("0.00");

                Response.Data = overViewResponse;
                var responseStatus = "Successfull";
                Response.Response = responseStatus;

                response.OnResponseSuccess(Response);
            }
            catch
            {
                //throw no such user exception
            }
        }
    
    }

    public class OverviewResponse : ZResponse<User>
    {
        public User CurrentUser;
        public bool NewUser;
        public Account CurrentAccount;
        public Card Card;
        public Branch Branch;
        public string Balance;

        public string Income;
        public string Expense;
        public string CurrentMonthIncome;
        public string CurrentMonthExpense;

        public string PrimaryAccountBalance;
        public Dictionary<String, double> AccountAndBalance = new Dictionary<string, double>();
    }
}
