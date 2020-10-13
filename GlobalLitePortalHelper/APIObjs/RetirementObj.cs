using GlobalLitePortalHelper.APIObjs.Business;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using XPLUG.WEBTOOLKIT;

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

    public class RegRetirementObj : AdminObj
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

        [Required(AllowEmptyStrings = false, ErrorMessage = "AmountRetired is required")]
        public decimal AmountRetired { get; set; }

        //[CheckNumber(0, ErrorMessage ="RetirementMode is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RetirementMode is required")]
        public int RetirementMode { get; set; }

        [CheckNumber(0, ErrorMessage = "RetiredBy is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RetiredBy is required")]
        public int RetiredBy { get; set; }

        [StringLength(50)]
        public string TimeStampRetired { get; set; }

        [Column(TypeName = "varchar")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "RecievedItemIds must be between 1 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RecievedItemIds is required")]
        public string RecievedItemIds { get; set; }

        [CheckNumber(0, ErrorMessage = "DisbursementId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DisbursementId is required")]
        public long DisbursementId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "RetirementType is required")]
        public int RetirementType { get; set; }

        public List<RegReceiptObj> Receipts { get; set; } 
    }

    public class EditRetirementObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "RetirementId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "RetirementId is required")]
        public long RetirementId { get; set; }

    }

    public class DeleteRetirementObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "RetirementId is required")]
        public long RetirementId { get; set; }
    }

    public class RetirementSearchObj : AdminObj
    {
        public long RetirementId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public string VoucheNumber { get; set; }
    }

    public class RetirementRegRespObj
    {
        public long RetirementId { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class RetirementObj
    {
        public long RetirementId { get; set; }

        public long ExpenseId { get; set; }

        public long DisbursementId { get; set; }

        public long WorkflowItemId { get; set; }

        public long ExpenseRequisitionId { get; set; }

        public int RequestType { get; set; }

        public int RetirementType { get; set; }

        public string RecievedItemIds { get; set; }

        public string VoucherNumber { get; set; }

        public int ClientId { get; set; }

        public int ProductId { get; set; }

        public int ProductItemId { get; set; }

        public decimal AmountRetired { get; set; }

        public int RetirementMode { get; set; }

        public int RetiredBy { get; set; }

        public string TimeStampRetired { get; set; }
        public string StatusLabel { get; set; }
        public List<RegReceiptObj> Receipts { get; set; }
    }
    public class RetirementRespObj
    {
        public List<RetirementObj> Retirements { get; set; }
        public APIResponseStatus Status { get; set; }
    }
}
