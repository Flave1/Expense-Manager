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
	///* Generated	14-10-2019 10:49:16
	///*******************************************************************************


	public class RegAppVersionObj : AdminObj
	{
		
		
        

        [CheckNumber(0, ErrorMessage = "AppCode is required")]
        public int AppCode { get; set; }

       
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Application Name must be between 5 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Application Name is required")]
        public string AppName { get; set; }

        public int HashCode { get; set; }

        [CheckNumber(-1, ErrorMessage = "Android Version is required")]
        public int AndroidVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "IOS Version is required")]
        public int IosVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Web Version is required")]
        public int WebVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Web Version is required")]
        public int WindowsVersion { get; set; }

       
        [StringLength(35, MinimumLength = 7, ErrorMessage = "Date Time Registered is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Time Registered is required")]
        public string TimeStampUpdated { get; set; }

        
        [StringLength(200, MinimumLength = 0, ErrorMessage = "Android Package Name must be between 0 and 200 characters")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Android Package Name is required")]
        public string AndroidPackageName { get; set; }

        
        [StringLength(200, MinimumLength = 0, ErrorMessage = "IOS Package Name must be between 0 and 200 characters")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "IOS Package Name is required")]
        public string IosPackageName { get; set; }

        
        [StringLength(200, MinimumLength = 0, ErrorMessage = "Windows Package Name must be between 0 and 200 characters")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Windows Package Name is required")]
        public string WindowsPackageName { get; set; }

        public int Priority { get; set; }

    }


	public class EditAppVersionObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AppVersionId is required")]
		public int AppVersionId { get; set; }

        [CheckNumber(0, ErrorMessage = "AppCode is required")]
        public int AppCode { get; set; }


        [StringLength(200, MinimumLength = 5, ErrorMessage = "Application Name must be between 5 and 200 characters")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Application Name is required")]
        public string AppName { get; set; }

        public int HashCode { get; set; }

        [CheckNumber(-1, ErrorMessage = "Android Version is required")]
        public int AndroidVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "IOS Version is required")]
        public int IosVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Web Version is required")]
        public int WebVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Web Version is required")]
        public int WindowsVersion { get; set; }


        [StringLength(35, MinimumLength = 7, ErrorMessage = "Date Time Registered is required")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Date Time Registered is required")]
        public string TimeStampUpdated { get; set; }


        [StringLength(200, MinimumLength = 0, ErrorMessage = "Android Package Name must be between 0 and 200 characters")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Android Package Name is required")]
        public string AndroidPackageName { get; set; }


        [StringLength(200, MinimumLength = 0, ErrorMessage = "IOS Package Name must be between 0 and 200 characters")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "IOS Package Name is required")]
        public string IosPackageName { get; set; }


        [StringLength(200, MinimumLength = 0, ErrorMessage = "Windows Package Name must be between 0 and 200 characters")]
        [Required(AllowEmptyStrings = true, ErrorMessage = "Windows Package Name is required")]
        public string WindowsPackageName { get; set; }

        public int Priority { get; set; }
        
		public int Status { get; set; }

	}


	public class DeleteAppVersionObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="AppVersionId is required")]
		public int AppVersionId { get; set; }
	}


	public class AppVersionSearchObj : AdminObj
	{
		public int AppVersionId { get; set; }
		public int Status { get; set; }
		public string StartDate { get; set; }
		public string StopDate { get; set; }
	}


	public class AppVersionRegRespObj
	{
		public int AppVersionId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class AppVersionObj
	{
		public int AppVersionId { get; set; }
		public int AppCode { get; set; }
		public string AppName { get; set; }
		public int HashCode { get; set; }
		public int AndroidVersion { get; set; }
		public int IosVersion { get; set; }
		public int WebVersion { get; set; }
		public int WindowsVersion { get; set; }
		public string TimeStampUpdated { get; set; }
		public string AndroidPackageName { get; set; }
		public string IosPackageName { get; set; }
		public string WindowsPackageName { get; set; }
		public int Priority { get; set; }
		public int Status { get; set; }
		public string StatusLabel { get; set; }
	}

    public class AppCurrentVersionRespObj
    {
        public AppVersionObj AppVersion;
        public APIResponseStatus Status;
    }
    public class AppVersionRespObj
	{
		public List<AppVersionObj>  AppVersions { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
