using Library.Data.DataManager;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Library.Domain.UseCase.AddUser;

namespace Library.Domain.UseCase
{
    public interface IAddUserDataManager
    {
        void AddNewUser(AddUserRequest request, IUsecaseCallbackBaseCase<AddUserResponse> response);
    }

    public class AddUserRequest
    {
        public UserAccountDetails NewUser { get; set; }
        public AddUserRequest(UserAccountDetails addNewUser, string userId)
        {
            NewUser = addNewUser;
        }
    }
    public class AddUser :UseCaseBase<AddUserResponse>
    {
        private IAddUserDataManager _addUserDataManager;
        private AddUserRequest _addUserRequest;
        IPresenterAddUserCallback _presenterAddUserCallback;
        public AddUser(AddUserRequest request, IPresenterAddUserCallback responseCallback)
        {
            _addUserDataManager = ServiceProvider.GetInstance().Services.GetService<IAddUserDataManager>();
            _addUserRequest = request;
            _presenterAddUserCallback = responseCallback;
        }

        public override void Action()
        {
            this._addUserDataManager.AddNewUser(_addUserRequest, new AddUserCallback(this));
        }

        public class AddUserCallback: IUsecaseCallbackBaseCase<AddUserResponse>
        {
            AddUser addUser;
            public AddUserCallback(AddUser addUser)
            {
                this.addUser = addUser;
            }


            public void OnResponseError(BException response)
            {
                addUser._presenterAddUserCallback?.OnError(response);
            }
            public void OnResponseFailure(ZResponse<AddUserResponse> response)
            {
                addUser._presenterAddUserCallback?.OnFailure(response);
            }
            public void OnResponseSuccess(ZResponse<AddUserResponse> response)
            {
                addUser._presenterAddUserCallback?.OnSuccessAsync(response);

            }

        }
    }

    public interface IPresenterAddUserCallback: IResponseCallbackBaseCase<AddUserResponse>
    {
    }


    public class AddUserResponse : ZResponse<User>
    {
        public User User;
        public Credentials Credentials;
        public Account Account;
        public bool UserExists;
    }
}
