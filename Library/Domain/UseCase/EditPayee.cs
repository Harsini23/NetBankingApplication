using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.EditPayee;

namespace Library.Domain.UseCase
{
    public interface IEditPayeeDataManager
    {
        void EditPayee(EditPayeeRequest request, EditPayeeCallback response);//call back
    }

    public class EditPayeeRequest : IRequest
    {
        public string UserId { get; set; }
        public Payee EditedPayee { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public EditPayeeRequest(string userId, Payee payee, CancellationTokenSource cts)
        {
            UserId = userId;
            EditedPayee = payee;
            CtsSource = cts;
        }
    }

    public interface IPresenterEditPayeeCallback : IResponseCallbackBaseCase<String>
    {
    }
    public class EditPayee: UseCaseBase<String>
    {
        private IEditPayeeDataManager _editPayeeDataManager;
        private EditPayeeRequest _editPayeeRequest;
        IPresenterEditPayeeCallback _editPayeeResponseCallback;
        public EditPayee(EditPayeeRequest request, IPresenterEditPayeeCallback responseCallback)
        {
            _editPayeeDataManager = ServiceProvider.GetInstance().Services.GetService<IEditPayeeDataManager>();
            _editPayeeRequest = request;
            _editPayeeResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this._editPayeeDataManager.EditPayee(_editPayeeRequest, new EditPayeeCallback(this));
        }


        public class EditPayeeCallback : ZResponse<String>
        {
            EditPayee _editPayee;
            public EditPayeeCallback(EditPayee editPayee)
            {
                _editPayee = editPayee;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _editPayee._editPayeeResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<String> response)
            {
                _editPayee._editPayeeResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<String> response)
            {
                _editPayee._editPayeeResponseCallback?.OnSuccessAsync(response);

            }
        }
    }
}
