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
        void ValidateUserLogin(UserLoginRequest request, UserLoginCallback response);//call back
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
        private ILoginDataManager LoginDataManager;
        private UserLoginRequest LoginRequest;
        IPresenterLoginCallback LoginResponseCallback;
        public Login(UserLoginRequest request, IPresenterLoginCallback responseCallback) : base(responseCallback,request.CtsSource)
        {
            var serviceProviderInstance= ServiceProvider.GetInstance();
            LoginDataManager = serviceProviderInstance.Services.GetService<ILoginDataManager>();
            LoginRequest = request;
            LoginResponseCallback = responseCallback;
        }
        public override void Action()
        {
            //use call back
            this.LoginDataManager.ValidateUserLogin(LoginRequest, new UserLoginCallback(this));
        }
        public override bool GetIfAvailableInCache()
        {
            return false;
        }
        public class UserLoginCallback : ZResponse<LoginResponse>
        {
            private Login login;
            public UserLoginCallback(Login login)
            {
                this.login = login;
            }

            public string Response { get; set; }

            public void OnResponseError(String response)
            {
                login.LoginResponseCallback?.OnError( response);

            }
            public void OnResponseFailure(ZResponse<LoginResponse> response)
            {
               login.LoginResponseCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<LoginResponse> response)
            {
                login.LoginResponseCallback?.OnSuccess(response);
            }
        }
    }

   

}

