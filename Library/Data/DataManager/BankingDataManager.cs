using Library.Data.DataBaseService;
using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{

    public class BankingDataManager
    {
        protected static IDbHandler DbHandler;
        protected static INetHandler NetHandler;

        public BankingDataManager(IDbHandler dbHandler, INetHandler netHandler)
        {
            DbHandler = dbHandler;
            NetHandler = netHandler;
        }
    }
   
}
