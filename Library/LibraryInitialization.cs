using Library.Data;
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
            if (createTableInstance == null)
            {
                createTableInstance = CreateTables.GetInstance();
            }
        }
    }
}
