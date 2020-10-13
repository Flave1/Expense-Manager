using System.Collections.Generic;
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

    public class RegExpenseItemSettingObj : AdminObj
    { 
        public long ExpenseItemSettingId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }
        [CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Category Id is required")]
        public int ExpenseCategoryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Classification Id is required")]
        public int ExpenseClassificationId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Type Id is required")]
        public int ExpenseTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Item Id is required")]
        public int ExpenseItemId { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal UnitPrice { get; set; }

        [CheckNumber(0, ErrorMessage = "Regular Quantity  is required")]
        public int RegularQuantity { get; set; }

        [CheckNumber(0, ErrorMessage = "Prefered Vendor Id is required")]
        public int PreferedVendorId { get; set; }

        [CheckNumber(0, ErrorMessage = "Request Frequency is required")]
        public int RequestFrequency { get; set; }

        public int RequestFrequencyType { get; set; }

        [CheckNumber(0, ErrorMessage = "Registered By Id  is required")]
        public int RegisteredBy { get; set; }  

        public bool IsEnabled { get; set; } 

        public int Status { get; set; }
        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }  

	public class EditExpenseItemSettingObj : AdminObj
	{
        public long ExpenseItemSettingId { get; set; }

        [CheckNumber(0, ErrorMessage = "Client Id is required")]
        public int ClientId { get; set; }

        [CheckNumber(0, ErrorMessage = "Product Id is required")]
        public int ProductId { get; set; }
        [CheckNumber(0, ErrorMessage = "Product Item Id is required")]
        public int ProductItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Category Id is required")]
        public int ExpenseCategoryId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Classification Id is required")]
        public int ExpenseClassificationId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Type Id is required")]
        public int ExpenseTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Expense Item Id is required")]
        public int ExpenseItemId { get; set; }

        [RegularExpression(@"\d+(\.\d{1,2})?", ErrorMessage = "Invalid price")]
        public decimal UnitPrice { get; set; }

        [CheckNumber(0, ErrorMessage = "Regular Quantity  is required")]
        public int RegularQuantity { get; set; }

        [CheckNumber(0, ErrorMessage = "Prefered Vendor Id is required")]
        public int PreferedVendorId { get; set; }

        [CheckNumber(0, ErrorMessage = "Request Frequency is required")]
        public int RequestFrequency { get; set; }

        public int RequestFrequencyType { get; set; }
        [CheckNumber(0, ErrorMessage = "Registered By Id  is required")]
        public int RegisteredBy { get; set; }
        public bool IsEnabled { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }

        public string StatusLabel { get; set; }
    }


	public class DeleteExpenseItemSettingObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="ExpenseItemSettingId is required")]
		public int ExpenseItemSettingId { get; set; }
	}


	public class ExpenseItemSettingSearchObj : AdminObj
	{
		public int ExpenseItemSettingId { get; set; }
		public int Status { get; set; }
		
	}


	public class ExpenseItemSettingRegRespObj
	{
		public int ExpenseItemSettingId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class ExpenseItemSettingObj
	{
        public long ExpenseItemSettingId { get; set; }
         
        public int ClientId { get; set; }
         
        public int ProductId { get; set; } 
        public int ProductItemId { get; set; }
         
        public int ExpenseCategoryId { get; set; }
         
        public int ExpenseClassificationId { get; set; }
         
        public int ExpenseTypeId { get; set; }
         
        public int ExpenseItemId { get; set; }
         
        public decimal UnitPrice { get; set; }
         
        public int RegularQuantity { get; set; }
         
        public int PreferedVendorId { get; set; }
         
        public int RequestFrequency { get; set; }

        public int RequestFrequencyType { get; set; } 
        public int RegisteredBy { get; set; }
        public bool IsEnabled { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }


	public class ExpenseItemSettingRespObj
	{
		public List<ExpenseItemSettingObj>  ExpenseItemSettings { get; set; }
        public int CurrentServerVersion { get; set; }

        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
