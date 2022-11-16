using Library.Domain;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class OverviewDataManager : BankingDataManager
    {
        public OverviewDataManager(IDbHandler dbHandler, INetHandler netHandler) : base(dbHandler, netHandler)
        {
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
