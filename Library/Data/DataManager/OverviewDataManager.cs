using Library.Data.DataBaseService;
using Library.Domain;
using Library.Domain.UseCase;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class OverviewDataManager : BankingDataManager,IOverviewDataManager
    {
        public OverviewDataManager() : base(new DbHandler(), new NetHandler())
        {
        }

        public void GetOverviewData(OverviewRequest request, Overview.OverviewCallback response)
        {
           //validate and get records
        }
    
    }

    public class OverviewResponse : ZResponse<User>
    {
        public User CurrentUser;
        public bool NewUser;
        public Account CurrentAccount;
        public Card Card;
        public Branch Branch;
    }
}
