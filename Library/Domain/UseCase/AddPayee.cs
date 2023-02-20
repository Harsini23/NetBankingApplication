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
        void AddNewPayee(AddPayeeRequest request, AddPayeeCallback response);
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
        private IAddPayeeDataManager AddPayeeDataManager;
        private AddPayeeRequest AddPayeeRequest;
        IPresenterAddPayeeCallback AddPayeeResponse;
        public AddPayee(AddPayeeRequest request, IPresenterAddPayeeCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            AddPayeeDataManager = serviceProviderInstance.Services.GetService<IAddPayeeDataManager>();
            AddPayeeRequest = request;
            AddPayeeResponse = responseCallback;
        }   

        public override void Action()
        {
            //use call back
            this.AddPayeeDataManager.AddNewPayee(AddPayeeRequest, new AddPayeeCallback(this));
        }
        public class AddPayeeCallback : ZResponse<AddPayeeResponse>
        {
            AddPayee addPayee;
            public AddPayeeCallback(AddPayee addPayee)
            {
                this.addPayee = addPayee;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                addPayee.AddPayeeResponse?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<String> response)
            {
                addPayee.AddPayeeResponse?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<String> response)
            {
                addPayee.AddPayeeResponse?.OnSuccessAsync(response);

            }
        }
    }
}
