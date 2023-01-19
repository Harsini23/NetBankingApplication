using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class GetAllAccountsViewModel : GetAllAccountsBaseViewModel
    {
        GetAllAccounts getAllAccounts;

        ISwitchUserView tranferView;
        public override void GetAllAccounts(string userId)
        {
            SetValueForCallback();
            getAllAccounts = new GetAllAccounts(new GetAllAccountsRequest(userId),  new PresenterGetAllAccountsCallback(this));
            getAllAccounts.Execute();
        }
        private void SetValueForCallback()
        {
            tranferView = TransferAmountView;
        }
    }

    public class PresenterGetAllAccountsCallback : IPresenterGetAllAccountsCallback
    {
        private GetAllAccountsViewModel GetAllAccountsViewModel;
        public PresenterGetAllAccountsCallback()
        {

        }
        public PresenterGetAllAccountsCallback(GetAllAccountsViewModel getAllAccountsViewModel)
        {
            this.GetAllAccountsViewModel = getAllAccountsViewModel;
        }

        public void OnError(ZResponse<GetAllAccountsResponse> response)
        {
        }

        public void OnFailure(ZResponse<GetAllAccountsResponse> response)
        {

        }

        public void OnSuccess(ZResponse<GetAllAccountsResponse> response)
        {
            var allAccounts = response.Data.allAccount;
             populateData(allAccounts);
            handleCallbackAsync();
           
            //GetAllAccountsViewModel.AllAccounts.Clear();
            //GetAllAccountsViewModel.AllAccountNumbers.Clear();

            //GetAllAccountsViewModel.AllAccounts = allAccounts;
            //foreach(var i in response.Data.allAccount)
            //{
            //    GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
            //  //  GetAllAccountsViewModel.AllAccounts.Add(i);

            //}
        }
            private async Task handleCallbackAsync()
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {
                  GetAllAccountsViewModel?.TransferAmountView?.SwitchBasedOnUserAccount();
              });
            }

        public async void populateData(List<Account> allAccounts)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(
              Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
              {

                  GetAllAccountsViewModel.AllAccounts.Clear();
                  GetAllAccountsViewModel.accounts.Clear();

                  GetAllAccountsViewModel.AllAccountNumbers.Clear();
                 // GetAllAccountsViewModel.AllAccounts = allAccounts;
                  foreach (var i in allAccounts)
                  {
                      GetAllAccountsViewModel.AllAccountNumbers.Add(i.AccountNumber);
                      GetAllAccountsViewModel.AllAccounts.Add(i);
                      GetAllAccountsViewModel.accounts.Add(i);

                  }
              });
        }
    }
    public abstract class GetAllAccountsBaseViewModel : NotifyPropertyBase
    {
        public abstract void GetAllAccounts(string userId);
        public List<Account> AllAccounts = new List<Account>();
        public ObservableCollection<String> AllAccountNumbers = new ObservableCollection<string>();
        public ObservableCollection<Account> accounts= new ObservableCollection<Account>();
        public ISwitchUserView TransferAmountView { get; set; }

    }

    public interface ISwitchUserView
    {
         void SwitchBasedOnUserAccount();
    }
}
