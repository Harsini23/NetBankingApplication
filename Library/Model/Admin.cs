using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Admin
    {
        public int BranchId { get; set; }
        public string EmployeeId { get; set; }
        public Admin( int branchId, string employeeId)
        {
            BranchId = branchId;
            EmployeeId = employeeId;
        }
        public Admin()
        {
                
        }
    }

}
