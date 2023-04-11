using Library.Data.DbAdapter;
using Library.Domain;
using Library.Model;
using Microsoft.Extensions.DependencyInjection;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data
{
    //call once when app is first initialized?
    public class CreateTables
    {
        private static CreateTables _instance;
       private IDbAdapter _adapter;

        public CreateTables()
        {
           _adapter= Domain.ServiceProvider.GetInstance().Services.GetService<IDbAdapter>();
            InstantiateAllTables();
        }
      
        public void InstantiateAllTables()
        {
            _adapter.Create(new Credentials());
            _adapter.Create(new User());
            _adapter.Create(new Admin());
            _adapter.Create(new AmountTransaction());
            _adapter.Create(new Branch());
            _adapter.Create(new Card());
            _adapter.Create(new Account());
            _adapter.Create(new Payee());
            _adapter.Create(new UserAccounts());
            _adapter.Create(new FDAccount());
            _adapter.Create(new FDRates());
        }
       
    }
}
