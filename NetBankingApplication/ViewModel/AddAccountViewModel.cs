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
            
        }
    }
    public abstract class AddAccountBaseViewModel : NotifyPropertyBase
    {
        public abstract void AddAccount(AccountBObj account);
    }
}
