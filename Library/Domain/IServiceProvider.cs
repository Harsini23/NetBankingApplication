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
            services.AddSingleton<IResetPasswordDataManager, ResetPasswordDataManager>();
            services.AddSingleton<ITransactionHistoryDataManager, TransactionHistoryDataManager>(); ;

            return services.BuildServiceProvider();
        }

    }
}
