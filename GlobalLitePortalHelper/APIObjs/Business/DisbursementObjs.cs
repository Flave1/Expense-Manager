using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPLUG.WEBTOOLKIT; 

// ReSharper disable once CheckNamespace
namespace GlobalLitePortalHelper.APIObjs
{

    /// This script was auto-generated from codeZAPP scripting tool version 3.0.135
    /// codeZAPP is fully developed and owned by xPlug Technologies Limited for OFFICIAL use only
    /// Copyright © 2019. xPlug Technologies Limited. All Rights Reserved
    /// This product is covered under the Intellectual Property Laws of Nigeria
    ///*******************************************************************************
    ///* Template Author: Oluwaseyi Adegboyega - sadegboyega@xplugng.com
    ///* Generated 04-11-2019 08:51:12
    ///*******************************************************************************

    public class RegDisbursementObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "ExpenseId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseId is required")]
        public long ExpenseId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Voucher or Reference Number must be between 6 and 30 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Voucher or Reference Number is required")]
        public string VoucherNumber { get; set; }

        [CheckNumber(0, ErrorMessage = "WorkflowItemId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowItemId is required")]
        public long WorkflowItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "ExpenseRequisitionId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public long ExpenseRequisitionId { get; set; }

        //[CheckNumber(0, ErrorMessage ="RequestType is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [CheckNumber(0, ErrorMessage = "ClientId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "ProductId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        //[CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }

        //[CheckNumber(0, ErrorMessage ="BeneficiaryType is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryType is required")]
        public int BeneficiaryType { get; set; }

        [CheckNumber(0, ErrorMessage = "BeneficiaryId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryId is required")]
        public int BeneficiaryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountDisbursed is required")]
        public decimal AmountDisbursed { get; set; }

        //[CheckNumber(0, ErrorMessage ="DisbursementMode is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DisbursementMode is required")]
        public int DisbursementMode { get; set; }

        [CheckNumber(0, ErrorMessage = "DisburesedBy is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DisburesedBy is required")]
        public int DisburesedBy { get; set; }

        [StringLength(50)]
        public string TimeStampDisburesed { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Disbursement Status is required")]
        public int Status { get; set; }



        public string BeneficiaryName { get; set; }
        public decimal AmountApproved { get; set; }
        public decimal Balance { get; set; }
    }

    public class EditDisbursementObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "DisbursementId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DisbursementId is required")]
        public long DisbursementId { get; set; }

        [CheckNumber(0, ErrorMessage = "ExpenseId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseId is required")]
        public long ExpenseId { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Voucher or Reference Number must be between 6 and 30 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Voucher or Reference Number is required")]
        public string VoucherNumber { get; set; }

        [CheckNumber(0, ErrorMessage = "WorkflowItemId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "WorkflowItemId is required")]
        public long WorkflowItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "ExpenseRequisitionId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ExpenseRequisitionId is required")]
        public long ExpenseRequisitionId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RequestType is required")]
        public int RequestType { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [CheckNumber(0, ErrorMessage = "ClientId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "ProductId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        //[CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }
        //[CheckNumber(0, ErrorMessage ="BeneficiaryType is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryType is required")]
        public int BeneficiaryType { get; set; }

        [CheckNumber(0, ErrorMessage = "BeneficiaryId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryId is required")]
        public int BeneficiaryId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountDisbursed is required")]
        public decimal AmountDisbursed { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "DisbursementMode is required")]
        public int DisbursementMode { get; set; }

        [CheckNumber(0, ErrorMessage = "DisburesedBy is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DisburesedBy is required")]
        public int DisburesedBy { get; set; }

        [StringLength(50)]
        public string TimeStampDisburesed { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Status is required")]
        public int Status { get; set; }
    }

    public class DeleteDisbursementObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "DisbursementId is required")]
        public long DisbursementId { get; set; }
    }

    public class DisbursementSearchObj : AdminObj
    {
        public long DisbursementId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string VoucheNumber { get; set; }
    }

    public class DisbursementRegRespObj
    {
        public long DisbursementId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class DisbursementObj
    {
        public string VoucherNumber { get; set; }
        public long DisbursementId { get; set; }
        public long ExpenseId { get; set; }
        public long WorkflowItemId { get; set; }
        public long ExpenseRequisitionId { get; set; }
        public long CurrentStageId { get; set; }
        public string CurrentStageName { get; set; }
        public int RequestType { get; set; }
        public string Title { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int BeneficiaryType { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal AmountDisbursed { get; set; }
        public int DisbursementMode { get; set; }
        public int DisburesedBy { get; set; }
        public string TimeStampDisburesed { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }

        public int AmountApproved { get; set; }

        public string ReguisitionDate { get; set; }
        public string ApprovalDate { get; set; }

    }
    public class DisbursementRespObj
    {
        public List<DisbursementObj> Disbursements { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class VoucherNumber
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int AdminUserId { get; set; }
    }

    public class VoucherNumberSearchObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int AdminUserId { get; set; }
    }


    public class VoucherRequestRespObj  
    {
        public string VoucherNumber { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    //Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
