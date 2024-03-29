﻿using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System.Collections.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class GetAllUsersDataManager : BankingDataManager,IGetAllUsersDataManager
    {
        public GetAllUsersDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
        }

        public void GetAllUsers(GetAllUserRequest request, IUsecaseCallbackBaseCase<GetAllUsersResponse> callback)
        {
            ZResponse<GetAllUsersResponse> response = new ZResponse<GetAllUsersResponse>();
            GetAllUsersResponse getAllUsersResponse = new GetAllUsersResponse();
            ObservableCollection<User> collectionOfUsers = new ObservableCollection<User>(DbHandler.GetAllUsers());
            getAllUsersResponse.Data = collectionOfUsers;
            response.Data = getAllUsersResponse;
            var responseStatus = "Successfull got all Users";
            response.Response = responseStatus;

            callback.OnResponseSuccess(response);
        }
    }
}
