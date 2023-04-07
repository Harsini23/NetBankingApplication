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
            services.AddTransient<TransferAmountBaseViewModel, TransferAmountViewModel>();
            services.AddTransient<FDAccountBaseViewModel, FDAccountViewModel>();
            services.AddTransient<GetAllUsersBaseViewModel, GetAllUsersViewModel>();
            services.AddTransient<AccountTransactionsBaseViewModel, AccountTransactionsViewModel>();
            services.AddTransient<AddAccountBaseViewModel, AddAccountViewModel  >();
            services.AddTransient<AddUserBaseViewModel, AddUserViewModel>();
            services.AddTransient<DeletePayeeBaseViewModel, DeletePayeeViewModel>();
            services.AddTransient<GetBranchDetailsBaseViewModel, GetBranchDetailsViewModel>();
            services.AddTransient<EditPayeeBaseViewModel, EditPayeeViewModel>();
            services.AddTransient<UpdateUserBaseViewModel, UpdateUserViewModel>();
            services.AddTransient<PasswordVerificationBaseViewModel, PasswordVerificationViewModel>();
            services.AddSingleton<LoginBaseViewModel, LoginViewModel>();
            services.AddSingleton<OverviewBaseViewModel, OverviewViewModel>();
            services.AddTransient<TransactionHistoryBaseViewModel, TransactionHistoryViewModel>();
            services.AddTransient<AddPayeeBaseViewModel,AddPayeeViewModel>();
            services.AddSingleton<GetAllPayeeBaseViewModel,GetAllPayeeViewModel>();
            services.AddSingleton<GetAllAccountsBaseViewModel,GetAllAccountsViewModel>();
            services.AddSingleton<FDAccountDetailsBaseViewModel, FDAccountDetailsViewModel>();
            services.AddSingleton<UserUpdate>();


            //services.AddSingleton<IUserProfileViewModel, UserProfileViewModel>();
            return services.BuildServiceProvider();
        }
    }
}
