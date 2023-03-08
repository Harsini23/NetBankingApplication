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
        private UpdateUserViewModel UpdateUserViewModel;
        public PresenterUpdateUserCallback(UpdateUserViewModel updateUserViewModel)
        {
            UpdateUserViewModel = updateUserViewModel;
        }
        NotificationServiceUser eventProvider = new NotificationServiceUser();

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<User> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<User> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {

                eventProvider.Subscribe(new UserUpdate());
                eventProvider.RaiseEvent(response.Data);
            });
        }
    }
    public abstract class UpdateUserBaseViewModel
    {
        public abstract void UpdateUser(User user);
    }
}
