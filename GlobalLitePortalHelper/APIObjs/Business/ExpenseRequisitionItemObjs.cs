using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using GlobalLitePortalHelper.APICore;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{
    public class RegExpenseRequisitionItemObj
    {

        [CheckNumber(0, ErrorMessage = "Expense requisition Id is required")]
        public int ExpenseRequisitionId { get; set; }
         
        public int ExpenseRequisitionItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "ProductItem Id is required")]
        public int ProductItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Classification Id is required")]
        public int ExpenseClassificationId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Category Id is required")]
        public int ExpenseCategoryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Type Id is required")]
        public int ExpenseTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Item Id is required")]
        public int ExpenseItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Item setting Id is required")]
        public int ExpenseItemSettingId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(350, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 350 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal UnitPrice { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity  is required")]
        public int Quantity { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Amount")]
        public decimal TotalAmount { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary type is required")]
        public int BeneficiaryType { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary Id is required")]
        public int BeneficiaryId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Remark must be between 1 and 500 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Remark is required")]
        public string GeneralRemark { get; set; }
        public bool StatusVal { get; set; }
        public int UniqueId { get; set; }
        public int Status { get; set; }

     

    }

    public class EditExpenseRequisitionItemObj
    {
        public long ExpenseRequisitionItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense requisition Id is required")]
        public long ExpenseRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }

        [CheckNumber(0, ErrorMessage = "ProductItem Id is required")]
        public int ProductItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Classification Id is required")]
        public int ExpenseClassificationId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Category Id is required")]
        public int ExpenseCategoryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Type Id is required")]
        public int ExpenseTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Item Id is required")]
        public int ExpenseItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Item setting Id is required")]
        public int ExpenseItemSettingId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(350, MinimumLength = 2, ErrorMessage = "Description must be between 2 and 350 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal UnitPrice { get; set; }

        [CheckNumber(0, ErrorMessage = "Quantity  is required")]
        public int Quantity { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Amount")]
        public decimal TotalAmount { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary type is required")]
        public int BeneficiaryType { get; set; }

        [CheckNumber(0, ErrorMessage = "Beneficiary Id is required")]
        public int BeneficiaryId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Remark must be between 1 and 500 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Remark is required")]
        public string GeneralRemark { get; set; }
        public bool StatusVal { get; set; }
        public int UniqueId { get; set; }
         
        public bool UniqueExists(int id, List<ExpenseRequisitionItemObj> listOfItems)
        {
            foreach (var item in listOfItems)
            {
                if (item.UniqueId == id)
                    return true;
                else
                    return false;
            }
            return default;
        }
    }



    public class ExpenseRequisitionItemSearchObj : AdminObj
    {
        public int ExpenseRequisitionItemId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class ExpenseRequisitionItemObj
    {
        public int ExpenseRequisitionItemId { get; set; }
        public int ExpenseRequisitionId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int ExpenseClassificationId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public int ExpenseTypeId { get; set; }
        public int ExpenseItemId { get; set; }
        public int ExpenseItemSettingId { get; set; }
        public string Description { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public int BeneficiaryType { get; set; }
        public int BeneficiaryId { get; set; }
        public string GeneralRemark { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        
        public int UniqueId { get; set; }
 
        public bool UniqueExists(int id, List<ExpenseRequisitionItemObj> listOfItems)
        {
            foreach (var item in listOfItems)
            {
                if (item.UniqueId == id)
                    return true;
                else
                    return false;
            }
            return default;
        }
    }
    //public class ExpenseRequisitionItemObj
    //{
    //    public int ExpenseRequisitionItemId { get; set; }
    //    public int ExpenseRequisitionId { get; set; }
    //    public int ClientId { get; set; }
    //    public int ProductId { get; set; }
    //    public int ProductItemId { get; set; }
    //    public int ExpenseClassificationId { get; set; }
    //    public int ExpenseCategoryId { get; set; }
    //    public int ExpenseTypeId { get; set; }
    //    public int ExpenseItemId { get; set; }
    //    public int ExpenseItemSettingId { get; set; }
    //    public string Description { get; set; }
    //    public decimal UnitPrice { get; set; }
    //    public int Quantity { get; set; }
    //    public int TotalAmount { get; set; }
    //    public int BeneficiaryType { get; set; }
    //    public int BeneficiaryId { get; set; }
    //    public string GeneralRemark { get; set; }
    //    public int Status { get; set; }
    //    public string StatusLabel { get; set; }
    //}

    public class ExpenseRequisitionItemRegRespObj
    {
        public int ExpenseRequisitionItemId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class ExpenseRequisitionItemRespObj
    {
        public List<ExpenseRequisitionItemObj> ExpenseRequisitionItems { get; set; }
        public APIResponseStatus Status { get; set; }
    }

}
