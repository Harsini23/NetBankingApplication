using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.Login;

namespace Library.Domain
{
    public interface ILoginDataManager
    {
        void ValidateUserLogin(UserLoginRequest request, IUsecaseCallbackBaseCase<LoginResponse> response);//call back
    }
    public class UserLoginRequest : IRequest
    {
        public string UserId { get; set; }  
        public string Password { get; set; }
        public CancellationTokenSource CtsSource { get ; set ; }

        public UserLoginRequest(string userId, string password,CancellationTokenSource _cts)
        {
            UserId = userId;
            Password = password;
            CtsSource =_cts;
        }
    }
    public interface IPresenterLoginCallback: IResponseCallbackBaseCase<LoginResponse>
    {
    }
    public class Login : UseCaseBase<LoginResponse>
    {
        private ILoginDataManager _loginDataManager;
        private UserLoginRequest _loginRequest;
        IPresenterLoginCallback _loginResponseCallback;
        public Login(UserLoginRequest request, IPresenterLoginCallback responseCallback) : base(responseCallback,request.CtsSource)
        {
            var serviceProviderInstance= ServiceProvider.GetInstance();
            _loginDataManager = serviceProviderInstance.Services.GetService<ILoginDataManager>();
            _loginRequest = request;
            _loginResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this._loginDataManager.ValidateUserLogin(_loginRequest, new UserLoginCallback(this));
        }
        public override bool GetIfAvailableInCache()
        {
            return false;
        }
        public class UserLoginCallback : IUsecaseCallbackBaseCase<LoginResponse>
        {
            private Login _login;
            public UserLoginCallback(Login login)
            {
                this._login = login;
            }

            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _login._loginResponseCallback?.OnError( response);

            }
            public void OnResponseFailure(ZResponse<LoginResponse> response)
            {
                _login._loginResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<LoginResponse> response)
            {
                _login._loginResponseCallback?.OnSuccessAsync(response);
            }
        }

        public class LoginResponse : ZResponse<User>
        {
            public User currentUser;
            public Admin currentAdmin;
            public bool NewUser;
            public bool IsAdmin;

        }
    }

   

}

