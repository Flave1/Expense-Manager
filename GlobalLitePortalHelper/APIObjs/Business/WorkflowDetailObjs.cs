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
    public class RegWorkflowDetailObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowTaskId is required")]
        public int WorkflowTaskId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskTitle is required")]
        public string TaskTitle { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Recommendation is required")]
        public string Recommendation { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ApprovalLevelId is required")]
        public int ApprovalLevelId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeId is required")]
        public int AssigneeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "IsEmailSent is required")]
        public bool IsEmailSent { get; set; }
        public string TimeStampRegistered { get; set; } 
        public int Status { get; set; } 



        public string BeneficiaryName { get; set; }
        public string GeeneralRemark { get; set; }
    }

    public class EditWorkflowDetailObj : AdminObj
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowDetailId is required")]
        public int WorkflowDetailId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowTaskId is required")]
        public int WorkflowTaskId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskTitle is required")]
        public string TaskTitle { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Recommendation is required")]
        public string Recommendation { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ApprovalLevelId is required")]
        public int ApprovalLevelId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeId is required")]
        public int AssigneeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "IsEmailSent is required")]
        public bool IsEmailSent { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }

    }

    public class WorkflowDetailSearchObj : AdminObj
    {
        public int WorkflowDetailId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class WorkflowDetailObj
    {
        public int WorkflowDetailId { get; set; }
        public int WorkflowTaskId { get; set; }
        public int ExpenseRequisitionId { get; set; }
        public int RequestType { get; set; }
        public string TaskTitle { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public string Recommendation { get; set; }
        public int ApprovalLevelId { get; set; }
        public int AssigneeId { get; set; }
        public bool IsEmailSent { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
    }

   

    public class WorkflowDetailRegRespObj  
    {
        public int WorkflowDetailId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowDetailRespObj
    {
        public List<WorkflowDetailObj> WorkflowDetails { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
