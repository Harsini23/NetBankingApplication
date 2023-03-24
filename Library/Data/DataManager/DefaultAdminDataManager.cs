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
        public DefaultAdminDataManager(IDbHandler DbHandler, INetHandler NetHandler) : base(DbHandler, NetHandler)
        {
            if (!DbHandler.CheckIfAdminExists("Admin"))
            {
                DbHandler.CreateDefaultAdminIfNotExists(new Credentials
                {
                    UserId = "Admin",
                    Password = PasswordEncryption.BytesToString(PasswordEncryption.EncryptPassword("UserAdmin@1")),
                    IsAdmin = true,
                    NewUser = false
                });
                List<Branch> branches = new List<Branch>();
                populateBranch(ref branches);
                

                DbHandler.InsertBankBranchDetails(branches);
            }
           
          
        }

        private void populateBranch(ref List<Branch> branches)
        {
            branches.Add(new Branch
            {
                BId = "B001",
                BName = "Chennai - Tambaram",
                BCity = "Chennai",
                IfscCode = "Zoho001"
            });
            branches.Add(new Branch
            {
                BId = "B002",
                BName = "Chennai - Annanagar",
                BCity = "Chennai",
                IfscCode = "Zoho002"
            });
            branches.Add(new Branch
            {
                BId = "B003",
                BName = "Trichy - Guduvanchery",
                BCity = "Trichy",
                IfscCode = "Zoho003"
            });
            branches.Add(new Branch
            {
                BId = "B004",
                BName = "Madurai - Guduvanchery",
                BCity = "Madurai",
                IfscCode = "Zoho004"
            });
            branches.Add(new Branch
            {
                BId = "B005",
                BName = "Tenkasi - Guduvanchery",
                BCity = "Tenkasi",
                IfscCode = "Zoho005"
            });

        }


    }

    //public class DefaultAdminResponse : ZResponse<DefaultAdminBObj>
    //{

    //}
}
