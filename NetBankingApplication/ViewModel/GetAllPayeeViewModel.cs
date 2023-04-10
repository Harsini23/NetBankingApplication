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
        GetAllPayee recipients;
      //  public IViewAndEditPayeeVM viewAndEditPayeeVMCallback;
        public GetAllPayeeViewModel()
        {
            //PresenterDeletePayeeCallback.ValueChanged += PresenterDeletePayeeCallback_ValueChanged;
        }
        //private void PresenterDeletePayeeCallback_ValueChanged(string id)
        //{
        //    GetAllPayee(id);
        //}

        public override void GetAllPayee(string userId)
        {
            //viewAndEditPayeeVMCallback =  ChangeVisibility;
            recipients = new GetAllPayee(new GetAllPayeeRequest(userId, new CancellationTokenSource()), new PresenterGetAllPayeeCallback(this));
            recipients.Execute();
        }
    }



    public class PresenterGetAllPayeeCallback : IPresenterGetAllPayeeCallback
    {
        private GetAllPayeeViewModel getAllPayeeViewModel;
        public PresenterGetAllPayeeCallback()
        {

        }
        public PresenterGetAllPayeeCallback(GetAllPayeeViewModel getAllPayeeViewModel)
        {
            this.getAllPayeeViewModel = getAllPayeeViewModel;
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
                var allPayee = response.Data.allRecipients;
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
                getAllPayeeViewModel.AllPayeeCollection.Remove(payee);
                getAllPayeeViewModel.PayeeNames.Remove(payee.PayeeName);
                getAllPayeeViewModel.AllPayee.Remove(payee);
            });
        }

        private async void BankingNotification_PayeeUpdated(Payee payee)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                Payee payeeToUpdate = getAllPayeeViewModel.AllPayeeCollection.FirstOrDefault(p => p.AccountNumber == payee.AccountNumber);
                if (payeeToUpdate != null)
                {
                    int index= getAllPayeeViewModel.AllPayeeCollection.IndexOf(payeeToUpdate);
                    payeeToUpdate.AccountHolderName = payee.AccountHolderName;
                    payeeToUpdate.BankName = payee.BankName;
                    payeeToUpdate.PayeeName = payee.PayeeName;
                    payeeToUpdate.IfscCode = payee.IfscCode;
                    getAllPayeeViewModel.AllPayeeCollection[index] = payeeToUpdate;

                }
            });
              
        }

        public async void populateData(IEnumerable<Payee> allPayee)
        {

         
                  getAllPayeeViewModel.AllPayeeCollection.Clear();
                  getAllPayeeViewModel.PayeeNames.Clear();
                  getAllPayeeViewModel.AllPayee.Clear();
                  foreach (var i in allPayee)
                  {
                      getAllPayeeViewModel.AllPayeeCollection.Add(i);
                      getAllPayeeViewModel.PayeeNames.Add(i.PayeeName);
                      getAllPayeeViewModel.AllPayee.Add(i);
                  }

            if(getAllPayeeViewModel.AllPayeeCollection.Count <= 0)
            {
                getAllPayeeViewModel.TextBoxVisibility = Visibility.Visible;

            }
            else
            {
                getAllPayeeViewModel.TextBoxVisibility = Visibility.Collapsed;

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
