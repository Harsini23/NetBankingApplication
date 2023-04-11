using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetAllPayee;

namespace Library.Domain.UseCase
{
    public interface IGetAllPayeeDataManager
    {
        void GetAllPayee(GetAllPayeeRequest request, IUsecaseCallbackBaseCase<GetAllPayeeResponse> response);//call back
    }

    public class GetAllPayeeRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public GetAllPayeeRequest(string userId,CancellationTokenSource cts)
        {
            UserId = userId;
            CtsSource=cts;
        }
    }

    public interface IPresenterGetAllPayeeCallback: IResponseCallbackBaseCase<GetAllPayeeResponse>
    {
    }
    public class GetAllPayee:UseCaseBase<GetAllPayeeResponse>
    {

        private IGetAllPayeeDataManager _getAllPayeeDataManager;
        private GetAllPayeeRequest _getAllPayeeRequest;
        IPresenterGetAllPayeeCallback _getAllPayeeResponseCallback;
        public GetAllPayee(GetAllPayeeRequest request, IPresenterGetAllPayeeCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            _getAllPayeeDataManager = serviceProviderInstance.Services.GetService<IGetAllPayeeDataManager>();
            _getAllPayeeRequest = request;
            _getAllPayeeResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._getAllPayeeDataManager.GetAllPayee(_getAllPayeeRequest, new GetAllPayeeCallback(this));
            // this.GetAllPayeeDataManager.ValidateUserLogin(GetAllPayeeRequest, new GetAllPayeeCallback(this));
        }

        public class GetAllPayeeCallback : IUsecaseCallbackBaseCase<GetAllPayeeResponse>
        {
            private GetAllPayee _getAllPayee;
            public GetAllPayeeCallback(GetAllPayee GetAllPayee)
            {
                this._getAllPayee = GetAllPayee;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _getAllPayee._getAllPayeeResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<GetAllPayeeResponse> response)
            {
                _getAllPayee._getAllPayeeResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<GetAllPayeeResponse> response)
            {
                _getAllPayee._getAllPayeeResponseCallback?.OnSuccessAsync(response);

            }
        }

        public class GetAllPayeeResponse : ZResponse<Payee>
        {
            public List<Payee> AllRecipients;
        }
    }
}
