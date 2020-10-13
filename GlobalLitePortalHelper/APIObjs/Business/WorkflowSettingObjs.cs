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
    public class RegWorkflowSettingObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductItemId is required")]
        public int ProductItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RegisteredBy is required")]
        public int RegisteredBy { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ApprovalWorkflow is required")]
        public string ApprovalWorkflow { get; set; }
        public string TimeStampRegiestered { get; set; }
        public bool StatusVal { get; set; }
        public int Status { get; set; }

        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
    }

    public class EditWorkflowSettingObj : AdminObj
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "ApprovalWorkflow is required")]
        public string ApprovalWorkflow { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowSettingId is required")]
        public int WorkflowSettingId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductItemId is required")]
        public int ProductItemId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]

        public int RequestType { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "RegisteredBy is required")]
        public int RegisteredBy { get; set; }
        public string TimeStampRegiestered { get; set; }


        public int Status { get; set; } 
        public bool StatusVal { get; set; } 

        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public string ProductItemName { get; set; }
    }

    public class WorkflowSettingSearchObj : AdminObj
    {
        public int WorkflowSettingId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class WorkflowSettingObj
    {
        public int WorkflowSettingId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int RequestType { get; set; }
        public int RegisteredBy { get; set; }
        public string ApprovalWorkflow { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }
    }

   

    public class WorkflowSettingRegRespObj  
    {
        public int WorkflowSettingId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class WorkflowSettingRespObj
    {
        public List<WorkflowSettingObj> WorkflowSettings { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
