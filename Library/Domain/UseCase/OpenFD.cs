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
        private IOpenFDDataManager _openFDDataManager;
        private OpenFDRequest _openFDRequest;
        IPresenterOpenFDCallback _openFDResponseCallback;
        public OpenFD(OpenFDRequest request, IPresenterOpenFDCallback responseCallback)
        {
            _openFDDataManager = ServiceProvider.GetInstance().Services.GetService<IOpenFDDataManager>();
            _openFDRequest = request;
            _openFDResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this._openFDDataManager.OpenFD(_openFDRequest, new OpenFDCallback(this));
        }

        public class OpenFDCallback : ZResponse<String>
        {
            OpenFD _openFD;
            public OpenFDCallback(OpenFD openFD)
            {
                _openFD = openFD;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _openFD._openFDResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<bool> response)
            {
                _openFD._openFDResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<bool> response)
            {
                //  response.Data.FDDetails = GetFDRate.CalculateFD(response.Data.Data);
                _openFD._openFDResponseCallback?.OnSuccessAsync(response);

            }
        }
    }

}
