using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBankingApplication.ViewModel
{
    public abstract class BaseViewModel
    {
        public abstract void CallUseCase();
        public abstract void PopulateData();
    }
}
