﻿using Library.Data.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.Overview;
using Microsoft.Extensions.DependencyInjection;
using Library.Model;

namespace Library.Domain.UseCase
{
    public interface IOverviewDataManager
    {
        void GetOverviewData(OverviewRequest request, IUsecaseCallbackBaseCase<OverviewResponse> response);//call back
    }

    public class OverviewRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public OverviewRequest(string userId,CancellationTokenSource cts)
        {
            UserId = userId;
            CtsSource = cts;
        }
    }
    public interface IPresenterOverviewCallback : IResponseCallbackBaseCase<OverviewResponse>
    {
    } 

    public class Overview:UseCaseBase<OverviewResponse>
    {
        private IOverviewDataManager _overviewDataManager;
        private OverviewRequest _overviewRequest;
        private IPresenterOverviewCallback _presenterOverviewCallback;
        public Overview(OverviewRequest request, IPresenterOverviewCallback responseCallback):base(responseCallback,request.CtsSource)
        {
            _overviewDataManager = ServiceProvider.GetInstance().Services.GetService<IOverviewDataManager>();
            _overviewRequest = request;
            _presenterOverviewCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._overviewDataManager.GetOverviewData(_overviewRequest, new OverviewCallback(this));   
        }

        public class OverviewCallback : IUsecaseCallbackBaseCase<OverviewResponse>
        {
            private Overview overview;
            public OverviewCallback(Overview overview)
            {
                this.overview = overview;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
               
            }

            public void OnResponseFailure(ZResponse<OverviewResponse> response)
            {
               
            }
            public void OnResponseSuccess(ZResponse<OverviewResponse> response)
            {
                overview._presenterOverviewCallback?.OnSuccessAsync(response);
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

            public string CurrentMonth;
            public string PrimaryAccountBalance;
            public Dictionary<String, double> AccountAndBalance = new Dictionary<string, double>();
        }
    }
}
