using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetBankingApplication.ViewModel;

namespace NetBankingApplication.View
{
    public class PresenterService
    {
        public IServiceProvider Services { get; }
        //singleton so constructor called and ConfigureServices is called only once
        private PresenterService()
        {
            Services = ConfigureServices();
        }
        public static PresenterService _instance;
        public static PresenterService GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PresenterService();
            }
            return _instance;
        }
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();
            //Services
            services.AddSingleton<LoginBaseViewModel, LoginViewModel>();
            services.AddSingleton<OverviewBaseViewModel, OverviewViewModel>();
            services.AddSingleton<TransactionHistoryBaseViewModel, TransactionHistoryViewModel>();
            services.AddSingleton<AddPayeeBaseViewModel,AddPayeeViewModel>();
            services.AddSingleton<GetAllPayeeBaseViewModel,GetAllPayeeViewModel>();
            services.AddSingleton<GetAllAccountsBaseViewModel,GetAllAccountsViewModel>();
            services.AddSingleton<TransferAmountBaseViewModel,TransferAmountViewModel>();
            services.AddSingleton<AccountTransactionsBaseViewModel,AccountTransactionsViewModel>();
            services.AddSingleton<AddUserBaseViewModel,AddUserViewModel>();

            //services.AddSingleton<IUserProfileViewModel, UserProfileViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
