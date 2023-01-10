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
        void AddNewUser(AddUserRequest request, AddUserCallback response);
    }

    public class AddUserRequest
    {
        public UserAccountDetails newUser { get; set; }
        public AddUserRequest(UserAccountDetails addNewUser, string userId)
        {
            newUser = addNewUser;
        }
    }
    public class AddUser :UseCaseBase
    {
        private IAddUserDataManager AddUserDataManager;
        private AddUserRequest addUserRequest;
        IPresenterAddUserCallback presenterAddUserCallback;
        public AddUser(AddUserRequest request, IPresenterAddUserCallback responseCallback)
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            AddUserDataManager = serviceProviderInstance.Services.GetService<IAddUserDataManager>();
            addUserRequest = request;
            presenterAddUserCallback = responseCallback;
        }

        public override void Action()
        {
            this.AddUserDataManager.AddNewUser(addUserRequest, new AddUserCallback(this));
        }

        public class AddUserCallback:ZResponse<AddUserResponse>
        {
            public AddUserCallback(AddUser addUser)
            {

            }
        }
    }

    public interface IPresenterAddUserCallback
    {
        void OnSuccess(ZResponse<AddUserResponse> response);
        void OnError(ZResponse<AddUserResponse> response);
        void OnFailure(ZResponse<AddUserResponse> response);
    }
}
