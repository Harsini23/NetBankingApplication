﻿using Library.Data.DataManager;
using System.Text;

    internal class LoginViewModel : LoginBaseViewModel
        
        Login login;
        public string userId, password;
     
        public override void CallUseCase()
            login = new Login(new UserLoginRequest(userId, password), new PresenterLoginCallback(this));
            login.Execute();
            //set data in view
        }

        public override void ValidateUserInput(string userId, string password)
        {
            this.userId = userId;
            //check password specification and UI logic is correct, or if needed - set binded value proceed to call use case or handle UI error
            CallUseCase();
        }

        public class PresenterLoginCallback : IPresenterLoginCallback

                loginViewModel.LoginResponseValue = response.Response.ToString();
                
}