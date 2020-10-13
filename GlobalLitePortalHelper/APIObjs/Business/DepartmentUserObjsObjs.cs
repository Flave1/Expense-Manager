using System.Collections.Generic;
using System.ComponentModel;
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

    public class RegDepartmentUserObj : AdminObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string UserRoles { get; set; }


        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }
    public class EditDepartmentUserObj : AdminObj
    {
        public int DepartmentUserId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public string UserRoles { get; set; }
        public int Status { get; set; }  
        public bool StatusVal { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
        public string FullName { get; set; }
    }


	public class DeleteDepartmentUserObj : AdminObj
	{
		[CheckNumber(0, ErrorMessage ="DepartmentUserId is required")]
		public int DepartmentUserId { get; set; }
	}


	public class DepartmentUserSearchObj : AdminObj
	{
		public int DepartmentUserId { get; set; }
		public int Status { get; set; }
		
	}


	public class DepartmentUserRegRespObj
	{
		public int DepartmentUserId { get; set; }
		public APIResponseStatus Status  { get; set; }
	}


    public class DepartmentUserObj
    {
        public int DepartmentUserId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int DepartmentId { get; set; }
        public string Email { get; set; }
        public string UserRoles { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public int Status { get; set; }

        public string ClientName { get; set; }
        public string ProductItemName { get; set; }
        public string ProductName { get; set; }
    }

    public class DepartmentUserRespObj
	{
		public List<DepartmentUserObj> DepartmentUsers { get; set; }
        public int CurrentServerVersion { get; set; }

        public APIResponseStatus Status  { get; set; }
	}
     
}
