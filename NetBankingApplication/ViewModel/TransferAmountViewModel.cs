using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using NetBankingApplication.View.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class TransferAmountViewModel : TransferAmountBaseViewModel
    {

        TransferAmountUseCase transfer;
        public override void SendTransaction(Transaction transaction)
        {
            transfer = new TransferAmountUseCase(new TransferAmountRequest(transaction), new PresenterTransferAmountCallback(this));
            transfer.Execute();
        }
    }

    public class PresenterTransferAmountCallback : IPresenterTransferAmountCallback
    {
        private TransferAmountViewModel TransferAmountViewModel;
        public PresenterTransferAmountCallback()
        {

        }
        public PresenterTransferAmountCallback(TransferAmountViewModel TransferAmountViewModel)
        {
            this.TransferAmountViewModel = TransferAmountViewModel;
        }

        public void OnError(ZResponse<TransferAmountResponse> response)
        {
        }

        public void OnFailure(ZResponse<TransferAmountResponse> response)
        {

        }

        public void OnSuccess(ZResponse<TransferAmountResponse> response)
        {
            
        }


      
    }

    public abstract class TransferAmountBaseViewModel : NotifyPropertyBase
    {
       
        public abstract void SendTransaction(Transaction transaction);
      
    }
}
