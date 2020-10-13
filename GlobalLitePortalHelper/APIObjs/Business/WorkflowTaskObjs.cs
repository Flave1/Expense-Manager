using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PluglexHelper.PortalObjs;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortalHelper.APIObjs
{
    public class RegWorkflowTaskObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowSettingId is required")]
        public int WorkflowSettingId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductItemId is required")]
        public int ProductItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskTitle is required")]
        public string TaskTitle { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskDescription is required")]
        public string TaskDescription { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "StartDate is required")]
        public string StartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowTaskId is required")]
        public string DueDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "DueDate is required")]
        public string StartTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "DueTime is required")]
        public string DueTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeId is required")]
        public int ApprovalLevelId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeEmail is required")]
        public int AssigneeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeEmail is required")]
        public string AssigneeEmail { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "IsEmailSent is required")]
        public bool IsEmailSent { get; set; }
        public string TimeStampCreated { get; set; }
        public int Status { get; set; } 
        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
    }

    public class EditWorkflowTaskObj : AdminObj
    {

        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowSettingId is required")]
        public int WorkflowSettingId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowTaskId is required")]
        public int WorkflowTaskId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductItemId is required")]
        public int ProductItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskTitle is required")]
        public string TaskTitle { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "TaskDescription is required")]
        public string TaskDescription { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "StartDate is required")]
        public string StartDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowTaskId is required")]
        public string DueDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "DueDate is required")]
        public string StartTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "DueTime is required")]
        public string DueTime { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeId is required")]
        public int ApprovalLevelId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeEmail is required")]
        public int AssigneeId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "AssigneeEmail is required")]
        public string AssigneeEmail { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "IsEmailSent is required")]
        public bool IsEmailSent { get; set; }
        public string TimeStampCreated { get; set; }
        public int Status { get; set; }
    }

    public class WorkflowTaskSearchObj : AdminObj
    {
        public int WorkflowTaskId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
    }

    public class WorkflowTaskObj
    {
        public int WorkflowTaskId { get; set; }
        public int WorkflowSettingId { get; set; }
        public int ProductItemId { get; set; }
        public int ExpenseRequisitionId { get; set; }
        public int RequestType { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public string TaskTitle { get; set; }
        public int HashCode { get; set; }
        public string TaskDescription { get; set; }
        public string StartDate { get; set; }
        public string DueDate { get; set; }
        public string StartTime { get; set; }
        public string DueTime { get; set; }
        public int ApprovalLevelId { get; set; }
        public int AssigneeId { get; set; }
        public string AssigneeEmail { get; set; }
        public bool IsEmailSent { get; set; }
        public string TimeStampCreated { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }

    }

   

    public class WorkflowTaskRegRespObj  
    {
        public int WorkflowTaskId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowTaskRespObj
    {
        public List<WorkflowTaskObj> WorkflowTasks { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
