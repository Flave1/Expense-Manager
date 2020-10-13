using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using XPLUG.WEBTOOLKIT;

namespace PlugLitePortalHelper.APIObjs
{

    #region Expense Category 
    public class ExpenseCategorySearchObj : AdminObj
    {
        public int ExpenseCategoryId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class ExpenseCategoryObj
    {
        public int ExpenseCategoryId { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }


        public int ProductId { get; set; }
        public int ClientId { get; set; }
        public int ProductItemId { get; set; }
        public string InclusionList { get; set; }
        public string ExclusionList { get; set; }

    }


    public class ExpenseCategoryRespObj
    {
        public List<ExpenseCategoryObj> ExpenseCategories { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    public class FilteredLookupSearchObj
    {
        public int ClientId { get; set; }
        public int ProductId { get; set; }
        public int AdminUserId { get; set; }
    }

    #endregion

    #region Department

    public class DepartmentSearchObj : AdminObj
    {
        public int DepartmentId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class DepartmentObj
    {
        public int DepartmentId { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }
    }

    public class DepartmentRespObj
    {
        public List<DepartmentObj> Departments { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

    #region Expense Classification

    public class ExpenseClassificationSearchObj : AdminObj
    {
        public int ExpenseClassificationId { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }


    public class ExpenseClassificationObj
    {
        public int ExpenseClassificationId { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }
    }


    public class ExpenseClassificationRespObj
    {
        public List<ExpenseClassificationObj> ExpenseClassifications { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

    #region ExpenseItem

    public class ExpenseItemSearchObj : AdminObj
    {
        public int ExpenseItemId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class ExpenseItemObj
    {
        public int ExpenseItemId { get; set; }
        public int ExpenseClassificationId { get; set; }
        public string ExpenseClassificationName { get; set; }
        public int ExpenseCategoryId { get; set; }
        public string ExpenseCategoryName { get; set; }
        public int ExpenseTypeId { get; set; }
        public string Description { get; set; }
        public string ExpenseTypeName { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public string StatusLabel { get; set; }
        public bool StatusVal { get; set; }
    }


    public class ExpenseItemRespObj
    {
        public List<ExpenseItemObj> ExpenseItems { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

    #region Expense Type

    public class ExpenseTypeSearchObj : AdminObj
    {
        public int ExpenseTypeId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class ExpenseTypeObj
    {
        public int ExpenseTypeId { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }
    }

    public class ExpenseTypeRespObj
    {
        public List<ExpenseTypeObj> ExpenseTypes { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

    #region  WorkFlow Approval Level

    public class WorkFlowApprovalLevelSearchObj : AdminObj
    {
        public int WorkFlowApprovalLevelId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class WorkFlowApprovalLevelObj
    {
        public int WorkFlowApprovalLevelId { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public int Rank { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }
    }


    public class WorkFlowApprovalLevelRespObj
    {
        public List<WorkFlowApprovalLevelObj> WorkflowApprovalLevels { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

    #region Work Flow Role

    public class WorkFlowRoleSearchObj : AdminObj
    {
        public int WorkFlowRoleId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class WorkFlowRoleObj
    {
        public int WorkFlowRoleId { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }
    }


    public class WorkFlowRoleRespObj
    {
        public List<WorkFlowRoleObj> WorkflowRoles { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

    #region Work Flow Stage

    public class WorkFlowStageSearchObj : AdminObj
    {
        public int WorkFlowStageId { get; set; }
        public int Status { get; set; }
        public string StartDate { get; set; }
        public string StopDate { get; set; }
    }

    public class WorkFlowStageObj
    {
        public int WorkFlowStageId { get; set; }
        public string Name { get; set; }
        public int HashCode { get; set; }
        public int Status { get; set; }
        public bool StatusVal { get; set; }
        public string StatusLabel { get; set; }
        public int Order { get; set; }
    }


    public class WorkFlowStageRespObj
    {
        public List<WorkFlowStageObj> WorkflowStages { get; set; }
        public APIResponseStatus Status { get; set; }
    }

    #endregion

     
   
}