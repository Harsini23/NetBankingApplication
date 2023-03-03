using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
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

        public void Subscribe(INotificationServicePayee subscriber)
        {
            _eventHandler += subscriber.OnMyEvent;
        }

        public void Unsubscribe(INotificationServicePayee subscriber)
        {
            _eventHandler -= subscriber.OnMyEvent;
        }

        public void RaiseEvent(string eventData)
        {
            _eventHandler?.Invoke(eventData);
        }

    }

    public interface INotificationServicePayee
    {
        void OnMyEvent(string eventData);
    }


    public class PayeeUpdate : INotificationServicePayee
    {
        public void OnMyEvent(string eventData)
        {
            var GetAllPayeeViewModel = PresenterService.GetInstance().Services.GetService<GetAllPayeeBaseViewModel>();
            // GetAllPayeeViewModel GetAllPayeeVm = new GetAllPayeeViewModel();
            GetAllPayeeViewModel.GetAllPayee(eventData);
        }
    }

   
}
