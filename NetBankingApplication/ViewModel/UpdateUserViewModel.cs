using Library;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class UpdateUserViewModel : UpdateUserBaseViewModel
    {
        UpdateUser updateUser;
        public override void UpdateUser(User user)
        {
            updateUser = new UpdateUser(new UpdateUserRequest(user,user.UserId),new PresenterUpdateUserCallback(this)) ;
            updateUser.Execute();
        }
    }

    public class PresenterUpdateUserCallback : IPresenterUpdateUserCallback
    {
        private UpdateUserViewModel _updateUserViewModel;
        public PresenterUpdateUserCallback(UpdateUserViewModel updateUserViewModel)
        {
            _updateUserViewModel = updateUserViewModel;
        }
        NotificationServiceUser eventProvider = new NotificationServiceUser();

        public async void OnError(BException errorMessage)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _updateUserViewModel.ResponseValue =errorMessage.exceptionMessage;
                _updateUserViewModel.settingsView.UpdateUserNotification();
            });
        }

        public void OnFailure(ZResponse<User> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<User> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _updateUserViewModel.CurrentUser = response.Data;
                _updateUserViewModel.CurrentUserInitial = response.Data.UserName.Substring(0, 1)[0];
                eventProvider.Subscribe(new UserUpdate());
                eventProvider.RaiseEvent(response.Data);
                _updateUserViewModel.ResponseValue = response.Response;
                _updateUserViewModel.settingsView.UpdateUserNotification();
            });
        }
    }
    public abstract class UpdateUserBaseViewModel : NotifyPropertyBase
    {
        public IUserUpdateNotification settingsView { get;set; }
        private string _response = String.Empty;
        public string ResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(ResponseValue));
            }
        }
        public abstract void UpdateUser(User user);

        private User _currentUser;
        public User CurrentUser
        {
            get { return this._currentUser; }
            set
            {
                _currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }


        private char _currentUserInitial;
        public char CurrentUserInitial
        {
            get { return this._currentUserInitial; }
            set
            {
                _currentUserInitial = value;
                OnPropertyChanged(nameof(CurrentUserInitial));
            }
        }

    }

    public interface IUserUpdateNotification
    {
        void UpdateUserNotification();
    }
}
