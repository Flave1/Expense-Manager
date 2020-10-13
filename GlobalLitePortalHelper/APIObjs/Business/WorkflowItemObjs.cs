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
    public class RegWorkflowItemObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "CurrentStageId is required")]
        public int CurrentStageId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastStageComment is required")]
        public string LastStageComment { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastStageBy is required")]
        public int LastStageBy { get; set; } 
        public string LastStageTimeStamp { get; set; }
        public string TimeStampRegistered { get; set; } 
    }

    public class EditWorkflowItemObj : AdminObj
    {


        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowItemId is required")]
        public int WorkflowItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public int ExpenseRequisitionId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "CurrentStageId is required")]
        public int CurrentStageId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastStageComment is required")]
        public string LastStageComment { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastStageBy is required")]
        public int LastStageBy { get; set; }
        public string LastStageTimeStamp { get; set; }
        public string TimeStampRegistered { get; set; }
    }

    public class WorkflowItemSearchObj : AdminObj
    {
        public int WorkflowItemId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class WorkflowItemObj
    {
        public int WorkflowItemId { get; set; }
        public int ExpenseRequisitionId { get; set; }
        public int RequestType { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int CurrentStageId { get; set; }
        public string LastStageComment { get; set; }
        public int LastStageBy { get; set; }
        public string LastStageTimeStamp { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
    }

   

    public class WorkflowItemRegRespObj  
    {
        public int WorkflowItemId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowItemRespObj
    {
        public List<WorkflowItemObj> WorkflowItems { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
