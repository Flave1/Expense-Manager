using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
    ///* Generated	29-10-2019 06:21:38
    ///*******************************************************************************

    public class RegBeneficiaryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "ClientId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "ProductId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }
        [CheckNumber(0, ErrorMessage = "DepartmentId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DepartmentId is required")]
        public int DepartmentId { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "MiddleName is required")]
        public string MiddleName { get; set; }
         
        public string CompanyName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryCode is required")]
        public string BeneficiaryCode { get; set; }

        [CheckNumber(0, ErrorMessage = "BeneficiaryType is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryType is required")]
        public int BeneficiaryType { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email address is not valid.")]
        [DataType(DataType.EmailAddress)]

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(11)]
        [CheckMobileNumber(ErrorMessage = "Invalid MobileNumber")]
        public string MobileNumber { get; set; }

        public int Status { get; set; }


        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }
    public class EditBeneficiaryObj : AdminObj
    {
        [CheckNumber(0, ErrorMessage = "ClientId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ClientId is required")]
        public int ClientId { get; set; }
        
        [CheckNumber(0, ErrorMessage = "BeneficiaryId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryId is required")]
        public int BeneficiaryId { get; set; }

        [CheckNumber(0, ErrorMessage = "ProductId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "ProductId is required")]
        public int ProductId { get; set; }
        [CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }
        [CheckNumber(0, ErrorMessage = "DepartmentId is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "DepartmentId is required")]
        public int DepartmentId { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "MiddleName is required")]
        public string MiddleName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "CompanyName is required")]
        public string CompanyName { get; set; }

        [StringLength(50)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryCode is required")]
        public string BeneficiaryCode { get; set; }

        [CheckNumber(0, ErrorMessage = "BeneficiaryType is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "BeneficiaryType is required")]
        public int BeneficiaryType { get; set; }

        [Required(ErrorMessage = "* Required")]
        [DisplayName("Email")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "This email address is not valid.")]
        [DataType(DataType.EmailAddress)]

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(11)]
        [CheckMobileNumber(ErrorMessage = "Invalid MobileNumber")]
        public string MobileNumber { get; set; }

        public int Status { get; set; }


        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }


	public class DeleteBeneficiaryObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="BeneficiaryId is required")]
		public int BeneficiaryId { get; set; }
	}


	public class BeneficiarySearchObj : AdminObj
	{
		public int BeneficiaryId { get; set; }
		public int Status { get; set; }
		
	}


	public class BeneficiaryRegRespObj
	{
		public int BeneficiaryId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class BeneficiaryObj
	{ 
        public int ClientId { get; set; }
        public int BeneficiaryId { get; set; }
         
        public int ProductId { get; set; } 
        public int ProductItemId { get; set; } 
        public int DepartmentId { get; set; }
         
        public string FirstName { get; set; }
         
        public string LastName { get; set; }
         
        public string MiddleName { get; set; }
         
        public string CompanyName { get; set; }
         
        public string BeneficiaryCode { get; set; }
         
        public int BeneficiaryType { get; set; }
         
        [DataType(DataType.EmailAddress)]

        [StringLength(50)]
        public string Email { get; set; }

        [StringLength(11)]
        [CheckMobileNumber(ErrorMessage = "Invalid MobileNumber")]
        public string MobileNumber { get; set; }

        public int Status { get; set; }
        public string StatusLabel { get; set; }


        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }


	public class BeneficiaryRespObj
	{
		public List<BeneficiaryObj> Beneficiaries { get; set; }
        public int CurrentServerVersion { get; set; }

        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
