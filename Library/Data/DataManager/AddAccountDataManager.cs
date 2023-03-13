using Library.Domain;
using Library.Domain.UseCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class AddAccountDataManager : IAddAccountDataManager
    {
        public void AddAccount(AddAccountRequest request, IUsecaseCallbackBaseCase<ZResponse<bool>> response)
        {
           
        }
    }
}
