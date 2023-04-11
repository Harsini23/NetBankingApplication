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
        private AddUser _addNewUser;
        public override void AddUser(UserAccountDetails userDetails)
        {
            AddUserRequest request = new AddUserRequest(userDetails, "userid");
            _addNewUser = new AddUser(request, new PresenterAddUserCallback(this) );
            _addNewUser.Execute();
        }
    }

    public class PresenterAddUserCallback : IPresenterAddUserCallback
    {
        private AddUserViewModel _addUserViewModel;
        public PresenterAddUserCallback(AddUserViewModel addUserViewModel)
        {   
            this._addUserViewModel = addUserViewModel;
        }
        public async void OnError(BException response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _addUserViewModel.Response = response.exceptionMessage;
                _addUserViewModel.addUserNotification?.NotificationUpdate();
            });
        }

        public void OnFailure(ZResponse<AddUserResponse> response)
        {
          
        }

        public async void OnSuccessAsync(ZResponse<AddUserResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _addUserViewModel.Response = response.Response;
                _addUserViewModel.addUserNotification?.NotificationUpdate();
                if (!response.Data.UserExists)
                {
                    _addUserViewModel.UserId = response.Data.Credentials.UserId;
                    _addUserViewModel.Password = response.Data.Credentials.Password;
                    _addUserViewModel.AccountNo = response.Data.Account.AccountNumber;
                    _addUserViewModel.adduserView?.ShowContentDialogueAsync();

                }
             
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
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        private string _userId = String.Empty;
        public string UserId
        {
            get { return this._userId; }
            set
            {
                _userId = value;
                OnPropertyChanged(nameof(UserId));
            }
        }

        private string _password = String.Empty;
        public string Password
        {
            get { return this._password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _response = String.Empty;
        public string Response
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }

        private string _accountNo = String.Empty;
        public string AccountNo
        {
            get { return this._accountNo; }
            set
            {
                _accountNo = value;
                OnPropertyChanged(nameof(AccountNo));
            }
        }
        public abstract void AddUser(UserAccountDetails userDetails);
        public IAddUserView adduserView;
        public ShowResponseNotification addUserNotification;
    }

    public interface IAddUserView
    {
         void ShowContentDialogueAsync();
    }
    public interface ShowResponseNotification
    {
        void NotificationUpdate();
    }
}
