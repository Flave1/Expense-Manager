using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalLitePortalHelper.APIObjs
{
    public class ReportExcelObj
    {
        
        public string Title { get; set; }
        public int Item { get; set; } 
        public string Request_No { get; set; }
        public string Total_Amount { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public int Beneficiary { get; set; }
    }
}
