using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain
{

    public interface IDefaultAdminDataManager
    {
        void AddDefaultAdmin();

    }
    public class DefaultAdmin:UseCaseBase<DefaultAdminBObj>
    {
        private IDefaultAdminDataManager _adminDataManager;
        public DefaultAdmin()
        {
            var serviceProviderInstance = ServiceProvider.GetInstance();
            _adminDataManager = serviceProviderInstance.Services.GetService<IDefaultAdminDataManager>();
        }
        public override void Action()
        {
            this._adminDataManager.AddDefaultAdmin(); 
        }
    }
}
