using NetBankingApplication.View.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class DashboadNavigationViewModel: DashboardNavigationBaseViewModel
    {
      
    }

    public class DashboardNavigationBaseViewModel : NotifyPropertyBase
    {

        public object _selectedComponent;
        public object SelectedComponent
        {
            get { return this._selectedComponent; }
            set
            {
                _selectedComponent = value;
                OnPropertyChangedAsync(nameof(SelectedComponent));
                //SetProperty(ref _response, value);
            }
        }
        public void SetOverview()
        {
            SelectedComponent = new Overview();
           
        }
        public void SetBankAccount()
        {
         
            SelectedComponent = new BankAccount();
        }

    }
}
