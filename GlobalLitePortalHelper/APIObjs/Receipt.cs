using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace GlobalLitePortalHelper.APIObjs
{
    public class Receipt
    {
        public string ReceiptNumber { get; set; }
        public string VoucherExtension { get; set; }

        [NotMapped]
        public HttpPostedFileBase   ImageFile { get; set; }
    }
}
