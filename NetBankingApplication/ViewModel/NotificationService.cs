using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    
    public class NotificationServiceUser
    {
        private Action<User> _eventHandler;

        public void Subscribe(INotificationServiceUser subscriber)
        {
            _eventHandler += subscriber.OnUserUpdate;
        }

        public void Unsubscribe(INotificationServiceUser subscriber)
        {
            _eventHandler -= subscriber.OnUserUpdate;
        }

        public void RaiseEvent(User eventData)
        {
            _eventHandler?.Invoke(eventData);
        }

    }

   

    public interface INotificationServiceUser
    {
        void OnUserUpdate(User user);
    }
    public interface INotificationServiceAccount
    {
        void OnAccountUpdate(String userId);
    }


    public class UserUpdate : INotificationServiceUser
    {
        public static LoginBaseViewModel LoginViewModelInstance { get; set; }
     
        public void OnUserUpdate(User user)
        {
            LoginViewModelInstance.CurrentUser = user;
        }
    }


}
