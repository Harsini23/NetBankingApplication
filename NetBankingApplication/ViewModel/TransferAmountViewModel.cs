using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using NetBankingApplication.View.UserControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class TransferAmountViewModel : TransferAmountBaseViewModel
    {

        TransferAmountUseCase transfer;
        public override void SendTransaction(AmountTransfer transaction, string userId)
        {
            transfer = new TransferAmountUseCase(new TransferAmountRequest(transaction, userId), new PresenterTransferAmountCallback(this));
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
            TransferAmountViewModel.ResultText = response.Response.ToString();
            var currentTransaction = response.Data.transaction;
            TransferAmountViewModel.AmountTransfered = currentTransaction.Amount;
            TransferAmountViewModel.TransactionIdValue = currentTransaction.TransactionId;
            TransferAmountViewModel.DateTime = currentTransaction.Date;
            TransferAmountViewModel.FromAccountNumber = currentTransaction.FromAccount;
            TransferAmountViewModel.ToAccountNumber = currentTransaction.ToAccount;
            TransferAmountViewModel.ToName = currentTransaction.Name;
            TransferAmountViewModel.Remark = currentTransaction.Remark;
            if (currentTransaction.Status)
            {
                TransferAmountViewModel.Status = "Success";
            }
            else
            {
                TransferAmountViewModel.Status = "Failed";
            }
            //Debug.WriteLine(response.Data.transaction.Name, response.Data.transaction.ToAccount);
        }



    }

    public abstract class TransferAmountBaseViewModel : NotifyPropertyBase
    {
        private string _response = String.Empty;
        public string ResultText
        {
            get { return this._response; }
            set
            {
                _response = value;
                OnPropertyChangedAsync(nameof(ResultText));
                //SetProperty(ref _response, value);
            }
        }

        private string _transactionIdValue = String.Empty;
        public string TransactionIdValue
        {
            get { return this._transactionIdValue; }
            set
            {
                _transactionIdValue = value;
                OnPropertyChangedAsync(nameof(TransactionIdValue));
                //SetProperty(ref _response, value);
            }
        }

        private string _datetime = String.Empty;
        public string DateTime
        {
            get { return this._datetime; }
            set
            {
                _datetime = value;
                OnPropertyChangedAsync(nameof(DateTime));
                //SetProperty(ref _response, value);
            }
        }

        private string _fromAccount = String.Empty;
        public string FromAccountNumber
        {
            get { return this._fromAccount; }
            set
            {
                _fromAccount = value;
                OnPropertyChangedAsync(nameof(FromAccountNumber));
                //SetProperty(ref _response, value);
            }
        }
        private string _toAccountNumber = String.Empty;
        public string ToAccountNumber
        {
            get { return this._toAccountNumber; }
            set
            {
                _toAccountNumber = value;
                OnPropertyChangedAsync(nameof(ToAccountNumber));
                //SetProperty(ref _response, value);
            }
        }
        private string _toName = String.Empty;
        public string ToName
        {
            get { return this._toName; }
            set
            {
                _toName = value;
                OnPropertyChangedAsync(nameof(ToName));
                //SetProperty(ref _response, value);
            }
        }

        private string _amountTransfered = String.Empty;
        public string AmountTransfered
        {
            get { return this._amountTransfered; }
            set
            {
                _amountTransfered = value;
                OnPropertyChangedAsync(nameof(AmountTransfered));
                //SetProperty(ref _response, value);
            }
        }

        private string _remark = String.Empty;
        public string Remark
        {
            get { return this._remark; }
            set
            {
                _remark = value;
                OnPropertyChangedAsync(nameof(Remark));
                //SetProperty(ref _response, value);
            }
        }
        private string _status = String.Empty;
        public string Status
        {
            get { return this._status; }
            set
            {
                _status = value;
                OnPropertyChangedAsync(nameof(Status));
                //SetProperty(ref _response, value);
            }
        }




        public abstract void SendTransaction(AmountTransfer transaction, string userId);

    }
}
