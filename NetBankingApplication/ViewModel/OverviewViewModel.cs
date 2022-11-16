using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{


    public class OverviewViewModel : OverviewBaseViewModel
    {
        public override void setUser(string userId)
        {
            UserId=userId;
        }

        public class PresenterOverViewCallback
        {

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

        public abstract void setUser(string userId);

    }
}
