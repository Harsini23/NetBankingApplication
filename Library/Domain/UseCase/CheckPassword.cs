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
        private ICheckPasswordDataManager _checkPasswordDataManager;
        private CheckPasswordRequest _checkPasswordRequest;
        IPresenterCheckPasswordCallback _presenterCheckPasswordCallback;
        public CheckPassword(CheckPasswordRequest request, IPresenterCheckPasswordCallback responseCallback)
        {
            _checkPasswordDataManager = ServiceProvider.GetInstance().Services.GetService<ICheckPasswordDataManager>();
            _checkPasswordRequest = request;
            _presenterCheckPasswordCallback = responseCallback;
        }

        public override void Action()
        {
            this._checkPasswordDataManager.VerifyPassword(_checkPasswordRequest, new CheckPasswordCallback(this));
        }


        public class CheckPasswordCallback : IUsecaseCallbackBaseCase<bool>
        {
            private CheckPassword _checkPassword;

            public CheckPasswordCallback(CheckPassword checkPassword)
            {
                this._checkPassword = checkPassword;
            }

            public void OnResponseError(BException response)
            {
                _checkPassword._presenterCheckPasswordCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<bool> response)
            {
                _checkPassword._presenterCheckPasswordCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<bool> response)
            {
                _checkPassword._presenterCheckPasswordCallback?.OnSuccessAsync(response);
            }
        }

    }
    public interface IPresenterCheckPasswordCallback : IResponseCallbackBaseCase<bool>
    {
    }
}
