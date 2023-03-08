using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.UseCase
{

    public interface IUpdateUserDataManager
    {
        void UpdateUser(UpdateUserRequest request, IUsecaseCallbackBaseCase<User> response);
    }
    public class UpdateUser:UseCaseBase<String>
    {
        IUpdateUserDataManager updateUserDataManager;
        IPresenterUpdateUserCallback presenterUpdateUserCallback;
        UpdateUserRequest updateUserRequest;
        public UpdateUser(UpdateUserRequest request, IPresenterUpdateUserCallback responseCallback)
        {
            updateUserDataManager = ServiceProvider.GetInstance().Services.GetService<IUpdateUserDataManager>();
            presenterUpdateUserCallback = responseCallback;
            updateUserRequest = request;
        }
        public override void Action()
        {
            this.updateUserDataManager.UpdateUser(updateUserRequest, new UpdateUserCallback(this));
        }

        public class UpdateUserCallback : IUsecaseCallbackBaseCase<User>
        {
            UpdateUser UpdateUser;
            public UpdateUserCallback(UpdateUser updateUser)
            {
                UpdateUser = updateUser;
            }
            public void OnResponseError(BException response)
            {
               UpdateUser.presenterUpdateUserCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<User> response)
            {
                UpdateUser.presenterUpdateUserCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<User> response)
            {
                UpdateUser.presenterUpdateUserCallback?.OnSuccessAsync(response);
            }
        }
    }

  
    public class UpdateUserRequest
    {
       public User UpdatedUser { get; set; }
        public UpdateUserRequest(User updatedUser, string userId)
        {
            UpdatedUser = updatedUser;
        }
        public UpdateUserRequest()
        {

        }
    }

    public interface IPresenterUpdateUserCallback : IResponseCallbackBaseCase<User>
    {
    }
}
