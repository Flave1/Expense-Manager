using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using GlobalLitePortalHelper.APIServices.Business;
using PluglexHelper.CoreService;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class ExpenseManagerController : Controller
    {
        public ActionResult Index(int? clientId, int? productId, int? productItemId)
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
                var ProductItemId = productItemId ?? userClientSession.ProductItemId;

                #endregion

                #region Current User Check
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<ExpenseObj>());
                }
                #endregion

                #region Request and Response Validations
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
                    ViewBag.Error = " Expense list is empty!";
                    return View(new List<ExpenseObj>());
                }

                if (retVal.Expenses == null || !retVal.Expenses.Any())
                {
                    return View(new List<ExpenseObj>());
                }
                #endregion

                #region List Initialzation into session

                var Expenses = retVal.Expenses.OrderByDescending(m => m.ExpenseId).Where(m => m.ClientId == ClientId && m.ProductId == ProductId && m.Status == 1 || m.Status == 2).ToList();
                Session["_ExpenseList_"] = Expenses;

                #endregion

                var clientProdExpenses = Expenses.OrderBy(m => m.ExpenseId).Where(m => m.ClientId == ClientId && m.ProductId == ProductId && m.Status == 1 || m.Status == 2).ToList();

                return View(clientProdExpenses);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<ExpenseObj>());
            }
        }

        #region Retirement


        public ActionResult RetirementModule(int? clientId, int? productId, int? productItemId)
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
                var ProductItemId = productItemId ?? userClientSession.ProductItemId;

                #endregion

                #region Current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {

                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<DisbursementObj>());
                }


                #endregion

                #region Check if list is null else return to view

                if (Session["_currentExpenseDisbursement_"] is List<DisbursementObj> DisbursementList && DisbursementList.Any())
                { 
                    return View(DisbursementList.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList());
                }

                #endregion

                #region Request and Response Validations

                var searchObj = new DisbursementSearchObj
                {
                    AdminUserId = userData.UserId,
                    DisbursementId = 0,
                    StartDate = "",
                    Status = (int)DisbursementStatus.Awaiting_Retirement,
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

                #region Get Amount Approved for each Retired Expense into Session

                var searchObjExp = new ExpenseSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseId = 0,
                    Status = -1000,
                    StartDate = " ",
                    StopDate = " ",
                };
                var retValExp = ExpenseRequisitionServices.LoadExpenses(searchObjExp, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " Expense list is empty!";
                    return View(new List<DisbursementObj>());
                }

                if (retValExp.Expenses == null || !retValExp.Expenses.Any())
                {
                    return View(new List<DisbursementObj>());
                }
                var expenseList = retValExp.Expenses.ToList();

                Session["_ExpenseList_"] = expenseList;
                #endregion

                #region GET Item Names
                var itemSearchObj = new ExpenseItemSearchObj()
                {
                    AdminUserId = userData.UserId,
                    ExpenseItemId = 0,
                    Status = 1
                };


                var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                if (itemRetVal?.Status == null)
                {
                    ViewBag.Error = "Error Occurred please try again later!";
                    return View(new List<DisbursementObj>());
                }
                var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                Session["_globalItems_"] = globalItemList;
                #endregion

                var clientProdExpenseDisbursement = retVal.Disbursements
                        .OrderBy(m => m.DisbursementId)
                        .Where(m => m.ClientId == ClientId && m.ProductId == ProductId)
                        .ToList();

                Session["_currentExpenseDisbursement_"] = clientProdExpenseDisbursement;


                return View(clientProdExpenseDisbursement);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<DisbursementObj>());
            }
        }

        public ActionResult RetirementItems(int expenseRequisitionId)
        {

            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";

                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new List<ExpenseRequisitionObj>());
                }

                #endregion

                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = userClientSession.ClientId;
                var ProductId = userClientSession.ProductId;
                var ProductItemId = userClientSession.ProductItemId;

                #endregion

                #region Request to requisition endpoint to get it's Items

                if (expenseRequisitionId < 1)
                {
                    expenseRequisitionId = 0;
                    return View(new List<ExpenseRequisitionObj>());
                }

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                var ExpenseRequisitionList = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).ToList();


                var expenseRequisitionItems = ExpenseRequisitionList.SingleOrDefault(m => m.ExpenseRequisitionId == expenseRequisitionId);
                if (expenseRequisitionItems == null || expenseRequisitionItems.ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new List<ExpenseRequisitionObj>());
                }
                #endregion

                #region GET ITEM NAMES
                if (Session["_globalItems_"] is List<ExpenseItemObj> GlobItemlist && GlobItemlist.Any())
                {
                    Session["_globalItems_"] = GlobItemlist;
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = -1000,
                        StartDate = "",
                        StopDate = "",
                    };


                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                    if (itemRetVal?.Status == null)
                    {
                        ViewBag.Error = "Error Occurred please try again later!";
                        return View(new List<ExpenseRequisitionObj>());
                    }
                    var globalItemList = itemRetVal.ExpenseItems.ToList();
                    Session["_globalItems_"] = globalItemList;
                }

                #endregion

                #region Generate voucher nuber

                var voucherSouchObj = new VoucherNumberSearchObj
                {
                    AdminUserId = userData.UserId,
                    ClientId = ClientId,
                    ProductId = ProductId
                };
                ViewBag.VoucherNumber = VNServices.GenerateVoucherNumbers(voucherSouchObj).VoucherNumber;
                #endregion

                return View(expenseRequisitionItems);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseRequisitionItemObj());
            }
        }

        public JsonResult ProcessAddRetirementRequest(ExpenseRequisitionObj model, List<HttpPostedFileBase> ImageFile)
        {
            try
            {
                #region current User session check
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                //if (userData.UserId < 1)
                //{
                //    //return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                //    return RedirectToAction("RetirementModule", new { clientId = model.ClientId, productId = model.ProductId, productItemId = model.ProductItemId, msg = "Your session has expired" });
                //}
                #endregion

                #region model Validation


                if (model.ExpenseRequisitionId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Unable to process  item" });
                }

                if (string.IsNullOrEmpty(model.RecievedItemIds))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Recieved Item not found" });
                }

                if (string.IsNullOrEmpty(model.AmountRetired))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Amount Retired" });
                }

                if (string.IsNullOrEmpty(model.AmountSpent))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Amount Spent" });
                }

                if (model.RetirementMode < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Retirement Mode" });
                }

                if (string.IsNullOrEmpty(model.VoucherNumber))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Voucher Number" });
                }
                #endregion

                #region  Get Expense Disbursed from Disbursement Service
                var searchObjForDis = new DisbursementSearchObj
                {
                    AdminUserId = userData.UserId,
                    DisbursementId = 0,
                    StartDate = "",
                    Status = 2,
                    StopDate = "",
                    VoucheNumber = ""
                };
                var retValForDis = DisbursementServices.LoadDisbursements(searchObjForDis, userData.Username);
                if (retValForDis.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Unable to process  item" });
                }

                var disbursementList = retValForDis.Disbursements.OrderBy(x => x.DisbursementId).ToList();
                var expenseDisbursed = disbursementList.SingleOrDefault(m => m.ExpenseRequisitionId == model.ExpenseRequisitionId);
                if (expenseDisbursed == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Unable to process  item" });
                }
                #endregion

                #region Get Expense form Expense Service

                var searchExpObj = new ExpenseSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseId = 0,
                    Status = -1000,
                    StartDate = " ",
                    StopDate = " ",
                };
                var retValExp = ExpenseRequisitionServices.LoadExpenses(searchExpObj, userData.Username);
                var thisExpense = retValExp.Expenses.SingleOrDefault(m => m.ExpenseId == expenseDisbursed.ExpenseId);
                if (thisExpense == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Unable to process  item" });
                }

                #endregion

                #region Build Retirement Request Object

                List<RegReceiptObj> recs = new List<RegReceiptObj>();

                foreach (var file in ImageFile)
                {
                    var rec = new RegReceiptObj()
                    {
                        ExpenseId = expenseDisbursed.ExpenseId,
                        ExpenseRequisitionId = expenseDisbursed.ExpenseRequisitionId,
                        ReceiptDocument = GetBytesFromFile(file),
                        ReceiptNunber = model.RecievedItemIds,
                        ReceiptFileName=  Path.GetFileName(file.FileName),
                        ExpenseRequisitionItemId = model.RecievedItemIds,
                    };
                    recs.Add(rec);
                }


                var requestObj = new RegRetirementObj
                {
                    ClientId = expenseDisbursed.ClientId,
                    ProductId = expenseDisbursed.ProductId,
                    AdminUserId = userData.UserId,
                    ProductItemId = expenseDisbursed.ProductItemId,
                    RequestType = expenseDisbursed.RequestType,
                    Title = expenseDisbursed.Title,
                    BeneficiaryType = expenseDisbursed.BeneficiaryType,
                    AmountRetired = Convert.ToDecimal(model.AmountRetired),
                    BeneficiaryId = expenseDisbursed.BeneficiaryId,
                    ExpenseId = expenseDisbursed.ExpenseId,
                    ExpenseRequisitionId = Convert.ToInt32(model.ExpenseRequisitionId),
                    RetiredBy = userData.UserId,
                    RetirementMode = model.RetirementMode,
                    TimeStampRetired = UtilTools.CurrentTimeStamp(),
                    VoucherNumber = model.VoucherNumber,
                    WorkflowItemId = Convert.ToInt32(expenseDisbursed.WorkflowItemId),
                    DisbursementId = expenseDisbursed.DisbursementId,
                    RecievedItemIds = model.RecievedItemIds,
                    RetirementType = model.RetirementMode,
                    Receipts = recs,
                };
                #endregion

                #region Response and Validations 
                var response = RetirementServices.AddRetirement(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to Process Data" : response.Status.Message.TechnicalMessage });
                }

                #endregion

                #region Request and Response Validations

                var searchObj = new DisbursementSearchObj
                {
                    AdminUserId = userData.UserId,
                    DisbursementId = 0,
                    StartDate = "",
                    Status = (int)DisbursementStatus.Awaiting_Retirement,
                    StopDate = "",
                    VoucheNumber = ""
                };
                var retVal = DisbursementServices.LoadDisbursements(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to Process Data" : response.Status.Message.TechnicalMessage });
                }

                if (retVal.Disbursements == null || !retVal.Disbursements.Any())
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to Process Data" : response.Status.Message.TechnicalMessage });

                }
                Session["_currentExpenseDisbursement_"] = retVal.Disbursements.Where(x => x.ClientId == model.ClientId && x.ProductId == model.ProductId).ToList();
                #endregion


                Session["_ListOfExpReqItems_"] = null;
                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = true, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion

        public static byte[] GetBytesFromFile(HttpPostedFileBase file)
        {
            byte[] imageByte = null;
            if (file != null)
            {
                using (var binaryReader = new BinaryReader(file.InputStream))
                {
                    imageByte = binaryReader.ReadBytes(file.ContentLength);
                }
            }
            return imageByte;
        }
    }
}

