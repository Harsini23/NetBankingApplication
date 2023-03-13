using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Domain.UseCase
{
    public interface IGetAllUsersDataManager
    {
        void GetAllUsers(GetAllUserRequest request, IUsecaseCallbackBaseCase<GetAllUsersResponse> response);//call back
    }

    public class GetAllUsers:UseCaseBase<GetAllUsersResponse>
    {
        private IGetAllUsersDataManager GetAllUsersDataManager;
        private GetAllUserRequest GetAllUserRequest;
        IPresenterGetAllUsersCallback GetAllUsersResponseCallback;
        public GetAllUsers(GetAllUserRequest request, IPresenterGetAllUsersCallback responseCallback)
        {
            GetAllUsersDataManager = ServiceProvider.GetInstance().Services.GetService<IGetAllUsersDataManager>();
            GetAllUserRequest = request;
            GetAllUsersResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this.GetAllUsersDataManager.GetAllUsers(GetAllUserRequest, new GetAllUsersCallback(this));
        }
        public class GetAllUsersCallback : IUsecaseCallbackBaseCase<GetAllUsersResponse>
        {
            private GetAllUsers getAllUsers;
            public GetAllUsersCallback(GetAllUsers getAllUsers)
            {
                this.getAllUsers = getAllUsers;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                getAllUsers.GetAllUsersResponseCallback?.OnError(response);

            }

            public void OnResponseFailure(ZResponse<GetAllUsersResponse> response)
            {
                getAllUsers.GetAllUsersResponseCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAllUsersResponse> response)
            {
                getAllUsers.GetAllUsersResponseCallback?.OnSuccessAsync(response);
            }
        }
    }

    public class GetAllUserRequest : IRequest
    {
        public string UserId { get; set; }
        public CancellationTokenSource CtsSource { get; set; }

        public GetAllUserRequest(string userId, CancellationTokenSource cts)
        {
            UserId = userId;
            CtsSource = cts;
        }
    }

    public interface IPresenterGetAllUsersCallback : IResponseCallbackBaseCase<GetAllUsersResponse>
    {
    }

    public class GetAllUsersResponse : ZResponse<ObservableCollection<User>>
    {
     
    }

   
}
