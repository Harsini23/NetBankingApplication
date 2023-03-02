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
        EditPayee editPayee;
        public override void EditPayee(Payee payee)
        {
            editPayee = new EditPayee(new EditPayeeRequest(payee.UserID,payee, new CancellationTokenSource()), new PresenterEditPayeeCallback(this));
            editPayee.Execute();
        }
    }

    public class PresenterEditPayeeCallback : IPresenterEditPayeeCallback
    {
        private EditPayeeViewModel EditPayeeViewModel;
        public PresenterEditPayeeCallback()
        {

        }
        public PresenterEditPayeeCallback(EditPayeeViewModel EditPayeeViewModel)
        {
            this.EditPayeeViewModel = EditPayeeViewModel;
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
                //refresh list after updation!

                var a = response;
            });
        }
    }

        public abstract class EditPayeeBaseViewModel : NotifyPropertyBase
        {

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
}
