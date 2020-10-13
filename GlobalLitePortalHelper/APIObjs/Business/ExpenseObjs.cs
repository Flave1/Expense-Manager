using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluglexHelper.PortalObjs;
using XPLUG.WEBTOOLKIT;

namespace PlugLitePortalHelper.APIObjs
{
    public class RegExpenseObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowItemId is required")] 
        public int WorkflowItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TotalAmount is required")]
        public int TotalAmount { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountApproved is required")]
        public int AmountApproved { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountDisbursed is required")]
        public int AmountDisbursed { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Balance is required")]
        public int Balance { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastDisbursementBy is required")]
        public int LastDisburesementBy { get; set; }
        public string LastDisburesementDate { get; set; }
        public string TimeStampCreated { get; set; } 
        public int Status { get; set; }              
    }

    public class EditExpenseObj : AdminObj
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseId is required")]
        public int ExpenseId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowItemId is required")]
        public int WorkflowItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TotalAmount is required")]
        public int TotalAmount { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountApproved is required")]
        public int AmountApproved { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountDisbursed is required")]
        public int AmountDisbursed { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Balance is required")]
        public int Balance { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastDisbursementBy is required")]
        public int LastDisburesementBy { get; set; }
        public string LastDisburesementDate { get; set; }
        public string TimeStampCreated { get; set; }
        public int Status { get; set; }
    }

    public class ExpenseSearchObj : AdminObj
    {
        public int ExpenseId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class ExpenseObj : AdminObj
    {
        public long ExpenseId { get; set; }
        public long WorkflowItemId { get; set; }
        public long ExpenseRequisitionId { get; set; }
        public int RequestType { get; set; }
        public string Title { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal AmountApproved { get; set; }
        public decimal AmountDisbursed { get; set; }
        public decimal Balance { get; set; }
        public int LastDisburesementBy { get; set; }
        public string LastDisburesementDate { get; set; }
        public string TimeStampCreated { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public string ReguisitionDate { get; set; }
    }


    public class ExpenseRespObj
    {
        public List<ExpenseObj> Expenses { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class ExpenseRegRespObj
    {
        public long ExpenseId { get; set; }
        public APIResponseStatus Status { get; set; }
    }







}
