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
    public class DeletePayeeViewModel : DeletePayeeBaseViewModel
    {
        DeletePayee deleteRecipient;
        public static String userId;
        public delegate void ValueChangedEventHandler(string value);
        public override void DeletePayee(Payee payee)
        {
            userId = payee.UserID;
            deleteRecipient = new DeletePayee(new DeletePayeeRequest(payee.UserID, payee, new CancellationTokenSource()), new PresenterDeletePayeeCallback(this));
            deleteRecipient.Execute();
        }
    }

    public class PresenterDeletePayeeCallback : IPresenterDeletePayeeCallback
    {
        private DeletePayeeViewModel deletePayeeViewModel;
        public static event DeletePayeeViewModel.ValueChangedEventHandler ValueChanged;

        public PresenterDeletePayeeCallback()
        {

        }
        public PresenterDeletePayeeCallback(DeletePayeeViewModel deletePayeeViewModel)
        {
            this.deletePayeeViewModel = deletePayeeViewModel;
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
                deletePayeeViewModel.ResponseValue = response.Data.ToString();
                ValueChanged?.Invoke(DeletePayeeViewModel.userId);
            });
        }
    }
    public abstract class DeletePayeeBaseViewModel : NotifyPropertyBase
    {

        public abstract void DeletePayee(Payee payee);
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
