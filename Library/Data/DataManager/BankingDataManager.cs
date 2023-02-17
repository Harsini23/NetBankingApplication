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

    //singleton class
    //public class BankingDataManager
    //{
    //    protected static IDbHandler DbHandler;
    //    protected static INetHandler NetHandler;

    //    private static BankingDataManager bankingDataManager = null;

    //    private static DbHandler dbhandlerInstance = null;
    //    private static NetHandler nethandlerInstance = null;
    //    private BankingDataManager()
    //    {
    //        dbhandlerInstance = new DbHandler();
    //        nethandlerInstance = new NetHandler();

    //    }
    //    public static BankingDataManager GetInstance()
    //    {
    //        if(bankingDataManager == null)
    //        {
    //            bankingDataManager = new BankingDataManager();
    //        }
    //         return bankingDataManager;
    //    }

    //    //public static DbHandler GetDbHandlerInstance()
    //    //{
    //    //    if (dbhandlerInstance == null)
    //    //    {
    //    //        dbhandlerInstance = new DbHandler();
    //    //    }
    //    //    return dbhandlerInstance;
    //    //}
    //    //public static NetHandler GetNetHandlerInstance()
    //    //{
    //    //    if (nethandlerInstance == null)
    //    //    {
    //    //        nethandlerInstance= new NetHandler();
    //    //    }
    //    //    return nethandlerInstance;
    //    //}
    //    //public BankingDataManager(IDbHandler dbHandler, INetHandler netHandler)
    //    //{
    //    //    DbHandler = dbHandler;
    //    //    NetHandler = netHandler;
    //    //}
    //}

   
}
