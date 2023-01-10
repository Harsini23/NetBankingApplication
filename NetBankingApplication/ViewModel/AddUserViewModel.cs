using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Library.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class AddUserViewModel : AddUserBaseViewModel
    {
        AddUser addNewUser;
        public override void AddUser(UserAccountDetails userDetails)
        {
            AddUserRequest request = new AddUserRequest(userDetails, "userid");
            addNewUser = new AddUser(request, new PresenterAddUserCallback(this) );
            addNewUser.Execute();
        }
    }

    public class PresenterAddUserCallback : IPresenterAddUserCallback
    {
        private AddUserViewModel addUserViewModel;
        public PresenterAddUserCallback(AddUserViewModel addUserViewModel)
        {   
            this.addUserViewModel = addUserViewModel;
        }
        public void OnError(ZResponse<AddUserResponse> response)
        {
            
        }

        public void OnFailure(ZResponse<AddUserResponse> response)
        {
          
        }

        public void OnSuccess(ZResponse<AddUserResponse> response)
        {
          
        }
    }

    public abstract class AddUserBaseViewModel : NotifyPropertyBase
    {
        public abstract void AddUser(UserAccountDetails userDetails);
    }
}
