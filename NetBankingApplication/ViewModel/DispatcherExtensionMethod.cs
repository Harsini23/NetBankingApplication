using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace NetBankingApplication.ViewModel
{
    public static class DispatcherExtensionMethod
    {
        public static async Task RunAsync(this CoreDispatcher dispatcher, Action action)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
        }
    }
}
