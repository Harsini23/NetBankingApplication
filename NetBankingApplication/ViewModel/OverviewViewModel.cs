using Library;
using Library.Data.DataManager;
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


    public class OverviewViewModel : OverviewBaseViewModel
    {
        static int Flag = 0;
        public Overview overview;
        public override void getData(string userId)
        {
            overview = new Overview(new OverviewRequest(userId,new CancellationTokenSource()),new PresenterOverViewCallback(this));
            overview.Execute();
        }


        public override void setUser(string userId)
        {
            UserId=userId;
        }

        public class PresenterOverViewCallback : IPresenterOverviewCallback
        {
            OverviewViewModel OverviewViewModel;
            public PresenterOverViewCallback(OverviewViewModel overviewViewModel)
            {
                OverviewViewModel = overviewViewModel;
            }
            public void OnError(BException errorMessage)
            {
                throw new NotImplementedException();
            }

            public void OnFailure(ZResponse<OverviewResponse> response)
            {
                throw new NotImplementedException();
            }

            public void OnSuccess(ZResponse<OverviewResponse> response)
            {
                throw new NotImplementedException();
            }
        }
    }
        
    public abstract class OverviewBaseViewModel : NotifyPropertyBase
    {
        private string _userId= String.Empty;
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChangedAsync(nameof(UserId));
            }
        }
     
        public abstract void getData(string userId);
        public abstract void setUser(string userId);

    }
}
