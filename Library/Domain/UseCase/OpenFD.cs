using Library.Model;
using Library.Util.FDCalculator;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.OpenFD;

namespace Library.Domain.UseCase
{
    public interface IOpenFDDataManager
    {
        void OpenFD(OpenFDRequest request, OpenFDCallback response);//call back
    }

    public class OpenFDRequest
    {
        public CancellationTokenSource CtsSource { get; set; }
        public FDAccountBObj FDAccountBObj { get; set; }
        public OpenFDRequest(FDAccountBObj fDAccountBObj, CancellationTokenSource ctsSource)
        {
            FDAccountBObj = fDAccountBObj;
            CtsSource = ctsSource;
        }
    }
    public interface IPresenterOpenFDCallback : IResponseCallbackBaseCase<bool>
    {
    }
    public class OpenFD : UseCaseBase<String>
    {
        private IOpenFDDataManager OpenFDDataManager;
        private OpenFDRequest OpenFDRequest;
        IPresenterOpenFDCallback OpenFDResponseCallback;
        public OpenFD(OpenFDRequest request, IPresenterOpenFDCallback responseCallback)
        {
            OpenFDDataManager = ServiceProvider.GetInstance().Services.GetService<IOpenFDDataManager>();
            OpenFDRequest = request;
            OpenFDResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this.OpenFDDataManager.OpenFD(OpenFDRequest, new OpenFDCallback(this));
        }

        public class OpenFDCallback : ZResponse<String>
        {
            OpenFD OpenFD;
            public OpenFDCallback(OpenFD openFD)
            {
                OpenFD = openFD;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                OpenFD.OpenFDResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<bool> response)
            {
                OpenFD.OpenFDResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<bool> response)
            {
                //  response.Data.FDDetails = GetFDRate.CalculateFD(response.Data.Data);
                OpenFD.OpenFDResponseCallback?.OnSuccessAsync(response);

            }
        }
    }

}
