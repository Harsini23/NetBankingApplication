using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.DeletePayee;

namespace Library.Domain.UseCase
{
    public interface IDeletePayeeDataManager
    {
        void DeletePayee(DeletePayeeRequest request, DeletePayeeCallback response);//call back
    }

    public class DeletePayeeRequest : IRequest
    {
        public string UserId { get; set; }
        public Payee payeeToDelete { get; set; }

        public DeletePayeeRequest(string userId, Payee payee)
        {
            UserId = userId;
            payeeToDelete = payee;
        }
    }

    public interface IPresenterDeletePayeeCallback
    {
        void OnSuccess(ZResponse<String> response);
        void OnError(ZResponse<String> response);
        void OnFailure(ZResponse<String> response);
    }
    public class DeletePayee : UseCaseBase
    {
        private IDeletePayeeDataManager DeletePayeeDataManager;
        private DeletePayeeRequest DeletePayeeRequest;
        IPresenterDeletePayeeCallback DeletePayeeResponse;
        public DeletePayee(DeletePayeeRequest request, IPresenterDeletePayeeCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            DeletePayeeDataManager = serviceProviderInstance.Services.GetService<IDeletePayeeDataManager>();
            DeletePayeeRequest = request;
            DeletePayeeResponse = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this.DeletePayeeDataManager.DeletePayee(DeletePayeeRequest, new DeletePayeeCallback(this));
            // this.GetAllPayeeDataManager.ValidateUserLogin(GetAllPayeeRequest, new GetAllPayeeCallback(this));
        }

        public class DeletePayeeCallback : ZResponse<String>
        {
            private DeletePayee DeletePayee;
            public DeletePayeeCallback(DeletePayee deletePayee)
            {
                this.DeletePayee = deletePayee;
            }
            public string Response { get; set; }

            public void OnResponseError(ZResponse<String> response)
            {
                DeletePayee.DeletePayeeResponse.OnError(response);
            }
            public void OnResponseFailure(ZResponse<String> response)
            {
                DeletePayee.DeletePayeeResponse.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<String> response)
            {
                DeletePayee.DeletePayeeResponse.OnSuccess(response);

            }
        }
    }
}
