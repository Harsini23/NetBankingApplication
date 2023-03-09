using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.UseCase
{
    public interface ICheckPasswordDataManager
    {
        void VerifyPassword(CheckPasswordRequest request, IUsecaseCallbackBaseCase<bool> response);
    }
    public class CheckPasswordRequest
    {
        public UserPasswordBObj CheckCredential { get; set; }
        public CheckPasswordRequest(UserPasswordBObj cerdential)
        {
            CheckCredential = cerdential;
        }
    }
    public class CheckPassword:UseCaseBase<bool>
    {
        private ICheckPasswordDataManager CheckPasswordDataManager;
        private CheckPasswordRequest CheckPasswordRequest;
        IPresenterCheckPasswordCallback presenterCheckPasswordCallback;
        public CheckPassword(CheckPasswordRequest request, IPresenterCheckPasswordCallback responseCallback)
        {
            CheckPasswordDataManager = ServiceProvider.GetInstance().Services.GetService<ICheckPasswordDataManager>();
            CheckPasswordRequest = request;
            presenterCheckPasswordCallback = responseCallback;
        }

        public override void Action()
        {
            this.CheckPasswordDataManager.VerifyPassword(CheckPasswordRequest, new CheckPasswordCallback(this));
        }


        public class CheckPasswordCallback : IUsecaseCallbackBaseCase<bool>
        {
            private CheckPassword checkPassword;

            public CheckPasswordCallback(CheckPassword checkPassword)
            {
                this.checkPassword = checkPassword;
            }

            public void OnResponseError(BException response)
            {
                checkPassword.presenterCheckPasswordCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
             checkPassword.presenterCheckPasswordCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                checkPassword.presenterCheckPasswordCallback?.OnSuccessAsync(response);
            }
        }

    }
    public interface IPresenterCheckPasswordCallback : IResponseCallbackBaseCase<bool>
    {
    }
}
