using Library;
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

namespace NetBankingApplication.ViewModel
{
    public class GetAllPayeeViewModel : GetAllPayeeBaseViewModel
    {
        GetAllPayee recipients;
        public IViewAndEditPayeeVM viewAndEditPayeeVMCallback;
        public GetAllPayeeViewModel()
        {
            PresenterDeletePayeeCallback.ValueChanged += PresenterDeletePayeeCallback_ValueChanged;
        }

        private void PresenterDeletePayeeCallback_ValueChanged(string id)
        {
            GetAllPayee(id);
        }

        public override void GetAllPayee(string userId)
        {
            viewAndEditPayeeVMCallback =  ChangeVisibility;
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
                populateData(SortedPayee);
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
                  getAllPayeeViewModel?.ChangeVisibility?.ChangeVisibility(getAllPayeeViewModel.AllPayeeCollection.Count <= 0);
             
        }
    }



    public abstract class GetAllPayeeBaseViewModel : NotifyPropertyBase
    {
        public ObservableCollection<Payee> AllPayeeCollection = new ObservableCollection<Payee>();
        public abstract void GetAllPayee(string userId);
        public List<Payee> AllPayee = new List<Payee>() { };
        public List<String> PayeeNames = new List<String>();
        public IViewAndEditPayeeVM ChangeVisibility;
    }

    public interface IViewAndEditPayeeVM
    {
        void ChangeVisibility(bool visible);
    }
}
