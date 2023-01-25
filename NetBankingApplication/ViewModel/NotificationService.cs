using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class NotificationService
    {
        public NotificationService()
        {
                
        }
        private void TransferAmountViewModel_ValueChanged(string value, string user)
        {
            //GetAllTransactions(value, user);
        }
    }

    public interface INotificationService
    {
        void HandleTransfer();
    }
}
