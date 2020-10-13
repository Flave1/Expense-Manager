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

    public class RequisitionStat
    {
        public int Item { get; set; }
        public int Count { get; set; }
        public string ItemLabel { get; set; }
    }

    public class DisbursementStat
    {
        public int Item { get; set; }
        public int Count { get; set; }
        public string ItemLabel { get; set; }
    }

    public class ExpenseStat
    {
        public int Item { get; set; }
        public int Count { get; set; }
        public string ItemLabel { get; set; }
    }

    public class WorkflowTasksStat
    {
        public int Item { get; set; }
        public int Count { get; set; }
        public string ItemLabel { get; set; }
    }

    public class WorkflowStagesStat
    {
        public int Item { get; set; }
        public int Count { get; set; }
        public string ItemLabel { get; set; }
    }
     

    public class AppStatsObj
    {
        public List<RequisitionStat> RequisitionStats { get; set; }
        public List<DisbursementStat> DisbursementStats { get; set; }
        public List<ExpenseStat> ExpenseStats { get; set; }
        public List<WorkflowTasksStat> workflowTasksStats { get; set; }
        public List<WorkflowStagesStat> workflowStagesStats { get; set; }
        public APIResponseStatus Status { get; set; }
    }


    public class AppStatSearchObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int ProductItemId { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
        public int AdminUserId { get; set; }
        public int UserId { get; set; }

    }

    //public class AppStatRespObj
    //{
    //    public List<AppStatsObj> AppStats { get; set; }
    //    public APIResponseStatus Status { get; set; }
    //}
}
