﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public abstract class LoginBaseViewModel: NotifyPropertyBase
    {
     //   public string LoginResponse { get; set; }


        private string _response = String.Empty;
        public string LoginResponseValue
        {
            get { return this._response; }
            set
            {
                 _response = value;
                OnPropertyChangedAsync(nameof(LoginResponseValue));
                //SetProperty(ref _response, value);
            }
        }

        public string ResetPasswordResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChangedAsync(nameof(ResetPasswordResponseValue));
                //SetProperty(ref _response, value);
            }
        }

        public ILoginViewModelInterface LoginViewModelCallback { get; set; }

        public abstract void ValidateUserInput(string userId, string password);
        public abstract void ResetPassword(string newPassword);
        public abstract void CallUseCase();
    
    }

    public interface ILoginViewModelInterface
    {
        void SwitchToResetPasswordContainer();
        //set iloginview instance to service provider
    }

}
