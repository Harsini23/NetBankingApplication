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
                List<FDRates> fDRates = new List<FDRates>();
                populateFDRates(ref fDRates);

                DbHandler.InsertBankBranchDetails(branches);
                DbHandler.InsertDefaultFDRates(fDRates);
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

        private void populateFDRates(ref List<FDRates> fDRates)
        {
            fDRates.Add(new FDRates
            {
                MinDuration = 7,
                MaxDuration = 29,
                Rate = 4.25
            }); 
            fDRates.Add(new FDRates
            {
                MinDuration = 30,
                MaxDuration = 45,
                Rate = 4.50
            }); 
            fDRates.Add(new FDRates
            {
                MinDuration = 46,
                MaxDuration = 184,
                Rate = 5
            }); 
            fDRates.Add(new FDRates
            {
                MinDuration = 185,
                MaxDuration = 364,
                Rate = 5.5
            });
            fDRates.Add(new FDRates
            {
                MinDuration = 366,
                MaxDuration = 1095,
                Rate = 6
            }); 
            fDRates.Add(new FDRates
            {
                MinDuration = 1096,
                MaxDuration = 1825,
                Rate = 6.5
            });
            fDRates.Add(new FDRates
            {
                MinDuration = 1826,
                MaxDuration = 10000,
                Rate = 7
            });

        }


    }

    //public class DefaultAdminResponse : ZResponse<DefaultAdminBObj>
    //{

    //}
}
