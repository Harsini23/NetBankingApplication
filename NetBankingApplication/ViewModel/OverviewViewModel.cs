using Library;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.Overview;

namespace NetBankingApplication.ViewModel
{


    public class OverviewViewModel : OverviewBaseViewModel
    {
        public Overview overview;
        public override void getData(string userId)
        {
            overview = new Overview(new OverviewRequest(userId,new CancellationTokenSource()),new PresenterOverViewCallback(this));
            overview.Execute();
        }


        public override void setUser(User user)
        {
            User=user;
        }

        public class PresenterOverViewCallback : IPresenterOverviewCallback
        {
            OverviewViewModel OverviewViewModel;
            public PresenterOverViewCallback(OverviewViewModel overviewViewModel)
            {
                OverviewViewModel = overviewViewModel;
            }
            public void OnError(BException errorMessage)
            {
                throw new NotImplementedException();
            }

            public void OnFailure(ZResponse<OverviewResponse> response)
            {
                throw new NotImplementedException();
            }

            public async void OnSuccessAsync(ZResponse<OverviewResponse> response)
            {
                double expense=0.0;
              double total = double.Parse(response.Data.Income) + double.Parse(response.Data.Expense);
                if (total > 0)
                {
                     expense = (double.Parse(response.Data.Expense) / total) * 100;
                }
                else
                {
                    expense=double.Parse(response.Data.Balance);
                }

                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    OverviewViewModel.TotalBalance = response.Data.Balance;
                    OverviewViewModel.Income = response.Data.Income;
                    OverviewViewModel.Expense = response.Data.Expense;
                    OverviewViewModel.CurrentMonthExpense = response.Data.CurrentMonthExpense;
                    OverviewViewModel.CurrentMonthIncome = response.Data.CurrentMonthIncome;
                    OverviewViewModel.Month = response.Data.CurrentMonth;
                    OverviewViewModel.ExpensePercentage = expense;
                });

            }
        }
    }
        
    public abstract class OverviewBaseViewModel : NotifyPropertyBase
    {
        private User _user;
        public User User
        {
            get { return _user; }
            set
            {
                _user = value;
                OnPropertyChanged(nameof(User));
            }
        }


        private string _totalBalance = String.Empty;
        public string TotalBalance
        {
            get { return _totalBalance; }
            set
            {
                _totalBalance = value;
                OnPropertyChanged(nameof(TotalBalance));
            }
        }



        private string _income = String.Empty;
        public string Income
        {
            get { return _income; }
            set
            {
                _income = value;
                OnPropertyChanged(nameof(Income));
            }
        }

        private string _expense = String.Empty;
        public string Expense
        {
            get { return _expense; }
            set
            {
                _expense = value;
                OnPropertyChanged(nameof(Expense));
            }
        }

        private string _currentMonthIncome = String.Empty;
        public string CurrentMonthIncome
        {
            get { return _currentMonthIncome; }
            set
            {
                _currentMonthIncome = value;
                OnPropertyChanged(nameof(CurrentMonthIncome));
            }
        }
        private double _expensePercentage = 0.0;
        public double ExpensePercentage
        {
            get { return _expensePercentage; }
            set
            {
                _expensePercentage = value;
                OnPropertyChanged(nameof(ExpensePercentage));
            }
        }

        private string __currentMonthExpense = String.Empty;
        public string CurrentMonthExpense
        {
            get { return __currentMonthExpense; }
            set
            {
                __currentMonthExpense = value;
                OnPropertyChanged(nameof(CurrentMonthExpense));
            }
        }

        private string _month = String.Empty;
        public string Month
        {
            get { return _month; }
            set
            {
                _month = value;
                OnPropertyChanged(nameof(Month));
            }
        }

        public abstract void getData(string userId);
        public abstract void setUser(User userId);

    }


}
