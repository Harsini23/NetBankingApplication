using Library;
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
        AddPayee addPayee;
        public override void AddPayee(Payee newRecipent)
        {
            //var UserId = "Harsh";
            //Payee recipent = new Payee{ AccountHolderName = "Chandler", AccountNumber = "567893471625", BankName = "ICICI", PayeeName = "Chan", IfscCode = "ICICI006", UserID = "Harsh" };
            AddPayeeRequest newReceiver = new AddPayeeRequest();
            newReceiver.UserId = newRecipent.UserID;
            newReceiver.NewPayee= newRecipent;
            addPayee = new AddPayee(newReceiver, new PresenterAddPayeeCallback(this));
            addPayee.Execute();
        }
    }

    public class PresenterAddPayeeCallback : IPresenterAddPayeeCallback
    {
        private AddPayeeViewModel addPayeeViewModel;
        public PresenterAddPayeeCallback()
        {

        }
        public PresenterAddPayeeCallback(AddPayeeViewModel addPayeeViewModel)
        {
            this.addPayeeViewModel = addPayeeViewModel;
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
                addPayeeViewModel.AddPayeeResponseValue = response.Response;
            });
          
        }
    }


    public abstract class AddPayeeBaseViewModel : NotifyPropertyBase
    {
        public abstract void AddPayee(Payee newRecipent);
     

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
}
