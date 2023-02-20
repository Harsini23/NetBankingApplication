using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace NetBankingApplication.ViewModel
{
    public static class SwitchToMainUIThread
    {
        public static async Task SwitchToMainThread( Action action)
        {
            //await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
            Windows.UI.Core.CoreDispatcherPriority.Normal, () => action());
        }
    }
}
