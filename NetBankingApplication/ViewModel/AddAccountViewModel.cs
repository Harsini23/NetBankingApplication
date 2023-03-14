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
        AddAccount addAccount;
        public override void AddAccount(AccountBObj account)
        {
            addAccount = new AddAccount(new AddAccountRequest(account, account.UserId), new PresenterAddAccountCallback(this));
            addAccount.Execute();
        }
    }


    public class PresenterAddAccountCallback : IPresenterAddAccountCallback
    {
        private AddAccountViewModel addAccountViewModel;

        public PresenterAddAccountCallback(AddAccountViewModel addAccountViewModel)
        {
            this.addAccountViewModel = addAccountViewModel;
        }

        public void OnError(BException errorMessage)
        {
        }

        public void OnFailure(ZResponse<bool> response)
        {
        }

        public void OnSuccessAsync(ZResponse<bool> response)
        {
        }
    }
    public abstract class AddAccountBaseViewModel : NotifyPropertyBase
    {
        public abstract void AddAccount(AccountBObj account);
    }
}
