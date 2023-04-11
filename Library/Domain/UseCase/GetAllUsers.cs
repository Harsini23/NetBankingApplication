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
        private IGetAllUsersDataManager _getAllUsersDataManager;
        private GetAllUserRequest _getAllUserRequest;
        IPresenterGetAllUsersCallback _getAllUsersResponseCallback;
        public GetAllUsers(GetAllUserRequest request, IPresenterGetAllUsersCallback responseCallback)
        {
            _getAllUsersDataManager = ServiceProvider.GetInstance().Services.GetService<IGetAllUsersDataManager>();
            _getAllUserRequest = request;
            _getAllUsersResponseCallback = responseCallback;
        }
        public override void Action()
        {
            this._getAllUsersDataManager.GetAllUsers(_getAllUserRequest, new GetAllUsersCallback(this));
        }
        public class GetAllUsersCallback : IUsecaseCallbackBaseCase<GetAllUsersResponse>
        {
            private GetAllUsers _getAllUsers;
            public GetAllUsersCallback(GetAllUsers getAllUsers)
            {
                this._getAllUsers = getAllUsers;
            }
            public string Response { get; set; }

            public void OnResponseError(BException response)
            {
                _getAllUsers._getAllUsersResponseCallback?.OnError(response);

            }

            public void OnResponseFailure(ZResponse<GetAllUsersResponse> response)
            {
                _getAllUsers._getAllUsersResponseCallback?.OnFailure(response);
            }

            public void OnResponseSuccess(ZResponse<GetAllUsersResponse> response)
            {
                _getAllUsers._getAllUsersResponseCallback?.OnSuccessAsync(response);
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
