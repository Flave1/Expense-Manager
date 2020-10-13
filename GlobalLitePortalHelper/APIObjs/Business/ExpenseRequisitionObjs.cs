using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using GlobalLitePortalHelper.APICore;
using PluglexHelper.PortalObjs;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{

    public class RegExpenseRequisitionObj : AdminObj
    { 

        [CheckNumber(0, ErrorMessage = "ProductItem Id is required")]
        public int ProductItemId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "Department Id is required")]
        public int DepartmentId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public int HashCode { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "RequestNumber must be between 2 and 30 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestNumber is required")]
        public string RequestNumber { get; set; }

        [CheckNumber(0, ErrorMessage = "Request type is required")]
        public int RequestType { get; set; }

        [CheckNumber(0, ErrorMessage = "Requester  is required")]
        public int RequesterId { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Amount")]
        public decimal TotalAmount { get; set; }

        [CheckNumber(0, ErrorMessage = "Workflow Stage Id is required")]
        public int WorkflowStageId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Remark must be between 1 and 500 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Remark is required")]
        public string GeneralRemark { get; set; }

        [CheckNumber(0, ErrorMessage = "Registered By Id  is required")]
        public int RegisteredBy { get; set; }

        public string TimeStampRegistered { get; set; }
        public int BeneficiaryId { get; set; }
        public int ExpenseCategoryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Registrar's Name is required")]
        public int Status { get; set; }
        public int BeneficiaryType { get; set; }
        public bool StatusVal { get; set; }
        public virtual ICollection<ExpenseRequisitionItemObj> ExpenseRequisitionItems { get; set; }


        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
    }

    public class EditExpenseRequisitionObj : AdminObj
    {

        public long ExpenseRequisitionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Department Id is required")]
        public int DepartmentId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }
        [CheckNumber(0, ErrorMessage = "ProductItem Id is required")]
        public int ProductItemId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }

        public int HashCode { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "RequestNumber must be between 2 and 30 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestNumber is required")]
        public string RequestNumber { get; set; }

        [CheckNumber(0, ErrorMessage = "Request type is required")]
        public int RequestType { get; set; }

        [CheckNumber(0, ErrorMessage = "Requester  is required")]
        public int RequesterId { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid Amount")]
        public decimal TotalAmount { get; set; }

        [CheckNumber(0, ErrorMessage = "Workflow Stage Id is required")]
        public int WorkflowStageId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "BeneficiaryType Stage Id is required")]
        public int BeneficiaryType { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Remark must be between 1 and 500 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Remark is required")]
        public string GeneralRemark { get; set; }

        [CheckNumber(0, ErrorMessage = "Registered By Id  is required")]
        public int RegisteredBy { get; set; }
        public string TimeStampRegistered { get; set; }

        [CheckNumber(0, ErrorMessage = "Registrar's Name is required")]
        public int Status { get; set; }
        public virtual ICollection<ExpenseRequisitionItemObj> ExpenseRequisitionItems { get; set; } 
    }

    public class ExpenseRequisitionSearchObj : AdminObj
    {
        public int ExpenseRequisitionId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class ExpenseRequisitionObj
    {
        public long ExpenseRequisitionId { get; set; }
        public int DepartmentId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string Title { get; set; }
        public int HashCode { get; set; }
        public string RequestNumber { get; set; }
        public int RequestType { get; set; }
        public int RequesterId { get; set; }
        public decimal TotalAmount { get; set; }
        public int WorkflowStageId { get; set; }
        public string GeneralRemark { get; set; }
        public int RegisteredBy { get; set; }
        public int BeneficiaryType { get; set; }
        public int BeneficiaryId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public List<ExpenseRequisitionItemObj> ExpenseRequisitionItems { get; set; }

        



        //using it on retirement 
        public int RetirementMode { get; set; }
        public string AmountRetired { get; set; }
        public string VoucherNumber { get; set; }
        public string RecievedItemIds { get; set; }
        public string AmountSpent { get; set; }
        public string Date { get; set; }
    }



    public class ExpenseRequisitionRespObj
    {
        public List<ExpenseRequisitionObj> ExpenseRequisitions { get; set; }
        public APIResponseStatus Status { get; set; }
    }




    public class ExpenseRequisitionRegRespObj  
    {
        public int ExpenseRequisitionId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

}
