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

	public class RegAlertItemObj : AdminObj
	{ 
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Item Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Alert Item Name is too short or too long")]
        public string Name { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Item Short Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Alert Item Short Name must be between 2 and 12 characters")]
        public string ShortName { get; set; }
        public bool StatusVal { get; set; }
        public int Status { get; set; } 
    } 

	public class EditAlertItemObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertItemId is required")]
		public int AlertItemId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Item Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Alert Item Name is too short or too long")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Item Short Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Alert Item Short Name must be between 2 and 12 characters")]
        public string ShortName { get; set; }

        public int Status { get; set; } 
    } 

	public class DeleteAlertItemObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertItemId is required")]
		public int AlertItemId { get; set; }
	} 

	public class AlertItemSearchObj : AdminObj
	{
		public int AlertItemId { get; set; }
		public int Status { get; set; } 
	} 

	public class AlertItemRegRespObj
	{
		public int AlertItemId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}
     
	public class AlertItemObj
	{
		public int AlertItemId { get; set; }
		public string Name { get; set; }
		public string ShortName { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }
        public int RegisteredBy { get; set; }
		public double TimeStampRegistered { get; set; }
	}


	public class AlertItemRespObj
	{
		public List<AlertItemObj>  AlertItems { get; set; }
        public int CurrentServerVersion { get; set; }

        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
