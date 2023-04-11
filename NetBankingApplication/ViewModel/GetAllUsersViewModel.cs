using Library;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class GetAllUsersViewModel : GetAllUsersBaseViewModel
    {
       private GetAllUsers _getAllUsers;
        public override void GetAllUsers()
        {
            _getAllUsers = new GetAllUsers(new GetAllUserRequest("", new CancellationTokenSource()), new PresenterGetAllUsersCallback(this));
            _getAllUsers.Execute();
        }
    }
    public class PresenterGetAllUsersCallback : IPresenterGetAllUsersCallback
    {
        private GetAllUsersViewModel _getAllUsersViewModel;

        public PresenterGetAllUsersCallback(GetAllUsersViewModel getAllUsersViewModel)
        {
            this._getAllUsersViewModel = getAllUsersViewModel;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<GetAllUsersResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<GetAllUsersResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _getAllUsersViewModel.AllUsers = response.Data.Data;
            });
        }
    }
    public abstract class GetAllUsersBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetAllUsers();
        public ObservableCollection<User> AllUsers = new ObservableCollection<User>();
    }
}

