﻿using Library.Model;
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

    public class NotificationServiceAccount
    {
        private Action<String> _eventHandler;

        public void Subscribe(INotificationServiceAccount subscriber)
        {
            _eventHandler += subscriber.OnAccountUpdate;
        }

        public void Unsubscribe(INotificationServiceAccount subscriber)
        {
            _eventHandler -= subscriber.OnAccountUpdate;
        }

        public void RaiseEvent(String userID)
        {
            _eventHandler?.Invoke(userID);
        }

    }

    public interface INotificationServicePayee
    {
        void OnMyEvent(string eventData);
    }

    public interface INotificationServiceUser
    {
        void OnUserUpdate(User user);
    }
    public interface INotificationServiceAccount
    {
        void OnAccountUpdate(String userId);
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

    public  class UserUpdate : INotificationServiceUser
    {
       public static LoginBaseViewModel LoginViewModelInstance { get; set; }
        //public UserUpdate(OverviewBaseViewModel overViewViewModel)
        //{
        //    OverViewViewModelInstance = overViewViewModel;
        //}
        public void OnUserUpdate(User user)
        {
            LoginViewModelInstance.CurrentUser=user;
        }
    }

    public class AccountUpdate : INotificationServiceAccount
    {
        public void OnAccountUpdate(string userId)
        {
            var getAllAccountsBaseViewModel = PresenterService.GetInstance().Services.GetService<GetAllAccountsBaseViewModel>();
            getAllAccountsBaseViewModel.GetAllAccounts(userId);
        }
    }


}
