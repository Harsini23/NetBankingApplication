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
        void ValidateUserLogin(UserLoginRequest request, UserLoginResponse response);//call back
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
        void VerfiedUser(IResponseType<LoginResponse> response);
        void BlockAccount(IResponseType<LoginResponse> response);
        void LoginFailed(IResponseType<LoginResponse> response);
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
            this.LoginDataManager.ValidateUserLogin(LoginRequest, new UserLoginResponse(this));
        }
        public override bool GetIfAvailableInCache()
        {
            return false;
        }

        public class UserLoginResponse : IResponseType<LoginResponse>
        {
            private Login login;
            public UserLoginResponse(Login login)
            {
                this.login = login;
            }

            public string Response { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public void OnResponseError(IResponseType<LoginResponse> response)
            {
                login.LoginResponse.LoginFailed( response);
            }
            public void OnResponseFailure(IResponseType<LoginResponse> response)
            {
               login.LoginResponse.LoginFailed(response);
            }
            public void OnResponseSuccess(IResponseType<LoginResponse> response)
            {
                /////////////////////////////////////////
                login.LoginResponse.VerfiedUser(response);
            }
        }
    }
}

