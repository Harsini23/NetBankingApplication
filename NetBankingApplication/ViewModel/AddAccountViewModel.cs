using Library;
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
    public class AddAccountViewModel : AddAccountBaseViewModel
    {
        private AddAccount _addAccount;
        private AccountBObj _accountBObj;
        public override void AddAccount(AccountBObj account)
        {
            _accountBObj = account;
            _addAccount = new AddAccount(new AddAccountRequest(account, account.UserId), new PresenterAddAccountCallback(this));
            _addAccount.Execute();
        }
    }
    public class PresenterAddAccountCallback : IPresenterAddAccountCallback
    {
        private AddAccountViewModel _addAccountViewModel;
        //NotificationServiceAccount accountEventProvider = new NotificationServiceAccount();
        public PresenterAddAccountCallback(AddAccountViewModel addAccountViewModel)
        {
            this._addAccountViewModel = addAccountViewModel;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<bool> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<bool> response)
        {
            //invoke notification to update the list preview
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _addAccountViewModel.Response = response.Response;
                _addAccountViewModel.addAccountView?.AccountNotification();
               // accountEventProvider.Subscribe(new AccountUpdate());
              //  accountEventProvider.RaiseEvent(addAccountViewModel.accountBObj.UserId);
            });
        }
    }
    public abstract class AddAccountBaseViewModel : NotifyPropertyBase
    {
        public abstract void AddAccount(AccountBObj account);
       public IAccountAddedNotification addAccountView { get; set; }
        private string _response = String.Empty;
        public string Response
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChanged(nameof(Response));
            }
        }
    }
    public interface IAccountAddedNotification
    {
        void AccountNotification();
    }
}
