using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs.Business
{
    public class RegReceiptObj
    { 
        [CheckNumber(0, ErrorMessage = " Expense Id is required")]
        public long ExpenseId { get; set; }

        [CheckNumber(0, ErrorMessage = "Retirement Id  is required")]
        public long RetirementId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense requisition Id is required")]
        public long ExpenseRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense requisition Item Id is required")]
        public string ExpenseRequisitionItemId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "ReceiptNunber must be between 1 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ReceiptNunber is required")]
        public string ReceiptNunber { get; set; }
        public string ReceiptFileName { get; set; }

        public byte[] ReceiptDocument { get; set; }
    }
} 
