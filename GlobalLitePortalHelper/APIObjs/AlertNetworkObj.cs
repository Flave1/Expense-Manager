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


	public class RegAlertNetworkObj : AdminObj
	{
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Network Operator Name must be between 1 and 50 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Network Operator Name is required")]
        public string Name { get; set; }
        
        [StringLength(500, MinimumLength = 3, ErrorMessage = "Operator Code(s) must be between 3 and 500 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Operator Code(s) is (are) required")]
        public string OperatorCodes { get; set; }
        public bool StatusVal { get; set; }
        public int Status { get; set; }
    }


	public class EditAlertNetworkObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertNetworkId is required")]
        public int AlertNetworkId { get; set; }

        [StringLength(50, MinimumLength = 1, ErrorMessage = "Network Operator Name must be between 1 and 50 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Network Operator Name is required")]
        public string Name { get; set; }

        [StringLength(500, MinimumLength = 3, ErrorMessage = "Operator Code(s) must be between 3 and 500 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Operator Code(s) is (are) required")]
        public string OperatorCodes { get; set; }

        public int Status { get; set; }
       

	}


	public class DeleteAlertNetworkObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AlertNetworkId is required")]
		public int AlertNetworkId { get; set; }
	}


	public class AlertNetworkSearchObj : AdminObj
	{
		public int AlertNetworkId { get; set; }
		public int Status { get; set; }
		
	}


	public class AlertNetworkRegRespObj
	{
		public int AlertNetworkId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class AlertNetworkObj
	{
		public int AlertNetworkId { get; set; }
		public string Name { get; set; }
		public string OperatorCodes { get; set; }
		public int RegisteredBy { get; set; }
		public string TimeStampRegistered { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }
    }


	public class AlertNetworkRespObj
	{
		public List<AlertNetworkObj>  AlertNetworks { get; set; }
        public int CurrentServerVersion { get; set; }
        public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
