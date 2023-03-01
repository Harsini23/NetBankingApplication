using Library.Data;
using Library.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library
{
    public class LibraryInitialization
    {
        CreateTables createTableInstance;
        private static LibraryInitialization _instance;

        private LibraryInitialization() { }

    
        public static LibraryInitialization GetInstance()
        {
            if (_instance == null)
            {
                _instance = new LibraryInitialization();
            }
            return _instance;
        }
        public void InitializeDb()
        {
            CreateTables createTables =  Library.Domain.ServiceProvider.GetInstance().Services.GetService<CreateTables>();
            if (createTableInstance == null)
            {
                createTableInstance = createTables;
            }
        }
    }
}
