using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Admin
    {
        [PrimaryKey]
        public string EmployeeId { get; set; }
        public string Name { get;set; }
        public long MobileNumber { get; set; }
        public string EmaiId { get; set; }
        public string BranchId { get; set; }
        
    
        public Admin(string employeeId, string name, long mobileNumber, string emaiId, string branchId) { 
            EmployeeId = employeeId;
            Name = name;
            MobileNumber = mobileNumber;
            EmaiId = emaiId;
            BranchId = branchId;
        }

        public Admin()
        {
                
        }
    }

}
