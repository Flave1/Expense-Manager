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
	///* Generated	29-10-2019 06:21:39
	///*******************************************************************************


	public class RegAlertScheduleObj : AdminObj
	{
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Schedule Title is required")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Alert Schedule Title must be between 3 and 80 characters")]
        public string Title { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Item is required")]
        public int AlertItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Provider is required")]
        public int AlertProviderId { get; set; }

        [Range(1, 7, ErrorMessage = "Invalid Day Item")]
        public int Day { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Order is required")]
        public int AlertOrder { get; set; }

        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Display Time is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Invalid Alert Display Time")]
        public string AlertDisplayTime { get; set; }

       
        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert System Time is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Invalid Alert System Time")]
        public string AlertSystemTime { get; set; }

        public int Status { get; set; }

    }


	public class EditAlertScheduleObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertScheduleId is required")]
		public int AlertScheduleId { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Item is required")]
        public int AlertItemId { get; set; }

        [CheckNumber(0, ErrorMessage = "Alert Provider is required")]
        public int AlertProviderId { get; set; }

        [Range(1, 7, ErrorMessage = "Invalid Day Item")]
        public int Day { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Schedule Title is required")]
        [StringLength(80, MinimumLength = 3, ErrorMessage = "Alert Schedule Title must be between 3 and 80 characters")]
        public string Title { get; set; }


        [CheckNumber(0, ErrorMessage = "Alert Order is required")]
        public int AlertOrder { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert Display Time is required")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "Invalid Alert Display Time")]
        public string AlertDisplayTime { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Alert System Time is required")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Invalid Alert System Time")]
        public string AlertSystemTime { get; set; }
        
		public int Status { get; set; }

	}


	public class DeleteAlertScheduleObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertScheduleId is required")]
		public int AlertScheduleId { get; set; }
	}


	public class AlertScheduleSearchObj : AdminObj
	{
		public int AlertScheduleId { get; set; }
        public int AlertItemId { get; set; }
        public int AlertProviderId { get; set; }
        public int Day { get; set; }
        public int Status { get; set; }
		
	}


	public class AlertScheduleRegRespObj
	{
		public int AlertScheduleId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class AlertScheduleObj
	{
		public int AlertScheduleId { get; set; }
		public int AlertItemId { get; set; }
        public string Title { get; set; }
        public string AlertItemName { get; set; }
		public int AlertProviderId { get; set; }
		public string AlertProviderName { get; set; }
		public int DayId { get; set; }
        public string Day { get; set; }
        public int AlertOrder { get; set; }
		public string AlertDisplayTime { get; set; }
		public string AlertSystemTime { get; set; }
		public int RegisteredBy { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
	}


	public class AlertScheduleRespObj
	{
		public List<AlertScheduleObj>  AlertSchedules { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
