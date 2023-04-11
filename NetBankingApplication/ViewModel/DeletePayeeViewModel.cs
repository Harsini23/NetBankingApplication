using Library;
using Library.BankingNotification;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using NetBankingApplication.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class DeletePayeeViewModel : DeletePayeeBaseViewModel
    {
        private DeletePayee _deleteRecipient;
        public static String userId;
       // public delegate void ValueChangedEventHandler(string value);
        public override void DeletePayee(Payee payee)
        {
            userId = payee.UserID;
            _deleteRecipient = new DeletePayee(new DeletePayeeRequest(payee.UserID, payee, new CancellationTokenSource()), new PresenterDeletePayeeCallback(this));
            _deleteRecipient.Execute();
        }
    }

    public class PresenterDeletePayeeCallback : IPresenterDeletePayeeCallback
    {
        private DeletePayeeViewModel _deletePayeeViewModel;
       // public static event DeletePayeeViewModel.ValueChangedEventHandler ValueChanged;

     //   NotificationService eventProvider = new NotificationService();


        public PresenterDeletePayeeCallback()
        {

        }
        public PresenterDeletePayeeCallback(DeletePayeeViewModel deletePayeeViewModel)
        {
            this._deletePayeeViewModel = deletePayeeViewModel;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<String> response)
        {

        }

        async void IResponseCallbackBaseCase<string>.OnSuccessAsync(ZResponse<string> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _deletePayeeViewModel.ResponseValue = response.Data.ToString();
                _deletePayeeViewModel.AddEditPayeeView?.CallDeleteNotificationNotification();
                //ValueChanged?.Invoke(DeletePayeeViewModel.userId);

                //eventProvider.Subscribe(new PayeeUpdate());
                //eventProvider.RaiseEvent(DeletePayeeViewModel.userId);
               // BankingNotification.PayeeUpdated += BankingNotification_PayeeUpdated; 
            });
        }

        //private void BankingNotification_PayeeUpdated(Payee obj)
        //{
        //    //throw new NotImplementedException();
        //}
    }
    public abstract class DeletePayeeBaseViewModel : NotifyPropertyBase
    {

        public abstract void DeletePayee(Payee payee);
        public IDeleteNotificationAlert AddEditPayeeView;
        private string _response = String.Empty;
        public string ResponseValue
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(ResponseValue));
            }
        }

    }

    public interface IDeleteNotificationAlert
    {
       void CallDeleteNotificationNotification();
    }
}
