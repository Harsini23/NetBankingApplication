using Library;
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
using System.Threading;
using System.Threading.Tasks;
using static Library.Domain.UseCase.TransferAmountUseCase;

namespace NetBankingApplication.ViewModel
{
    public class TransferAmountViewModel : TransferAmountBaseViewModel
    {
        public delegate void ValueChangedEventHandler(string value, string user);
        public String UserID;
       private TransferAmountUseCase _transfer;
        public override void SendTransaction(AmountTransfer transaction, string userId)
        {
            UserID = userId;
            _transfer = new TransferAmountUseCase(new TransferAmountRequest(transaction, userId, new CancellationTokenSource()), new PresenterTransferAmountCallback(this));
            _transfer.Execute();
        }
    }

    public class PresenterTransferAmountCallback : IPresenterTransferAmountCallback
    {
        private TransferAmountViewModel _transferAmountViewModel;
        public static event TransferAmountViewModel.ValueChangedEventHandler ValueChanged;

        public PresenterTransferAmountCallback()
        {

        }
        public PresenterTransferAmountCallback(TransferAmountViewModel TransferAmountViewModel)
        {
            this._transferAmountViewModel = TransferAmountViewModel;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<TransferAmountResponse> response)
        {
        }

        public async void OnSuccessAsync(ZResponse<TransferAmountResponse> response)
        {

            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                var currentTransaction = response.Data.transaction;
                _transferAmountViewModel.ResultStatus = response.Response;
                _transferAmountViewModel.AmountTransfered = currentTransaction.Amount;
                _transferAmountViewModel.TransactionIdValue = currentTransaction.TransactionId;
                _transferAmountViewModel.DateTime = currentTransaction.Date;
                _transferAmountViewModel.FromAccountNumber = currentTransaction.FromAccount;
                _transferAmountViewModel.ToAccountNumber = currentTransaction.ToAccount;
                _transferAmountViewModel.ToName = currentTransaction.Name;
                _transferAmountViewModel.Remark = currentTransaction.Remark;
                if (currentTransaction.Status)
                {
                    _transferAmountViewModel.Status = "Success";
                    ValueChanged?.Invoke(currentTransaction.FromAccount, _transferAmountViewModel.UserID);
                    if (_transferAmountViewModel.NewPayee)
                    {
                        //call to display usercontrol to suggest adding the payee
                        _transferAmountViewModel.suggestionPopUp?.addPayeeView();
                    }
                }
                else
                {
                    _transferAmountViewModel.Status = "Failed";
                }
            });
                //TransferAmountViewModel.ResultText = response.Response.ToString();
             
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
                OnPropertyChanged(nameof(ResultText));
            }
        }

        private string _transactionIdValue = String.Empty;
        public string TransactionIdValue
        {
            get { return this._transactionIdValue; }
            set
            {
                _transactionIdValue = value;
                OnPropertyChanged(nameof(TransactionIdValue));
            }
        }

        private string _datetime = String.Empty;
        public string DateTime
        {
            get { return this._datetime; }
            set
            {
                _datetime = value;
                OnPropertyChanged(nameof(DateTime));
            }
        }

        private string _fromAccount = String.Empty;
        public string FromAccountNumber
        {
            get { return this._fromAccount; }
            set
            {
                _fromAccount = value;
                OnPropertyChanged(nameof(FromAccountNumber));
            }
        }
        private string _toAccountNumber = String.Empty;
        public string ToAccountNumber
        {
            get { return this._toAccountNumber; }
            set
            {
                _toAccountNumber = value;
                OnPropertyChanged(nameof(ToAccountNumber));
            }
        }
        private string _toName = String.Empty;
        public string ToName
        {
            get { return this._toName; }
            set
            {
                _toName = value;
                OnPropertyChanged(nameof(ToName));
            }
        }

        private double _amountTransfered = 0.0;
        public double AmountTransfered
        {
            get { return this._amountTransfered; }
            set
            {
                _amountTransfered = value;
                OnPropertyChanged(nameof(AmountTransfered));
            }
        }

        private string _remark = String.Empty;
        public string Remark
        {
            get { return this._remark; }
            set
            {
                _remark = value;
                OnPropertyChanged(nameof(Remark));
            }
        }
        private string _status = String.Empty;
        public string Status
        {
            get { return this._status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private string _resultStatus = String.Empty;
        public string ResultStatus
        {
            get { return this._resultStatus; }
            set
            {
                _resultStatus = value;
                OnPropertyChanged(nameof(ResultStatus));
            }
        }
        private bool _newPayee = false;

        public bool NewPayee
        {
            get { return this._newPayee; }
            set
            {
                _newPayee = value;
                OnPropertyChanged(nameof(NewPayee));
            }
        }
        public ISuggestAndAddPayeeView suggestionPopUp { get; set; }
        public abstract void SendTransaction(AmountTransfer transaction, string userId);

    }

    public interface ISuggestAndAddPayeeView
    {
        void addPayeeView();
    }
}
