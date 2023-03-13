using Library.Data;
using Library.Data.DataBaseService;
using Library.Data.DataManager;
using Library.Domain.UseCase;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Library.Domain
{
    public sealed class ServiceProvider 
    {
        public IServiceProvider Services { get; }

        //singleton so constructor called and ConfigureServices is called only once
        private ServiceProvider()
        {
            Services = ConfigureServices();
        }

        public static ServiceProvider _instance;
        public static ServiceProvider GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ServiceProvider();
            }
            return _instance;
        } 

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            //Services
            services.AddSingleton<ILoginDataManager, LoginDataManager>();
            services.AddSingleton<IAddAccountDataManager, AddAccountDataManager>();
            services.AddSingleton<IResetPasswordDataManager, ResetPasswordDataManager>();
            services.AddSingleton<ITransactionHistoryDataManager, TransactionHistoryDataManager>();
            services.AddSingleton<IAddPayeeDataManager, AddPayeeDataManager>();
            services.AddSingleton<ITransferAmountDataManager, TransferAmountDataManager>();
            services.AddSingleton<IGetAllPayeeDataManager, GetAllPayeeDataManager>();
            services.AddSingleton<IGetAllAccountsDataManager, GetAllAccountsDataManager>();
            services.AddSingleton<IAccountTransactionsDataManager, AccountTransactionsDataManager>();
            services.AddSingleton<IAddUserDataManager, AddUserDataManager>();
            services.AddSingleton<IDeletePayeeDataManager, DeletePayeeDataManager>();
            services.AddSingleton<IGetBranchDetailsDataManager, GetBranchDetailsDataManager>();
            services.AddSingleton<IOverviewDataManager, OverviewDataManager>();
            services.AddSingleton<IDefaultAdminDataManager, DefaultAdminDataManager>();
            services.AddSingleton<IEditPayeeDataManager, EditPayeeDataManager>();
            services.AddSingleton<IUpdateUserDataManager, UpdateUserDataManager>();
            services.AddSingleton<ICheckPasswordDataManager, CheckPasswordDataManager>();
            services.AddSingleton<IGetAllUsersDataManager, GetAllUsersDataManager>();

            services.AddSingleton<IDbHandler, DbHandler>();
            services.AddSingleton<INetHandler, NetHandler>();

            services.AddSingleton<DataBasePath>();
            services.AddSingleton<DatabaseConnection>();
            services.AddSingleton<CreateTables>();
            services.AddSingleton<DbHandler>();

            return services.BuildServiceProvider();
        }

    }
}
