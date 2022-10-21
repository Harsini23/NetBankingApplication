using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public abstract class NotifyPropertyBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        protected void OnPropertyChanged(string propertyName)
        {
             PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            //if (PropertyChanged != null)
            //{
            //    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            //}

            //var handler = this.PropertyChanged;
            //if (handler != null)
            //    handler(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
