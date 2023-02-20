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

            public async void OnSuccessAsync(ZResponse<OverviewResponse> response)
            {
                await SwitchToMainUIThread.SwitchToMainThread(() =>
                {
                    OverviewViewModel.TotalBalance = response.Data.balance;

                });

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
                OnPropertyChanged(nameof(UserId));
            }
        }


        private string _totalBalance = String.Empty;
        public string TotalBalance
        {
            get { return _totalBalance; }
            set
            {
                _totalBalance = value;
                OnPropertyChanged(nameof(TotalBalance));
            }
        }

        public abstract void getData(string userId);
        public abstract void setUser(string userId);

    }


}
