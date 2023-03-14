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
        GetAllUsers getAllUsers;
        public override void GetAllUsers()
        {
            getAllUsers = new GetAllUsers(new GetAllUserRequest("", new CancellationTokenSource()), new PresenterGetAllUsersCallback(this));
            getAllUsers.Execute();
        }
    }
    public class PresenterGetAllUsersCallback : IPresenterGetAllUsersCallback
    {
        private GetAllUsersViewModel getAllUsersViewModel;

        public PresenterGetAllUsersCallback(GetAllUsersViewModel getAllUsersViewModel)
        {
            this.getAllUsersViewModel = getAllUsersViewModel;
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
                getAllUsersViewModel.AllUsers = response.Data.Data;
            });
        }
    }
    public abstract class GetAllUsersBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetAllUsers();
        public ObservableCollection<User> AllUsers = new ObservableCollection<User>();
    }
}

