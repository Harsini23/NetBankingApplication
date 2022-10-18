using Library.Data.DataManager;
using Library.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.Login;

namespace NetBankingApplication.ViewModel
{
    
    internal class LoginViewModel : BaseViewModel, ILoginViewModel
    {
        Login login;
        public override void CallUseCase()
        {
            login = new Login(new UserLoginRequest("Harsh200", "ZohoAppx"), new PresenterLoginCallback());
            login.Execute();
        }
        public override void PopulateData()
        {
            //set data in view
        }
        public void ValidateUserInput()
        {
            //check password specification and UI logic is correct, or if needed - set binded value proceed to call use case or handle UI error

            CallUseCase();
        }
    }

    public class PresenterLoginCallback : IPresenterLoginCallback
    {
        public void BlockAccount(IResponseType<LoginResponse> response)
        {
           //call BaseViewModel.PopulateData();
            Console.WriteLine(response.Response);
        }
        public void LoginFailed(IResponseType<LoginResponse> response)
        {
            Debug.WriteLine("--------------------");
            LoginResponse res = (LoginResponse)response;
            Debug.WriteLine(res.Response + " This is the response!");
         
            Debug.WriteLine("--------------------");
        }
        public void VerfiedUser(IResponseType<LoginResponse> response)
        {
            Debug.WriteLine("--------------------");
            LoginResponse res = (LoginResponse)response;
            Debug.WriteLine(res.Response+" This is the response!");
            Debug.WriteLine(res.currentUser.UserName);
            Debug.WriteLine("--------------------");

        }
    }
}
