using Library;
using Library.BankingNotification;
using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using static Library.Domain.UseCase.GetAllPayee;

namespace NetBankingApplication.ViewModel
{
    public class GetAllPayeeViewModel : GetAllPayeeBaseViewModel
    {
       private GetAllPayee _getAllPayee;
        public override void GetAllPayee(string userId)
        {
            _getAllPayee = new GetAllPayee(new GetAllPayeeRequest(userId, new CancellationTokenSource()), new PresenterGetAllPayeeCallback(this));
            _getAllPayee.Execute();
        }
    }



    public class PresenterGetAllPayeeCallback : IPresenterGetAllPayeeCallback
    {
        private GetAllPayeeViewModel _getAllPayeeViewModel;
        public PresenterGetAllPayeeCallback()
        {

        }
        public PresenterGetAllPayeeCallback(GetAllPayeeViewModel getAllPayeeViewModel)
        {
            this._getAllPayeeViewModel = getAllPayeeViewModel;
        }

        public void OnError(BException response)
        {
        }

        public void OnFailure(ZResponse<GetAllPayeeResponse> response)
        {

        }

        public async void OnSuccessAsync(ZResponse<GetAllPayeeResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                var allPayee = response.Data.AllRecipients;
                var SortedPayee = allPayee.OrderBy(i => i.PayeeName);
                BankingNotification.PayeeUpdated += BankingNotification_PayeeUpdated ;
                BankingNotification.PayeeDeleted += BankingNotification_PayeeDeleted;
                populateData(SortedPayee);
            });
               
        }

        private async void BankingNotification_PayeeDeleted(Payee payee)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                _getAllPayeeViewModel.AllPayeeCollection.Remove(payee);
                _getAllPayeeViewModel.PayeeNames.Remove(payee.PayeeName);
                _getAllPayeeViewModel.AllPayee.Remove(payee);
            });
        }

        private async void BankingNotification_PayeeUpdated(Payee payee)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                Payee payeeToUpdate = _getAllPayeeViewModel.AllPayeeCollection.FirstOrDefault(p => p.AccountNumber == payee.AccountNumber);
                if (payeeToUpdate != null)
                {
                    int index= _getAllPayeeViewModel.AllPayeeCollection.IndexOf(payeeToUpdate);
                    payeeToUpdate.AccountHolderName = payee.AccountHolderName;
                    payeeToUpdate.BankName = payee.BankName;
                    payeeToUpdate.PayeeName = payee.PayeeName;
                    payeeToUpdate.IfscCode = payee.IfscCode;
                    _getAllPayeeViewModel.AllPayeeCollection[index] = payeeToUpdate;

                }
                var sortedPayee = new ObservableCollection<Payee>(_getAllPayeeViewModel.AllPayeeCollection.OrderBy(i => i.PayeeName).ToList());
                _getAllPayeeViewModel.AllPayeeCollection.Clear();
                foreach(var i in sortedPayee)
                {
                    _getAllPayeeViewModel.AllPayeeCollection.Add(i);
                }

            });
              
        }

        public async void populateData(IEnumerable<Payee> allPayee)
        {
            _getAllPayeeViewModel.AllPayeeCollection.Clear();
            _getAllPayeeViewModel.PayeeNames.Clear();
            _getAllPayeeViewModel.AllPayee.Clear();
                  foreach (var i in allPayee)
                  {
                _getAllPayeeViewModel.AllPayeeCollection.Add(i);
                _getAllPayeeViewModel.PayeeNames.Add(i.PayeeName);
                _getAllPayeeViewModel.AllPayee.Add(i);
                  }

            if(_getAllPayeeViewModel.AllPayeeCollection.Count <= 0)
            {
                _getAllPayeeViewModel.TextBoxVisibility = Visibility.Visible;

            }
            else
            {
                _getAllPayeeViewModel.TextBoxVisibility = Visibility.Collapsed;

            }
           // GetAllPayeeViewModel.ChangeVisibility?.ChangeVisibility(getAllPayeeViewModel.AllPayeeCollection.Count <= 0);
             
        }
    }



    public abstract class GetAllPayeeBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<Payee> AllPayeeCollection = new ObservableCollection<Payee>();
        public abstract void GetAllPayee(string userId);
        public ObservableCollection<Payee> AllPayee = new ObservableCollection<Payee>() { };
        public ObservableCollection<String> PayeeNames = new ObservableCollection<string>();
       // public static IViewAndEditPayeeVM ChangeVisibility;

        private Visibility _textBoxVisibility = Visibility.Collapsed;
        public Visibility TextBoxVisibility
        {
            get { return _textBoxVisibility; }
            set
            {
                _textBoxVisibility = value;
                OnPropertyChanged(nameof(TextBoxVisibility));

            }
        }

    }

    //public interface IViewAndEditPayeeVM
    //{
    //    void ChangeVisibility(bool visible);
    //}
}
