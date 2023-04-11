using Library;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class EditPayeeViewModel : EditPayeeBaseViewModel
    {
       private EditPayee _editPayee;
       private Payee _currentpayee;
        public override void EditPayee(Payee payee)
        {
            _currentpayee = payee;
            _editPayee = new EditPayee(new EditPayeeRequest(payee.UserID,payee, new CancellationTokenSource()), new PresenterEditPayeeCallback(this));
            _editPayee.Execute();
        }
    }

    public class PresenterEditPayeeCallback : IPresenterEditPayeeCallback
    {
        private EditPayeeViewModel _editPayeeViewModel;
      //  NotificationService eventProvider = new NotificationService();
        public PresenterEditPayeeCallback()
        {

        }
        public PresenterEditPayeeCallback(EditPayeeViewModel EditPayeeViewModel)
        {
            this._editPayeeViewModel = EditPayeeViewModel;
        }

        public void OnError(BException response)
        {
        }
        public void OnFailure(ZResponse<string> response)
        {

        }
        public async void OnSuccessAsync(ZResponse<string> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _editPayeeViewModel.ResponseValue = response.Response;
                _editPayeeViewModel.AddEditPayeeView?.CallEditNotificationNotification();
                //refresh list after updation!
                //eventProvider.Subscribe(new PayeeUpdate());
                //eventProvider.RaiseEvent(EditPayeeViewModel.Currentpayee.UserID);
            });
        }
    }

        public abstract class EditPayeeBaseViewModel : NotifyPropertyBase
        {
        public IEditNotificationAlert AddEditPayeeView;

        public abstract void EditPayee(Payee payee);
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

    public interface IEditNotificationAlert
    {
        void CallEditNotificationNotification();
    }



}
