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

    public class RegRequestTypeSettingObj : AdminObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int RequestType { get; set; }
        public double AmountAllowed { get; set; }
        public int AllowedTaskTimeSpan { get; set; }
        public int TimeElapseAction { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public int RequestFrequencyType { get; set; }
    }
    public class EditRequestTypeSettingObj : AdminObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int RequestType { get; set; }
        public double AmountAllowed { get; set; }
        public int AllowedTaskTimeSpan { get; set; }
        public int TimeElapseAction { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public int RequestFrequencyType { get; set; }
        public int RequestTypeSettingId { get; set; }
    }

     

	public class RequestTypeSettingSearchObj : AdminObj
	{
        public int RequestTypeSettingId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }


    public class RequestTypeSetting
    {
        public int RequestTypeSettingId { get; set; }
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public int RequestType { get; set; }
        public int RequestFrequencyType { get; set; }
        public double AmountAllowed { get; set; }
        public int AllowedTaskTimeSpan { get; set; }
        public int TimeElapseAction { get; set; }
        public string TimeStampRegistered { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
    }



    public class RequestTypeSettingRespObj
    {
        public List<RequestTypeSetting> RequestTypeSettings { get; set; }
        public APIResponseStatus Status { get; set; }
    }
    public class RequestTypeSettingRegRespObj
	{
        public List<RequestTypeSetting> RequestTypeSettings { get; set; }
        public APIResponseStatus Status { get; set; }
    }


	//Class File Generated from codeZAPP 3.0.135 | www.xplugng.com | All Rights Reserved.
}
