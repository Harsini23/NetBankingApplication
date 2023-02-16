using Library.Data.DataBaseService;
using Library.Domain;
using Library.Model;
using Library.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Data.DataManager
{
    public class DefaultAdminDataManager : BankingDataManager, IDefaultAdminDataManager
    {
        public void AddDefaultAdmin()
        {
            
        }
        public DefaultAdminDataManager() : base(new DbHandler(), new NetHandler())
        {
            if (!DbHandler.CheckIfUserExists("Admin"))
            {
                DbHandler.CreateDefaultAdminIfNotExists(new Credentials
                {
                    UserId = "Admin",
                    Password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword("UserAdmin@1")),
                    IsAdmin = true,
                    NewUser = false
                });
            }
          
        }

    }

    //public class DefaultAdminResponse : ZResponse<DefaultAdminBObj>
    //{

    //}
}
