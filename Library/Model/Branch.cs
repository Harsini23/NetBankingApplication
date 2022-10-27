using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Branch
    {
        public int BId { get; set; }
        public string BName { get; set; }
        public string BCity { get; set; }
        public string IfscCode { get; set; }
        public Branch(int bId, string bName, string bCity, string ifscCode)
        {
            BId = bId;
            BName = bName;
            BCity = bCity;
            IfscCode = ifscCode;
        }
        public Branch()
        {

        }
    }
}
