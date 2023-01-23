using Library.Data.DataManager;
using Library.Domain;
using Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public class GetBranchDetailsViewModel : GetBranchDetailsBaseViewModel
    {
        GetBranchDetails getBranchDetails;
        
        public override void FetchBranchDetails(string BId)
        {
          getBranchDetails= new GetBranchDetails("B001",new PresenterGetBranchDetailsCallback(this));
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
        public void OnError(ZResponse<GetBranchDetailsResponse> response)
        {
           
        }

        public void OnFailure(ZResponse<GetBranchDetailsResponse> response)
        {
          
        }

        public void OnSuccess(ZResponse<GetBranchDetailsResponse> response)
        {
            GetBranchDetailsViewModel.City= response.Data.Data.BCity;
            GetBranchDetailsViewModel.BName= response.Data.Data.BName;
            GetBranchDetailsViewModel.IfscCode = response.Data.Data.IfscCode;
        }
    }

    public abstract class GetBranchDetailsBaseViewModel : NotifyPropertyBase
    {
        public abstract void FetchBranchDetails(String BId);

        private string _city = String.Empty;
        public string City
        {
            get { return this._city; }
            set
            {
                _city = value;
                OnPropertyChangedAsync(nameof(City));
                //SetProperty(ref _response, value);
            }
        }

        private string _bName = String.Empty;
        public string BName
        {
            get { return this._bName; }
            set
            {
                _bName = value;
                OnPropertyChangedAsync(nameof(BName));
                //SetProperty(ref _response, value);
            }
        }

        private string _ifscCode = String.Empty;
        public string IfscCode
        {
            get { return this._ifscCode; }
            set
            {
                _ifscCode = value;
                OnPropertyChangedAsync(nameof(IfscCode));
                //SetProperty(ref _response, value);
            }
        }

    }
}
