using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.UseCase
{
    public interface ICloseFDDataManager
    {
        void CloseFD(CloseFDRequest request, IUsecaseCallbackBaseCase<bool> response);
    }
    public class CloseFDRequest : IRequest
    {
        public string UserId { get; set; }
        public FDAccount FDAccount { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public CloseFDRequest(string userId,FDAccount fDAccount, CancellationTokenSource cts)
        {
            UserId = userId;
            FDAccount = fDAccount;
            CtsSource = cts;
        }

        public CloseFDRequest()
        {
        }
    }
 
    public interface IPresenterCloseFDCallback : IResponseCallbackBaseCase<bool> { }
    public class CloseFD : UseCaseBase<bool>
    {
        private ICloseFDDataManager CloseFDDataManager;
        private CloseFDRequest CloseFDRequest;
        IPresenterCloseFDCallback CloseFDResponseCallback;
        public CloseFD(CloseFDRequest request, IPresenterCloseFDCallback responseCallback)
        {
            CloseFDDataManager = ServiceProvider.GetInstance().Services.GetService<ICloseFDDataManager>();
            CloseFDRequest = request;
            CloseFDResponseCallback = responseCallback;
        }

        public override void Action()
        {
            //use call back
            this.CloseFDDataManager.CloseFD(CloseFDRequest, new CloseFDCallback(this));
        }
        public class CloseFDCallback : IUsecaseCallbackBaseCase<bool>
        {
            private CloseFD closeFD;

            public CloseFDCallback(CloseFD closeFD)
            {
                this.closeFD = closeFD;
            }

            public void OnResponseError(BException response)
            {
                closeFD.CloseFDResponseCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                closeFD.CloseFDResponseCallback?.OnFailure(response);   
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                closeFD.CloseFDResponseCallback?.OnSuccessAsync(response);
            }
        }
    }
}
