using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{


    public class OverviewViewModel : OverviewBaseViewModel
    {
        static int Flag = 0;
        // get data from db and set card component - cardcomponentItems only once
        public void setCardComponents()
        {
            Flag = 1;
            cardcomponentItems.Add(new CardComponent() { Heading = "User", Title = "UserID", Content = "harsh" });
            cardcomponentItems.Add(new CardComponent() { Heading = "Account", Title = "UserID", Content = "harsh" });
            cardcomponentItems.Add(new CardComponent() { Heading = "Card", Title = "UserID", Content = "harsh" });
            cardcomponentItems.Add(new CardComponent() { Heading = "Total Balance", Title = "UserID", Content = "harsh" });
            cardcomponentItems.Add(new CardComponent() { Heading = "Savings", Title = "UserID", Content = "harsh" });
        }

        public override void getCardComponents()
        {
            if(Flag==0)
            setCardComponents();
        }

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

        public ObservableCollection<CardComponent> cardcomponentItems = new ObservableCollection<CardComponent>();
        public string UserId
        {
            get { return _userId; }
            set
            {
                _userId = value;
                OnPropertyChangedAsync(nameof(UserId));
            }
        }
     
        public abstract void getCardComponents();
        public abstract void setUser(string userId);

    }
}
