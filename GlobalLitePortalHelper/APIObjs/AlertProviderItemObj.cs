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


	public class RegAlertProviderItemObj : AdminObj
	{
		
		public int AlertProviderId { get; set; }
        public string AlertProviderName { get; set; }

        [CheckNumber(0, ErrorMessage ="AlertItemId is required")]
		public int AlertItemId { get; set; }

        public int Status { get; set; }
        public bool StatusVal { get; set; }

    }


	public class EditAlertProviderItemObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertProviderItemId is required")]
		public int AlertProviderItemId { get; set; }

		[CheckNumber(0, ErrorMessage ="AlertProviderId is required")]
		public int AlertProviderId { get; set; }

		[CheckNumber(0, ErrorMessage ="AlertItemId is required")]
		public int AlertItemId { get; set; }

        public int Status { get; set; }

    }


	public class DeleteAlertProviderItemObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertProviderItemId is required")]
		public int AlertProviderItemId { get; set; }
	}


	public class AlertProviderItemSearchObj : AdminObj
	{
		public int AlertProviderItemId { get; set; }
        public int AlertProviderId { get; set; }
        public int AlertItemId { get; set; }
        public int Status { get; set; }
		
	}


	public class AlertProviderItemRegRespObj
	{
		public int AlertProviderItemId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}

    public class AlertProviderItemInfoObj
    {
        public int AlertProviderId { get; set; }
       
    }
    public class AlertProviderItemObj
	{
		public int AlertProviderItemId { get; set; }
		public int AlertProviderId { get; set; }
		public string AlertProviderName { get; set; }
		public int AlertItemId { get; set; }
		public string AlertItemName { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }
    }


	public class AlertProviderItemRespObj
	{
		public List<AlertProviderItemObj>  AlertProviderItems { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
