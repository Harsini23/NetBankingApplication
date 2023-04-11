using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.AddPayee;

namespace Library.Domain.UseCase
{

    public interface IAddPayeeDataManager
    {
        void AddNewPayee(AddPayeeRequest request, IUsecaseCallbackBaseCase<String> response);
    }
    public class AddPayeeRequest : IRequest
    {
        public string UserId { get; set; }
        public Payee NewPayee { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public AddPayeeRequest(string userId, Payee newPayee, CancellationTokenSource cts)
        {
            UserId = userId;
            NewPayee = newPayee;
            CtsSource = cts;
        }

        public AddPayeeRequest()
        {
        }
    }
    public interface IPresenterAddPayeeCallback : IResponseCallbackBaseCase<String> { }
    public class AddPayee:UseCaseBase<String>
    {
        private IAddPayeeDataManager _addPayeeDataManager;
        private AddPayeeRequest _addPayeeRequest;
        private IPresenterAddPayeeCallback _addPayeeResponseCallback;
        public AddPayee(AddPayeeRequest request, IPresenterAddPayeeCallback responseCallback)
        {
            _addPayeeDataManager = ServiceProvider.GetInstance().Services.GetService<IAddPayeeDataManager>();
            _addPayeeRequest = request;
            _addPayeeResponseCallback = responseCallback;
        }   

        public override void Action()
        {
            //use call back
            this._addPayeeDataManager.AddNewPayee(_addPayeeRequest, new AddPayeeCallback(this));
        }
        public class AddPayeeCallback : IUsecaseCallbackBaseCase<String>
        {
           private AddPayee _addPayee;
            public AddPayeeCallback(AddPayee addPayee)
            {
                this._addPayee = addPayee;
            }
            public void OnResponseError(BException response)
            {
                _addPayee._addPayeeResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<String> response)
            {
                _addPayee._addPayeeResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<String> response)
            {
                _addPayee._addPayeeResponseCallback?.OnSuccessAsync(response);

            }
        }
        public class AddPayeeResponse : ZResponse<String>
        {

        }
    }
}
