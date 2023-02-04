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
            recipients = new GetAllPayee(new GetAllPayeeRequest(userId), new PresenterGetAllPayeeCallback(this));
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

        public void OnError(ZResponse<GetAllPayeeResponse> response)
        {
        }

        public void OnFailure(ZResponse<GetAllPayeeResponse> response)
        {

        }

        public  void OnSuccess(ZResponse<GetAllPayeeResponse> response)
        {
            var allPayee = response.Data.allRecipients;
            populateData(allPayee);
        }


        public async void populateData(List<Payee> allPayee)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  getAllPayeeViewModel.AllPayeeCollection.Clear();
                  getAllPayeeViewModel.PayeeNames.Clear();
                  getAllPayeeViewModel.AllPayee.Clear();
                  foreach (var i in allPayee)
                  {
                      getAllPayeeViewModel.AllPayeeCollection.Add(i);
                      getAllPayeeViewModel.PayeeNames.Add(i.PayeeName);
                      getAllPayeeViewModel.AllPayee.Add(i);
                      //Debug.WriteLine("/////////////////////////////////////////////////////////////////////////////////////////");
                      //Debug.WriteLine(i.PayeeName);

                  }
                      getAllPayeeViewModel?.ChangeVisibility?.ChangeVisibility(allPayee.Count <= 0);
              });
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
