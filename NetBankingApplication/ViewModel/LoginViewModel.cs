using Library.Data.DataManager;using Library.Domain;using System;using System.Collections.Generic;using System.ComponentModel;using System.Diagnostics;using System.Linq;using System.Runtime.CompilerServices;
using System.Text;using System.Threading.Tasks;using static Library.Domain.Login;namespace NetBankingApplication.ViewModel{

    internal class LoginViewModel : LoginBaseViewModel, ILoginViewModel    {
        
        Login login;
        public string userId, password;
     
        public override void CallUseCase()        {
            login = new Login(new UserLoginRequest(userId, password), new PresenterLoginCallback(this));
            login.Execute();        }        public override void PopulateData()        {
            //set data in view
        }        public void ValidateUserInput(string userId, string password)        {            this.userId = userId;            this.password = password;
            //check password specification and UI logic is correct, or if needed - set binded value proceed to call use case or handle UI error
            CallUseCase();        }
        public class PresenterLoginCallback : IPresenterLoginCallback        {            private LoginViewModel loginViewModel;            public PresenterLoginCallback(LoginViewModel loginViewModel)            {                this.loginViewModel = loginViewModel;            }            public void BlockAccount(ZResponse<LoginResponse> response)            {                Debug.WriteLine("--------------------");                Debug.WriteLine(response.Response.ToString());                Debug.WriteLine("--------------------");            }            public void LoginFailed(ZResponse<LoginResponse> response)            {                Debug.WriteLine("--------------------");                Debug.WriteLine(response.Response.ToString());                Debug.WriteLine("--------------------");            }            public void VerfiedUser(ZResponse<LoginResponse> response)            {                Debug.WriteLine("Verified user :--------------------");                Debug.WriteLine(response.Response.ToString());               // Debug.WriteLine(response.Data.currentUser.UserId.ToString());

                loginViewModel.LoginResponseValue = response.Response.ToString();
                                Debug.WriteLine("--------------------");            }        }    }
}