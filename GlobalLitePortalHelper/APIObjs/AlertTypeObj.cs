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
	///* Generated	29-10-2019 06:21:40
	///*******************************************************************************


	public class RegAlertTypeObj : AdminObj
	{
       
        [CheckNumber(0, ErrorMessage = "Alert Provider is required")]
        public int AlertProviderId { get; set; }
        
    }


	public class EditAlertTypeObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertTypeId is required")]
        public int AlertTypeId { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Provider is required")]
        public int AlertProviderId { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Code is required")]
        public int Code { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Type Title is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Alert Type Title is too short or too long")]
        public string Title { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Service Codes are required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Alert Service Codes is too short or too long")]
        public string ServiceCodes { get; set; }

        public int Status { get; set; }

    }


	public class DeleteAlertTypeObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertTypeId is required")]
		public int AlertTypeId { get; set; }
	}


	public class AlertTypeSearchObj : AdminObj
	{
		public int AlertTypeId { get; set; }
        public int AlertProviderId { get; set; }
        public int Status { get; set; }
	
	}


	public class AlertTypeRegRespObj
	{
		public int AlertTypeId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class AlertTypeObj
	{
		public int AlertTypeId { get; set; }
		public int AlertProviderId { get; set; }
		public string AlertProviderName { get; set; }
		public int Code { get; set; }
		public string Title { get; set; }
		public string ServiceCodes { get; set; }
		public int RegisteredBy { get; set; }
		public string TimeStampRegistered { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }

    }


	public class AlertTypeRespObj
	{
		public List<AlertTypeObj>  AlertTypes { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
