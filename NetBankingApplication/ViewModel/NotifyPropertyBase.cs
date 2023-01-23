using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;

namespace NetBankingApplication.ViewModel
{
    public class NotifyPropertyBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        //{
        //    if (Equals(storage, value)) return false;
        //    storage = value;
        //    OnPropertyChangedAsync(propertyName);
        //    return true;
        //}
        protected async Task OnPropertyChangedAsync(string propertyName)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
                Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

                });
        }

        protected async Task OnViewPropertyChange(string propertyName, CoreDispatcher dispatcher)
        {
            CoreApplicationView currentView = CoreApplication.GetCurrentView();
            await dispatcher?.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
              });
        }
    }
}
       

 