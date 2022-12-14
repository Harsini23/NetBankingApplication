using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class DeletePayeeViewModel : DeletePayeeBaseViewModel
    {
        DeletePayee deleteRecipient;
        public override void DeletePayee(Payee payee)
        {
            deleteRecipient = new DeletePayee(new DeletePayeeRequest(payee.UserID, payee), new PresenterDeletePayeeCallback(this));
            deleteRecipient.Execute();
        }
    }

    public class PresenterDeletePayeeCallback : IPresenterDeletePayeeCallback
    {
        private DeletePayeeViewModel deletePayeeViewModel;
        public PresenterDeletePayeeCallback()
        {

        }
        public PresenterDeletePayeeCallback(DeletePayeeViewModel deletePayeeViewModel)
        {
            this.deletePayeeViewModel = deletePayeeViewModel;
        }

        public void OnError(ZResponse<String> response)
        {
        }

        public void OnFailure(ZResponse<String> response)
        {

        }

        public void OnSuccess(ZResponse<String> response)
        {
            deletePayeeViewModel.ResponseValue = response.Data.ToString();
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
                OnPropertyChangedAsync(nameof(ResponseValue));
                //SetProperty(ref _response, value);
            }
        }

    }
}
