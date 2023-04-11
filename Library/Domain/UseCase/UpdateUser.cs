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
        IUpdateUserDataManager _updateUserDataManager;
        IPresenterUpdateUserCallback _presenterUpdateUserCallback;
        UpdateUserRequest _updateUserRequest;
        public UpdateUser(UpdateUserRequest request, IPresenterUpdateUserCallback responseCallback)
        {
            _updateUserDataManager = ServiceProvider.GetInstance().Services.GetService<IUpdateUserDataManager>();
            _presenterUpdateUserCallback = responseCallback;
            _updateUserRequest = request;
        }
        public override void Action()
        {
            this._updateUserDataManager.UpdateUser(_updateUserRequest, new UpdateUserCallback(this));
        }

        public class UpdateUserCallback : IUsecaseCallbackBaseCase<User>
        {
            UpdateUser _updateUser;
            public UpdateUserCallback(UpdateUser updateUser)
            {
                _updateUser = updateUser;
            }
            public void OnResponseError(BException response)
            {
                _updateUser._presenterUpdateUserCallback?.OnError(response);
            }

            public void OnResponseFailure(ZResponse<User> response)
            {
                _updateUser._presenterUpdateUserCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<User> response)
            {
                _updateUser._presenterUpdateUserCallback?.OnSuccessAsync(response);
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
