using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataBaseService
{
    public interface IService
    {
        //CRUD operation for all class with type R
         void GetData<T>() where T: new();
        //void UpdateData<T>() where T : new();
        //void DeleteData<T>() where T : new();
    }
}
