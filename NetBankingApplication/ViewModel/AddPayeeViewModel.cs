﻿using Library;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class AddPayeeViewModel : AddPayeeBaseViewModel
    {
        private AddPayee _addPayee;
        public override void AddPayee(Payee newRecipent)
        {
            AddPayeeRequest newReceiver = new AddPayeeRequest();
            newReceiver.UserId = newRecipent.UserID;
            newReceiver.NewPayee= newRecipent;
            _addPayee = new AddPayee(newReceiver, new PresenterAddPayeeCallback(this));
            _addPayee.Execute();
        }
    }

    public class PresenterAddPayeeCallback : IPresenterAddPayeeCallback
    {
        private AddPayeeViewModel _addPayeeViewModel;
        public PresenterAddPayeeCallback()
        {

        }
        public PresenterAddPayeeCallback(AddPayeeViewModel addPayeeViewModel)
        {
            this._addPayeeViewModel = addPayeeViewModel;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<String> response)
        {

        }

    
        public async void OnSuccessAsync(ZResponse<string> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _addPayeeViewModel.AddPayeeResponseValue = response.Response;
                _addPayeeViewModel.AddPayeeView?.CallNotification();
            });
          
        }
    }


    public abstract class AddPayeeBaseViewModel : NotifyPropertyBase
    {
        public abstract void AddPayee(Payee newRecipent);
        public INotificationAlert AddPayeeView { get; set; }

        private string _response = String.Empty;
        public string AddPayeeResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(AddPayeeResponseValue));
                //SetProperty(ref _response, value);
            }
        }

    }

    public interface INotificationAlert
    {
        void CallNotification();
    }
}
