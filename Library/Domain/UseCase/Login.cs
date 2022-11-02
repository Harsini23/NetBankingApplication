using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public UserLoginRequest(string userId, string password)
        {
            UserId = userId;
            Password = password;
        }
    }
    public interface IPresenterLoginCallback
    {
        void VerfiedUserAsync(ZResponse<LoginResponse> response);
        void BlockAccount(ZResponse<LoginResponse> response);
        void LoginFailed(ZResponse<LoginResponse> response);
    }
    public class Login : UseCaseBase
    {
        private ILoginDataManager LoginDataManager;
        private UserLoginRequest LoginRequest;
        IPresenterLoginCallback LoginResponse;
        public Login(UserLoginRequest request, IPresenterLoginCallback responseCallback)
        {
            var serviceProviderInstance= ServiceProvider.GetInstance();
            LoginDataManager = serviceProviderInstance.Services.GetService<ILoginDataManager>();
            LoginRequest = request;
            LoginResponse= responseCallback;
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

            public void OnResponseError(ZResponse<LoginResponse> response)
            {
                login.LoginResponse.LoginFailed( response);
            }
            public void OnResponseFailure(ZResponse<LoginResponse> response)
            {
               login.LoginResponse.LoginFailed(response);
            }
            public void OnResponseSuccess(ZResponse<LoginResponse> response)
            {
                login.LoginResponse.VerfiedUserAsync(response);
               
            }
        }
    }
}

