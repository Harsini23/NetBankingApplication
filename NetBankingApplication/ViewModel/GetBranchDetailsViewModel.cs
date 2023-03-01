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
using System.Threading.Tasks;
using static Library.Domain.UseCase.GetBranchDetails;

namespace NetBankingApplication.ViewModel
{
    public class GetBranchDetailsViewModel : GetBranchDetailsBaseViewModel
    {
        GetBranchDetails getBranchDetails;
        
        public override void FetchBranchDetails(string BId)
        {
          getBranchDetails= new GetBranchDetails(new BranchDetailsRequest() { BranchId=BId},new PresenterGetBranchDetailsCallback(this));
            getBranchDetails.Execute();
        }

        public override void FetchBranchDetails()
        {
            getBranchDetails = new GetBranchDetails(new BranchDetailsRequest(), new PresenterGetBranchDetailsCallback(this));
            getBranchDetails.Execute();
        }
    }

  

    public class PresenterGetBranchDetailsCallback : IPresenterGetBranchDetailsCallback
    {
        GetBranchDetailsViewModel GetBranchDetailsViewModel;
        public PresenterGetBranchDetailsCallback(GetBranchDetailsViewModel GetBranchDetailsViewModel)
        {
            this.GetBranchDetailsViewModel = GetBranchDetailsViewModel;
        }
        public void OnError(BException response)
        {
           
        }

        public void OnFailure(ZResponse<GetBranchDetailsResponse> response)
        {
          
        }

        public async void OnSuccessAsync(ZResponse<GetBranchDetailsResponse> response)
        {
            await SwitchToMainUIThread.SwitchToMainThread(() =>
            {
                if (response.Data.Data != null)
                {
                    GetBranchDetailsViewModel.City = response.Data.Data.BCity;
                    GetBranchDetailsViewModel.BName = response.Data.Data.BName;
                    GetBranchDetailsViewModel.IfscCode = response.Data.Data.IfscCode;
                }
                if(response.Data.allBranchDetails != null)
                {
                    GetBranchDetailsViewModel.allBranchDetails = response.Data.allBranchDetails;
                }
               
            });

        }
    }

    public abstract class GetBranchDetailsBaseViewModel : NotifyPropertyBase
    {
        public abstract void FetchBranchDetails(String BId);
        public abstract void FetchBranchDetails();
        public ObservableCollection<Branch> allBranchDetails;

        private string _city = String.Empty;
        public string City
        {
            get { return this._city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _bName = String.Empty;
        public string BName
        {
            get { return this._bName; }
            set
            {
                _bName = value;
                OnPropertyChanged(nameof(BName));
            }
        }

        private string _ifscCode = String.Empty;
        public string IfscCode
        {
            get { return this._ifscCode; }
            set
            {
                _ifscCode = value;
                OnPropertyChanged(nameof(IfscCode));
            }
        }

    }
}
