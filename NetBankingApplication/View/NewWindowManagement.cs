using NetBankingApplication.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WindowManagement;

namespace NetBankingApplication.View
{
    public class NewWindowManagement: NewWindowManagementBase
    {
    }
    public abstract class NewWindowManagementBase : NotifyPropertyBase
    {
        public Dictionary<int, AppWindow> _appWindows = new Dictionary<int, AppWindow>();

    }
}
