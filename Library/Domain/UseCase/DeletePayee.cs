using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        public Payee PayeeToDelete { get; set; }
        public CancellationTokenSource CtsSource { get; set ; }

        public DeletePayeeRequest(string userId, Payee payee,CancellationTokenSource cts)
        {
            UserId = userId;
            PayeeToDelete = payee;
            CtsSource = cts;
        }
    }

    public interface IPresenterDeletePayeeCallback: IResponseCallbackBaseCase<String>
    {
    }
    public class DeletePayee : UseCaseBase<String>
    {
        private IDeletePayeeDataManager _deletePayeeDataManager;
        private DeletePayeeRequest _deletePayeeRequest;
        IPresenterDeletePayeeCallback _deletePayeeResponseCallback;
        public DeletePayee(DeletePayeeRequest request, IPresenterDeletePayeeCallback responseCallback)
        {
            _deletePayeeDataManager = ServiceProvider.GetInstance().Services.GetService<IDeletePayeeDataManager>();
            _deletePayeeRequest = request;
            _deletePayeeResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._deletePayeeDataManager.DeletePayee(_deletePayeeRequest, new DeletePayeeCallback(this));
            // this.GetAllPayeeDataManager.ValidateUserLogin(GetAllPayeeRequest, new GetAllPayeeCallback(this));
        }

        public class DeletePayeeCallback : ZResponse<String>
        {
            private DeletePayee _deletePayee;
            public DeletePayeeCallback(DeletePayee deletePayee)
            {
                this._deletePayee = deletePayee;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _deletePayee._deletePayeeResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<String> response)
            {
                _deletePayee._deletePayeeResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<String> response)
            {
                _deletePayee._deletePayeeResponseCallback?.OnSuccessAsync(response);

            }
        }
    }
}
