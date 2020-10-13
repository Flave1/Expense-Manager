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


	public class RegLookupVersionObj : AdminObj
	{
        [CheckNumber(0, ErrorMessage = "Application Version is required")]
        public int AppVersionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Application Code is required")]
        public int AppCode { get; set; }
        public int LookupItem { get; set; }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Item name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Item name must be between 2 and 100 characters")]
        public string Name { get; set; }

        public int HashCode { get; set; }

        [CheckNumber(-1, ErrorMessage = "Android Version is required")]
        public int AndroidVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "IOS Version is required")]
        public int IosVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Web Version is required")]
        public int WebVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Windows Version is required")]
        public int WindowsVersion { get; set; }

    }


	public class EditLookupVersionObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="LookupVersionId is required")]
        public int LookupVersionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Application Version is required")]
        public int AppVersionId { get; set; }

        [CheckNumber(0, ErrorMessage = "Application Code is required")]
        public int AppCode { get; set; }
        public int LookupItem { get; set; }


        [Required(AllowEmptyStrings = false, ErrorMessage = "Item name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Item name must be between 2 and 100 characters")]
        public string Name { get; set; }

        public int HashCode { get; set; }

        [CheckNumber(-1, ErrorMessage = "Android Version is required")]
        public int AndroidVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "IOS Version is required")]
        public int IosVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Web Version is required")]
        public int WebVersion { get; set; }

        [CheckNumber(-1, ErrorMessage = "Windows Version is required")]
        public int WindowsVersion { get; set; }

    }


	public class DeleteLookupVersionObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="LookupVersionId is required")]
		public int LookupVersionId { get; set; }
	}


	public class LookupVersionSearchObj : AdminObj
	{
		public int LookupVersionId { get; set; }
		public int Status { get; set; }
	
	}

    public class VersionRequestObj
    {
        [CheckNumber(0, ErrorMessage = "App Version Id is required")]
        public int AppVersionId { get; set; }
    }

    public class LookupVersionRegRespObj
	{
		public int LookupVersionId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


	public class LookupVersionObj
	{
		public int LookupVersionId { get; set; }
		public int AppVersionId { get; set; }
		public int AppCode { get; set; }
		public int LookupItem { get; set; }
		public string Name { get; set; }
		public int HashCode { get; set; }
		public int AndroidVersion { get; set; }
		public int IosVersion { get; set; }
		public int WebVersion { get; set; }
		public int WindowsVersion { get; set; }
	}

   
    public class LookupVersionRespObj
	{
		public List<LookupVersionObj>  LookupVersions { get; set; }
		public APIResponseStatus Status  { get; set; }
	}

    public class LookupStatObj
    {
        public int Count { get; set; }
        public int LookupItem { get; set; }
    }

    public class LookupStatRespObj
    {
        public List<LookupStatObj> LookupStats { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    //Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
