using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PluglexHelper.CoreService;
using PluglexHelper.PortalManager;
using PluglexHelper.PortalObjs;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class ReportManagerController : Controller
    {
        public ActionResult GetRequisitions(int? clientId, int? productId, int? requestType, int? requestStatus, string start, string end, int? beneficiary, int? item, bool? printExcel)
        {
            try
            {
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = clientId ?? userClientSession.ClientId;
                var ProductId = productId ?? userClientSession.ProductId;
                var ProductItemId = userClientSession.ProductItemId;

                #endregion

                #region Current Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<ExpenseRequisitionObj>());
                }


                #endregion
                 
                DateTime dateTimeStart = Convert.ToDateTime(start);
                DateTime dateTimeEnd = Convert.ToDateTime(end + " 23:59:59 PM"); 
               
                List<ExpenseRequisitionObj> filteredReqs = null;  
                if (Session["_ExpenseRequisitionList_"] is List<ExpenseRequisitionObj> ExpenseRequisition && ExpenseRequisition.Any())
                {

                    #region check if department session is empty else get from service

                    if (Session["_DeptList_"] is List<DepartmentObj> DeptsList && DeptsList.Any())
                    {
                        Session["_DeptList_"] = DeptsList;
                    }
                    else
                    {
                        var searchObj3 = new DepartmentSearchObj
                        {
                            AdminUserId = userData.UserId,
                            DepartmentId = 0,
                            Status = -2,
                            StopDate = "",
                            StartDate = ""
                        };
                        var retValForDepartment = ExpenseLookUpServices.LoadDepartments(searchObj3, userData.Username);
                        var Depts = retValForDepartment.Departments.OrderBy(m => m.DepartmentId).ToList();
                        Session["_DeptList_"] = Depts;
                    }

                    #endregion 

                    if (start != null &&(start.Length > 0 || !string.IsNullOrEmpty(start) || start != "") || end != null &&(end.Length > 0 || !string.IsNullOrEmpty(end) || end != ""))
                    {
                       var b = ExpenseRequisition.Select(m => Convert.ToDateTime(m.TimeStampRegistered.Replace('-', ' ').Trim()));

                        filteredReqs = ExpenseRequisition.Where(m => 
                        Convert.ToDateTime(m.TimeStampRegistered.Replace('-',' ').Trim()) >= dateTimeStart 
                        && Convert.ToDateTime(m.TimeStampRegistered.Replace('-', ' ') .Trim()) <= dateTimeEnd).ToList();
                        List<ExpenseRequisitionObj> filtReqs;

                        filtReqs = filteredReqs.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList();

                        ViewBag.BeneficiaryId = beneficiary;
                        ViewBag.ExpenseItemId = item;
                        ViewBag.GrandTotal = ExpenseRequisition.Where(x => x.ClientId == ClientId && x.ProductId == ProductId).Sum(x => x.TotalAmount);

                        if (requestType != 0)
                        {
                            filteredReqs= filteredReqs.Where(m =>m.RequestType == requestType).ToList();
                        }
                        if (requestStatus >= -100)
                        {
                            filteredReqs = filteredReqs.Where(m => m.Status == requestStatus).ToList();
                        }
                        if (beneficiary > 0 || item > 0)
                        { 
                            var expenseItems = filteredReqs.SelectMany(m => m.ExpenseRequisitionItems).Where(x => x.ExpenseItemId == item && x.BeneficiaryId == beneficiary).ToList();

                            if(!expenseItems.Any())
                            {
                                return View(new List<ExpenseRequisitionObj>());
                            }
                            else
                            {
                                filtReqs = filteredReqs.Where(m => m.ExpenseRequisitionItems.FirstOrDefault().BeneficiaryId == expenseItems.FirstOrDefault().BeneficiaryId
                                    && m.ExpenseRequisitionItems.FirstOrDefault().ExpenseItemId == expenseItems.FirstOrDefault().ExpenseItemId).ToList();
                            } 
                        }


                        if (printExcel ?? false)
                        {
                            return  GenerateExcel(filtReqs);

                        }
                        return View(filtReqs);
                    } 
                }

                #region request and response validations

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = requestStatus ?? -1000,
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);

                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseRequisition list is empty!";
                    return View(new List<ExpenseRequisitionObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  ExpenseRequisition list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<ExpenseRequisitionObj>());
                }
                if (retVal.ExpenseRequisitions == null || !retVal.ExpenseRequisitions.Any())
                {
                    ViewBag.Error = " ExpenseRequisition list is empty!";
                    return View(new List<ExpenseRequisitionObj>());
                }

                #endregion

                var ExpenseRequisitions = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).ToList();//.Where(m => ).ToList();
                Session["_ExpenseRequisitionList_"] =  ExpenseRequisitions.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList();  
                return View(new List<ExpenseRequisitionObj>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< ExpenseRequisitionObj>());
            }
        }
 

        public ActionResult GetDisbursements(int? clientId, int? productId, int? requestType, int? requestStatus, string start, string end)
        {
            try
            {
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = clientId ?? userClientSession.ClientId;
                var ProductId = productId ?? userClientSession.ProductId;
                var ProductItemId = userClientSession.ProductItemId;

                #endregion

                #region Current Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<DisbursementObj>());
                }


                #endregion

                DateTime dateTimeStart = Convert.ToDateTime(start);
                DateTime dateTimeEnd = Convert.ToDateTime(end + " 23:59:59 PM");

                List<DisbursementObj> disbursement = null;
                if (Session["_DisbursementList_"] is List<DisbursementObj> Disbursement && Disbursement.Any())
                {

                    #region check if department session is empty else get from service

                    if (Session["_DepartmentList_"] is List<DepartmentObj> DeptsList && DeptsList.Any())
                    {
                        Session["_DepartmentList_"] = DeptsList;
                    }
                    else
                    {
                        var searchObj3 = new DepartmentSearchObj
                        {
                            AdminUserId = userData.UserId,
                            DepartmentId = 0,
                            Status = -2,
                            StopDate = "",
                            StartDate = ""
                        };
                        var retValForDepartment = ExpenseLookUpServices.LoadDepartments(searchObj3, userData.Username);
                        var Depts = retValForDepartment.Departments.OrderBy(m => m.DepartmentId).ToList();
                        Session["_DepartmentList_"] = Depts;
                    }

                    #endregion 

                    if (start != null && (start.Length > 0 || !string.IsNullOrEmpty(start) || start != "") || end != null && (end.Length > 0 || !string.IsNullOrEmpty(end) || end != ""))
                    { 
                        disbursement = Disbursement.Where(m =>
                        Convert.ToDateTime(m.ApprovalDate.Replace('-', ' ').Trim()) >= dateTimeStart
                        && Convert.ToDateTime(m.ApprovalDate.Replace('-', ' ').Trim()) <= dateTimeEnd).ToList();

                        if (requestType != 0)
                        {
                            disbursement = disbursement.Where(m => m.RequestType == requestType).ToList();
                        }
                        if (requestStatus >= -100)
                        {
                            disbursement = disbursement.Where(m => m.Status == requestStatus).ToList();
                        } 

                        var disbursementList = disbursement.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList();
                        ViewBag.GrandTotal = Disbursement.Where(x => x.ClientId == ClientId && x.ProductId == ProductId).Sum(x => x.AmountDisbursed);
                        ViewBag.PageSum = disbursementList.Sum(x => x.AmountDisbursed);
                        return View(disbursementList);
                    }
                }

                #region request and response validations

                var searchObj = new DisbursementSearchObj
                {
                    AdminUserId = userData.UserId,
                    DisbursementId = 0,
                    Status = requestStatus ?? -1000,
                };
                var retVal = DisbursementServices.LoadDisbursements(searchObj, userData.Username);

                if (retVal?.Status == null)
                {
                    ViewBag.Error = " Disbursement list is empty!";
                    return View(new List<DisbursementObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  Disbursement list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<DisbursementObj>());
                }
                if (retVal.Disbursements == null || !retVal.Disbursements.Any())
                {
                    ViewBag.Error = " Disbursement list is empty!";
                    return View(new List<DisbursementObj>());
                }

                #endregion

                var Disbursements = retVal.Disbursements.OrderBy(m => m.DisbursementId).ToList();//.Where(m => ).ToList();
                Session["_DisbursementList_"] = Disbursements.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList();
                return View(new List<DisbursementObj>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<DisbursementObj>());
            }
        }

        public ActionResult _Receipt(int ExpenseRequisitionId)
        {
            try
            {
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = userClientSession.ClientId;
                var ProductId =  userClientSession.ProductId;
                var ProductItemId = userClientSession.ProductItemId;

                #endregion

                #region Current Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<RegReceiptObj>());
                }


                #endregion
                 
                #region current Id check

                if (ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new List<RegReceiptObj>());
                }

                #endregion

                #region Request and response validations


                var searchObj = new RetirementSearchObj
                {
                    AdminUserId = userData.UserId,
                    RetirementId = 0,
                    StartDate = "",
                    StopDate = "",
                    VoucheNumber = "",
                    Status = -1000
                };
                var retVal = RetirementServices.LoadRetirements(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    return View(new List<RegReceiptObj>());
                }
                if (retVal.Retirements == null || !retVal.Retirements.Any())
                {
                    return View(new List<RegReceiptObj>());
                }
                var ReqReceipt = retVal.Retirements.SelectMany(x => x.Receipts).Where(c=>c.ExpenseRequisitionId == ExpenseRequisitionId);
                if (ReqReceipt == null)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new List<RegReceiptObj>());
                }

                Session["_ReqReceipt_"] = ReqReceipt;

                return View(ReqReceipt);
                #endregion

            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult GenerateExcel(List<ExpenseRequisitionObj> filtReqs)
        {
            var stream = new MemoryStream();

            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sheet1");
                workSheet.Cells.LoadFromCollection(filtReqs, true);
                package.Save(); 
            }
            stream.Position = 0;

            string excelName = $"ExpenseXpat-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}.xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


       
    }


   
}