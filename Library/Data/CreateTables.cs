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
        IDbAdapter adapter;

        public CreateTables()
        {
           adapter= Domain.ServiceProvider.GetInstance().Services.GetService<IDbAdapter>();
            InstantiateAllTables();
        }
      
        public void InstantiateAllTables()
        {
            adapter.Create(new Credentials());
            adapter.Create(new User());
            adapter.Create(new Admin());
            adapter.Create(new Transaction());
            adapter.Create(new Branch());
            adapter.Create(new Card());
            adapter.Create(new Account());
            adapter.Create(new Payee());
            adapter.Create(new UserAccounts());
            adapter.Create(new FDAccount());
        }
       
    }
}
