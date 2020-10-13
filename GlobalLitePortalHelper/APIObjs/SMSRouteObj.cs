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


	public class RegSMSRouteObj : AdminObj
	{
        [StringLength(50, MinimumLength = 3, ErrorMessage = "SMS Route is too long or too short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SMS Route is required")]
        public string Name { get; set; }
        
        [CheckAmount(0, ErrorMessage = "Invalid Route Rate")]
        public decimal Rate { get; set; }

        [CheckNumber(-1, ErrorMessage = "Invalid Bonus Quantity")]
        public int Bonus { get; set; }

        [CheckNumber(-1, ErrorMessage = "Invalid Loyalty Bonus Quantity")]
        public int Loyalty { get; set; }

        [CheckNumber(-1, ErrorMessage = "Invalid App Bonus Quantity")]
        public int AppBonus { get; set; }
        public bool StatusVal { get; set; }
        public int Status { get; set; }

    }


	public class EditSMSRouteObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="SMSRouteId is required")]
        public int SMSRouteId { get; set; }

        [StringLength(50, MinimumLength = 3, ErrorMessage = "SMS Route is too long or too short")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "SMS Route is required")]
        public string Name { get; set; }

        [CheckAmount(0, ErrorMessage = "Invalid Route Rate")]
        public decimal Rate { get; set; }

        [CheckNumber(-1, ErrorMessage = "Invalid Bonus Quantity")]
        public int Bonus { get; set; }

        [CheckNumber(-1, ErrorMessage = "Invalid Loyalty Bonus Quantity")]
        public int Loyalty { get; set; }

        [CheckNumber(-1, ErrorMessage = "Invalid App Bonus Quantity")]
        public int AppBonus { get; set; }

        public int Status { get; set; }

    }


	public class DeleteSMSRouteObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="SMSRouteId is required")]
		public int SMSRouteId { get; set; }
	}


	public class SMSRouteSearchObj : AdminObj
	{
		public int SMSRouteId { get; set; }
		public int Status { get; set; }
		
	}


	public class SMSRouteRegRespObj
	{
		public int SMSRouteId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class SMSRouteObj
	{
		public int SMSRouteId { get; set; }
		public string Name { get; set; }
		public decimal Rate { get; set; }
		public int Bonus { get; set; }
		public int Loyalty { get; set; }
		public int AppBonus { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
		public int RegisteredBy { get; set; }
		public double TimeStampRegistered { get; set; }
        public bool StatusVal { get; set; }

    }


	public class SMSRouteRespObj
	{
		public List<SMSRouteObj>  SMSRoutes { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
