﻿using Library.Data.DataManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.Overview;

namespace Library.Domain.UseCase
{
    public interface IOverviewDataManager
    {
        void GetOverviewData(OverviewRequest request, OverviewCallback response);//call back
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
        private IOverviewDataManager overviewDataManager;
        private OverviewRequest overviewRequest;
        private IPresenterOverviewCallback presenterOverviewCallback;
        public Overview(OverviewRequest request, IPresenterOverviewCallback responseCallback):base(responseCallback,request.CtsSource)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
           // overviewDataManager = serviceProviderInstance.Services.GetService<IOverviewDataManager>();
        }
        public override void Action()
        {
            //use call back
           
        }

        public class OverviewCallback : ZResponse<OverviewResponse>
        {
            private Login login;
            public OverviewCallback(Login login)
            {
                this.login = login;
            }

            public string Response { get; set; }

            public void OnResponseError(ZResponse<OverviewResponse> response)
            {
                
            }
            public void OnResponseFailure(ZResponse<OverviewResponse> response)
            {
               
            }
            public void OnResponseSuccess(ZResponse<OverviewResponse> response)
            {
                
            }
        }
    }
}
