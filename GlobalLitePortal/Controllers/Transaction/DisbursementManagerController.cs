using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using GlobalLitePortalHelper.APIServices.Business;
using PluglexHelper.CoreService; 
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class DisbursementManagerController : Controller
    {
        public ActionResult Index(int? clientId, int? productId, int? productItemId)
        {
            try
            { 
                ViewBag.Error = ""; 

                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = clientId ?? userClientSession.ClientId;
                var ProductId = productId ?? userClientSession.ProductId;
                var ProductItemId = productItemId ?? userClientSession.ProductItemId;

                #endregion

                #region Current User session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<DisbursementObj>());
                }

                #endregion

                #region check if Disbursement session list is not empty return to view
                 if (Session["_currentExpenseDisbursementL_"] is List<DisbursementObj> ListOFDisbursements && ListOFDisbursements.Any())
                {
                    return View(ListOFDisbursements);
                }
                #endregion
                
                #region Response and Request validations 

                var searchObj = new DisbursementSearchObj
                {
                    AdminUserId = userData.UserId,
                    DisbursementId = 0,
                    StartDate = "",
                    Status = -1000,
                    StopDate = "",
                    VoucheNumber = ""
                };
                var retVal = DisbursementServices.LoadDisbursements(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return View(new List<DisbursementObj>());
                }

                if (retVal.Disbursements == null || !retVal.Disbursements.Any())
                {
                    ViewBag.Error = " DisbursementObj list is empty!";
                    return View(new List<DisbursementObj>());
                }


                #endregion

                var currentExpenseDisbursement = retVal.Disbursements.OrderBy(m => m.DisbursementId).Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList(); 
                 
                Session["_currentExpenseDisbursementL_"] = currentExpenseDisbursement;
                return View(currentExpenseDisbursement);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< DisbursementObj>());
            }
        }

        #region Add DisbursementObj
        public ActionResult AddDisbursement(int clientId, int productId, int productItemId,int ExpenseId, string BeneficiaryName)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegDisbursementObj());
                }

                if(ExpenseId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegDisbursementObj());
                }

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
              
                var expenseSearchObj = new ExpenseSearchObj
                {
                    ExpenseId = ExpenseId,
                    AdminUserId = userData.UserId,
                    StartDate = "",
                    Status = -1000,
                    StopDate = ""
                };


            
                var expensesResponse = ExpenseRequisitionServices.LoadExpenses(expenseSearchObj, userData.Username);
                var expenseToDisburse = expensesResponse.Expenses.Find(m => m.ExpenseId == ExpenseId);
 
                if (expenseToDisburse == null)
                { 
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return RedirectToAction("Login","Portal"); 
                }

                
                var expensesReqResponse = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                var expenseReqToDisburse = expensesReqResponse.ExpenseRequisitions.FirstOrDefault(m => m.ExpenseRequisitionId == expenseToDisburse.ExpenseRequisitionId).ExpenseRequisitionItems.Find(m => m.ExpenseRequisitionId == expenseToDisburse.ExpenseRequisitionId);

                #region Generate voucher nuber

                var voucherSouchObj = new VoucherNumberSearchObj
                {
                    AdminUserId = userData.UserId,
                    ClientId = expenseToDisburse.ClientId,
                    ProductId = expenseToDisburse.ProductId
                };
                ViewBag.VoucherNumber = VNServices.GenerateVoucherNumbers(voucherSouchObj).VoucherNumber;
                #endregion

                return View(new RegDisbursementObj {
                    ProductId = expenseToDisburse.ProductId, 
                    ClientId = expenseToDisburse.ClientId, 
                    ProductItemId = expenseToDisburse.ProductItemId,
                    ExpenseId = Convert.ToInt32(expenseToDisburse.ExpenseId),  
                    AmountDisbursed = Convert.ToInt32(expenseToDisburse.AmountDisbursed),
                    Title = expenseToDisburse.Title,
                    WorkflowItemId = Convert.ToInt32(expenseToDisburse.WorkflowItemId), 
                    AmountApproved = expenseToDisburse.AmountApproved,
                    RequestType = expenseToDisburse.RequestType,
                    ExpenseRequisitionId = Convert.ToInt32(expenseToDisburse.ExpenseRequisitionId),
                    Balance = expenseToDisburse.Balance, 
                    BeneficiaryId = expenseReqToDisburse.BeneficiaryId
                });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegDisbursementObj());
            }
        }
        public JsonResult ProcessAddDisbursementRequest(RegDisbursementObj model)
        {
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #region model validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "client required " });
                }


                if (model.VoucherNumber.Length < 6 || model.VoucherNumber.Length > 30)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Voucher Number require a minimium length of 6 characters and a maximium length 30 Characters " });
                }
                if (model.VoucherNumber.Length < 1 || string.IsNullOrEmpty(model.VoucherNumber))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Voucher Number required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product required " });
                }
                if (model.AmountDisbursed < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "AmountDisbursed required " });
                }
                if (model.DisbursementMode < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Disbursement Mode required " });
                }
                if (model.ExpenseId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Expense  required " });
                }
                if (model.ExpenseRequisitionId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseRequisitionId required " });
                }
                if (string.IsNullOrEmpty(model.Title) || model.Title.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = " Title required" });
                }

                #endregion

                var previousDisbursementList = (List<DisbursementObj>)Session["_DisbursementList_"];
         

                var requestObj = new RegDisbursementObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,//model.ProductItemId,
                    BeneficiaryId = model.BeneficiaryId,
                    BeneficiaryType = model.BeneficiaryType,
                    RequestType = model.RequestType,
                    AmountDisbursed = model.AmountDisbursed,
                    DisburesedBy = userData.UserId,
                    ExpenseRequisitionId = model.ExpenseRequisitionId,
                    DisbursementMode = model.DisbursementMode,
                    TimeStampDisburesed = DateTime.Now.ToString("D"),
                    Title = model.Title,
                    WorkflowItemId = model.WorkflowItemId,
                    ExpenseId = model.ExpenseId,
                    VoucherNumber = model.VoucherNumber
                };

                var response = DisbursementServices.AddDisbursement(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }

                #region List Initialzation into session
                var searchObj = new ExpenseSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseId = 0,
                    Status = -1000,
                    StartDate = " ",
                    StopDate = " ",
                };
                var retVal = ExpenseRequisitionServices.LoadExpenses(searchObj, userData.Username);

                if (retVal?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (retVal.Expenses == null || !retVal.Expenses.Any())
                {
                    ViewBag.Error = " DisbursementObj list is empty!";
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }




                var Expenses = retVal.Expenses.OrderByDescending(m => m.ExpenseId).Where(m => m.ClientId == model.ClientId && m.ProductId == model.ProductId && m.Status == 1 || m.Status == 2).ToList();
                Session["_ExpenseList_"] = Expenses;

                #endregion

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = true, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion 
    }
}