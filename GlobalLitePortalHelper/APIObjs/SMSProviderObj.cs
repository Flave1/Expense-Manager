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
	///* Generated	29-10-2019 06:21:41
	///*******************************************************************************


	public class RegSMSProviderObj : AdminObj
	{
        [StringLength(50, MinimumLength = 1, ErrorMessage = "SMS Provider Name is too long or too short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SMS Provider  Name is required")]
        public string Name { get; set; }
        public bool StatusVal { get; set; }
        public int Status { get; set; }

    }


	public class EditSMSProviderObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="SMS Provider Id is required")]
        public int SMSProviderId { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "SMS Provider Name is too long or too short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SMS Provider  Name is required")]
        public string Name { get; set; }

        public int Status { get; set; }

    }


	public class DeleteSMSProviderObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="SMSProviderId is required")]
		public int SMSProviderId { get; set; }
	}


	public class SMSProviderSearchObj : AdminObj
	{
		public int SMSProviderId { get; set; }
		public int Status { get; set; }
		
	}


	public class SMSProviderRegRespObj
	{
		public int SMSProviderId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class SMSProviderObj
	{
		public int SMSProviderId { get; set; }
		public string Name { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
		public int RegisteredBy { get; set; }
		public string TimeStampRegistered { get; set; }
        public bool StatusVal { get; set; }

    }


	public class SMSProviderRespObj
	{
		public List<SMSProviderObj>  SMSProviders { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
