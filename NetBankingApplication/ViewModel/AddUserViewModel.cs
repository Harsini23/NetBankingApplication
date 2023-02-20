using Library;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class AddUserViewModel : AddUserBaseViewModel
    {
        AddUser addNewUser;
        public override void AddUser(UserAccountDetails userDetails)
        {
            AddUserRequest request = new AddUserRequest(userDetails, "userid");
            addNewUser = new AddUser(request, new PresenterAddUserCallback(this) );
            addNewUser.Execute();
        }
    }

    public class PresenterAddUserCallback : IPresenterAddUserCallback
    {
        private AddUserViewModel addUserViewModel;
        public PresenterAddUserCallback(AddUserViewModel addUserViewModel)
        {   
            this.addUserViewModel = addUserViewModel;
        }
        public void OnError(BException response)
        {
            
        }

        public void OnFailure(ZResponse<AddUserResponse> response)
        {
          
        }

        public async void OnSuccessAsync(ZResponse<AddUserResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                addUserViewModel.UserId = response.Data.credentials.UserId;
                addUserViewModel.Password = response.Data.credentials.Password;
                addUserViewModel.AccountNo = response.Data.account.AccountNumber;
            });
        }
    }

    public abstract class AddUserBaseViewModel : NotifyPropertyBase
    {
        private string _errorMessage = String.Empty;
        public string ErrorMessage
        {
            get { return this._errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChangedAsync(nameof(ErrorMessage));
                //SetProperty(ref _response, value);
            }
        }



        private string _userId = String.Empty;
        public string UserId
        {
            get { return this._userId; }
            set
            {
                _userId = value;
                OnPropertyChangedAsync(nameof(UserId));
                //SetProperty(ref _response, value);
            }
        }


        private string _password = String.Empty;
        public string Password
        {
            get { return this._password; }
            set
            {
                _password = value;
                OnPropertyChangedAsync(nameof(Password));
                //SetProperty(ref _response, value);
            }
        }

        private string _accountNo = String.Empty;
        public string AccountNo
        {
            get { return this._accountNo; }
            set
            {
                _accountNo = value;
                OnPropertyChangedAsync(nameof(AccountNo));
                //SetProperty(ref _response, value);
            }
        }












        public abstract void AddUser(UserAccountDetails userDetails);
    }
}
