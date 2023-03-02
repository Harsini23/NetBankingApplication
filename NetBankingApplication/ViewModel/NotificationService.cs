using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class NotificationService
    {
        private Action<string> _eventHandler;

        public void Subscribe(INotificationServiceDeletePayee subscriber)
        {
            _eventHandler += subscriber.OnMyEvent;
        }

        public void Unsubscribe(INotificationServiceDeletePayee subscriber)
        {
            _eventHandler -= subscriber.OnMyEvent;
        }

        public void RaiseEvent(string eventData)
        {
            _eventHandler?.Invoke(eventData);
        }

    }

    public interface INotificationServiceDeletePayee
    {
        void OnMyEvent(string eventData);
    }

    public class DeletionUpdate : INotificationServiceDeletePayee
    {
        public void OnMyEvent(string eventData)
        {
            GetAllPayeeViewModel GetAllPayeeVm = new GetAllPayeeViewModel();
               GetAllPayeeVm.GetAllPayee(eventData);         
        }
    }
}
