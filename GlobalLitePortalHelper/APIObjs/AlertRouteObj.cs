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


	public class RegAlertRouteObj : AdminObj
	{
		
        [CheckNumber(0, ErrorMessage = "SMS Route is required")]
        public int SMSRouteId { get; set; }

        [CheckNumber(0, ErrorMessage = "GSM Network is required")]
        public int AlertNetworkId { get; set; }

        [CheckNumber(0, ErrorMessage = "SMS Provider is required")]
        public int SMSProviderId { get; set; }
        
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Service Route Name is too long or too short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Service Route Name is required")]
        public string Name { get; set; }

        public int Status { get; set; }

        public bool StatusVal { get; set; }

    }


	public class EditAlertRouteObj : AdminObj
	{
        [CheckNumber(0, ErrorMessage = "Service Route Id is required")]
        public int AlertRouteId { get; set; }

        [CheckNumber(0, ErrorMessage = "SMS Route is required")]
        public int SMSRouteId { get; set; }

        [CheckNumber(0, ErrorMessage = "GSM Network is required")]
        public int AlertNetworkId { get; set; }

        [CheckNumber(0, ErrorMessage = "SMS Provider is required")]
        public int SMSProviderId { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "Service Route Name is too long or too short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Service Route Name is required")]
        public string Name { get; set; }

        public int Status { get; set; }

    }


	public class DeleteAlertRouteObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertRouteId is required")]
		public int AlertRouteId { get; set; }
	}


	public class AlertRouteSearchObj : AdminObj
	{
		public int AlertRouteId { get; set; }
        public int SMSRouteId { get; set; }
        public int SMSProviderId { get; set; }
        public int AlertNetworkId { get; set; }
        public int Status { get; set; }
		
	}


	public class AlertRouteRegRespObj
	{
		public int AlertRouteId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class AlertRouteObj
	{
		public int AlertRouteId { get; set; }
		public int SMSRouteId { get; set; }
		public string SMSRouteName { get; set; }
		public int AlertNetworkId { get; set; }
		public string AlertNetworkName { get; set; }
		public int SMSProviderId { get; set; }
		public string SMSProviderName { get; set; }
		public string Name { get; set; }
		public int HashCode { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
		public int RegisteredBy { get; set; }
		public string TimeStampRegistered { get; set; }
        public bool StatusVal { get; set; }
    }


	public class AlertRouteRespObj
	{
		public List<AlertRouteObj>  AlertRoutes { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
