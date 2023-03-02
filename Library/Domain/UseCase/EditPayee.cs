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
        private IEditPayeeDataManager DeletePayeeDataManager;
        private EditPayeeRequest EditPayeeRequest;
        IPresenterEditPayeeCallback EditPayeeResponseCallback;
        public EditPayee(EditPayeeRequest request, IPresenterEditPayeeCallback responseCallback)
        {
            DeletePayeeDataManager = ServiceProvider.GetInstance().Services.GetService<IEditPayeeDataManager>();    
            EditPayeeRequest = request;
            EditPayeeResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this.DeletePayeeDataManager.EditPayee(EditPayeeRequest, new EditPayeeCallback(this));
        }


        public class EditPayeeCallback : ZResponse<String>
        {
            EditPayee EditPayee;
            public EditPayeeCallback(EditPayee editPayee)
            {
                EditPayee = editPayee;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                EditPayee.EditPayeeResponseCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<String> response)
            {
                EditPayee.EditPayeeResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<String> response)
            {
                EditPayee.EditPayeeResponseCallback?.OnSuccessAsync(response);

            }
        }
    }
}
