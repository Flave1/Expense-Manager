using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using PluglexHelper.CoreService;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class ExpenseRequisitionManagerController : Controller
    {
        public ActionResult Index(int? clientId, int? productId)
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

                #region Get Departments Name

                var searchObjfroDept = new DepartmentSearchObj
                {
                    AdminUserId = userData.UserId,
                    DepartmentId = 0,
                    Status = -2,
                    StopDate = "",
                    StartDate = ""
                };
                var retValfroDept = ExpenseLookUpServices.LoadDepartments(searchObjfroDept, userData.Username);
                if (retValfroDept?.Status == null)
                {
                    ViewBag.Error = "Error Occurred Please try again later!";
                    return View(new List<ExpenseRequisitionObj>());
                }

                var DeptList = retValfroDept.Departments.OrderBy(x => x.DepartmentId).ToList();

                Session["_DeptList_"] = DeptList;

                #endregion

                #region Expense Requisition request and response validations

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseRequisitionObj list is empty!";
                    return View(new List<ExpenseRequisitionObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  ExpenseRequisitionObj list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<ExpenseRequisitionObj>());
                }
                if (retVal.ExpenseRequisitions == null || !retVal.ExpenseRequisitions.Any())
                {
                    ViewBag.Error = " ExpenseRequisitionObj list is empty!";
                    return View(new List<ExpenseRequisitionObj>());
                }

                #endregion

                #region Expense Requisition initialization into session

                var currentUserRequisitions = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).Where(m => m.RequesterId == userData.UserId).ToList();
                Session["_ExpenseRequisitionList_"] = currentUserRequisitions.Where(m => m.ClientId == ClientId && m.ProductId == ProductId).ToList();


                #endregion

                var currentUserRequisition = currentUserRequisitions.OrderBy(m => m.ExpenseRequisitionId)
                                            .Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList();

               
                return View(currentUserRequisition);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<ExpenseRequisitionObj>());
            }
        }

        #region Add ExpenseRequisitionItem
        public ActionResult _AddExpenseRequisitionItem(int clientId, int productId)
        {
            try
            {  
               
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                } 

                #endregion

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegExpenseRequisitionItemObj());
                }
               
                return View(new RegExpenseRequisitionItemObj { ClientId = clientId, ProductId = productId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseRequisitionItemObj());
            }
        }
        public JsonResult ProcessAddExpenseRequisitionItemRequest(RegExpenseRequisitionItemObj model)
        {
            try
            {
                #region Current user check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region Model Validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product required " });
                }
                if (model.Quantity < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Quantity required " });
                }
                if (model.UnitPrice < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "UnitPrice required" });
                }
                if (model.TotalAmount < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "TotalAmount required " });
                }
                if (model.BeneficiaryId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Beneficiary required " });
                }
                if (model.BeneficiaryType < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "BeneficiaryType required " });
                }

                if (model.ExpenseCategoryId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseCategory required " });
                }
                if (model.ExpenseClassificationId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseClassification required " });
                }
                if (model.ExpenseItemSettingId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Item Configuration not Found " });
                }
                if (string.IsNullOrEmpty(model.Description))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Description required " });
                }
                if (model.ExpenseTypeId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseType required " });
                }
                if (string.IsNullOrEmpty(model.GeneralRemark))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "GeneralRemark required " });
                }
                var ListOfExpReqItems = Session["_ListOfExpReqItems_"] as List<ExpenseRequisitionItemObj>;
                if (ListOfExpReqItems == null)
                {
                    ListOfExpReqItems = new List<ExpenseRequisitionItemObj>();
                }

                #endregion

                #region Add item to Requisition if item does not Exist in the same Requisition

                ExpenseRequisitionItemObj items = null;
                if (ListOfExpReqItems != null)
                {
                    items = ListOfExpReqItems.FirstOrDefault(x => x.ExpenseCategoryId == model.ExpenseCategoryId
                                                                    && x.BeneficiaryId == model.BeneficiaryId
                                                                    && x.ExpenseItemId == model.ExpenseItemId
                                                                    && x.ExpenseClassificationId == model.ExpenseClassificationId
                                                                    && x.ExpenseItemSettingId == model.ExpenseItemSettingId
                                                                    && x.ExpenseRequisitionId == model.ExpenseRequisitionId);
                }

              
                if (items == null)
                {
                    int lastItem(List<ExpenseRequisitionItemObj> reqItems)
                    {
                        if (reqItems == null)
                        {
                            return 0;
                        }
                        return Convert.ToInt16(reqItems.Select(x => x.UniqueId).LastOrDefault());
                    }

                    model.UniqueId = lastItem(ListOfExpReqItems) + 1;
                    ListOfExpReqItems.Add(new ExpenseRequisitionItemObj
                    {
                        ExpenseCategoryId = model.ExpenseCategoryId,
                        ExpenseClassificationId = model.ExpenseClassificationId,
                        BeneficiaryId = model.BeneficiaryId,
                        ExpenseTypeId = model.ExpenseTypeId,
                        ProductItemId = model.ProductItemId,
                        Status = (int)RequestItemStatus.Registered,
                        ClientId = model.ClientId,
                        ProductId = model.ProductId,
                        ExpenseItemId = model.ExpenseItemId,
                        ExpenseItemSettingId = model.ExpenseItemSettingId,
                        BeneficiaryType = model.BeneficiaryType,
                        UnitPrice = model.UnitPrice,
                        TotalAmount = model.TotalAmount,
                        Quantity = model.Quantity,
                        Description = model.Description,
                        GeneralRemark = model.GeneralRemark,
                        UniqueId = model.UniqueId,
                    });

                    Session["_ListOfExpReqItems_"] = ListOfExpReqItems;
                }
                else
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Item Already Added to requisition" });
                }
                #endregion

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }

        public PartialViewResult _ExpenseRequisitionItemsList(int? id)
        {
            try
            {
                ViewBag.Error = "";
                #region Current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);

                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return PartialView(new List<RegExpenseRequisitionObj>());
                }

                #endregion

                #region Get Item Names

                if(Session["_globalItems_"] is List<ExpenseItemObj> GlobalExpenseItems && GlobalExpenseItems.Any())
                {
                    Session["_globalItems_"] = GlobalExpenseItems.ToList();
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj()
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = 1
                    };

                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);

                    if (itemRetVal?.Status == null)
                    {
                        ViewBag.Error = "Error Occurred Please Try Again Later";
                        return PartialView(new List<RegExpenseRequisitionObj>());
                    }
                    var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                    Session["_globalItems_"] = globalItemList;
                }


                #endregion

                #region Deletes Item Added to ExpenseRequisition Item List Session 

                if (Session["_ListOfExpReqItems_"] is List<ExpenseRequisitionItemObj> itemList && itemList.Any())
                {
                    if (id > 0)
                    {
                        var itemToDelete = itemList.FirstOrDefault(i => i.UniqueId == id || i.ExpenseRequisitionItemId == id);
                        itemList.Remove(itemToDelete);
                        Session["_ExpenseRequisitionItemList_"] = itemList;
                        return PartialView(new List<RegExpenseRequisitionObj>());
                    }

                    Session["_ExpenseRequisitionItemList_"] = new List<ExpenseRequisitionItemObj>();
                    return PartialView(new List<RegExpenseRequisitionObj>());
                }

                #endregion
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return PartialView(new List<RegExpenseRequisitionObj>());
            }
            return PartialView(new List<RegExpenseRequisitionObj>());
        }

        #endregion

        #region Edit ExpenseRequisitionItem
        public ActionResult _EditExpenseRequisitionItem(int? ExpenseRequisitionId, int? ExpenseRequisitionItemId, int? uniqueId)
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
                    return View(new RegExpenseRequisitionItemObj());
                }

                #endregion

                #region In Memory Unique Id to delete ExpenseRequisition Item from session if it has no being Added to Database 

                if (uniqueId > 0)
                {
                    var ListOfExpReqItems = Session["_ListOfExpReqItems_"] as List<ExpenseRequisitionItemObj>;
                    if (ListOfExpReqItems == null)
                    {
                        ListOfExpReqItems = new List<ExpenseRequisitionItemObj>();
                    }
                    var itemToEdit = ListOfExpReqItems.FirstOrDefault(m => m.UniqueId == uniqueId);
                    return View(itemToEdit);
                }

                #endregion

                #region RequisitionID and ItemID check

                if (ExpenseRequisitionId < 1)
                {
                    ExpenseRequisitionId = 0;
                    return View(new RegExpenseRequisitionItemObj());
                }

                if (ExpenseRequisitionItemId < 1)
                {
                    ExpenseRequisitionId = 0;
                    return View(new RegExpenseRequisitionItemObj());
                }

                #endregion

                #region Request and response validation

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                var ExpenseRequisitionList = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).ToList();

                var expenseRequisitionObj = ExpenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);
                if (expenseRequisitionObj == null || expenseRequisitionObj.ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item \n Try Adding a new Item";
                    return View(new ExpenseRequisitionItemObj());
                }
                var thisReqItem = expenseRequisitionObj.ExpenseRequisitionItems.Find(m => m.ExpenseRequisitionItemId == ExpenseRequisitionItemId);

                if (thisReqItem == null || thisReqItem.ExpenseRequisitionItemId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RegExpenseRequisitionItemObj());
                }

                #endregion

                return View(thisReqItem);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseRequisitionItemObj());
            }
        }

        public JsonResult ProcessEditExpenseRequisitionItemRequest(ExpenseRequisitionItemObj model)
        {
            try
            {
                #region Current User seeion check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region Model Validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product required " });
                }
                if (model.Quantity < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Quantity required " });
                }
                if (model.UnitPrice < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "UnitPrice required" });
                }
                if (model.TotalAmount < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "TotalAmount required " });
                }
                if (model.BeneficiaryId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Beneficiary required " });
                }
                if (model.BeneficiaryType < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "BeneficiaryType required " });
                }

                if (model.ExpenseCategoryId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseCategory required " });
                }
                if (model.ExpenseClassificationId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseClassification required " });
                }
                if (model.ExpenseItemSettingId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Item Configuration not Found " });
                }
                if (string.IsNullOrEmpty(model.Description))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Description required " });
                }
                if (model.ExpenseTypeId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ExpenseType required " });
                }
                if (string.IsNullOrEmpty(model.GeneralRemark))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "GeneralRemark required " });
                }

                #endregion

                #region Check if Items Session is null

                ExpenseRequisitionItemObj itemToReplace = new ExpenseRequisitionItemObj();
                List<ExpenseRequisitionItemObj> ListOfExpReqItems;
                if (Session["_ListOfExpReqItems_"] == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                ListOfExpReqItems = (List<ExpenseRequisitionItemObj>)Session["_ListOfExpReqItems_"];
                #endregion

                #region Edits an Item using in Memory Unique Id 

                if (model.UniqueId > 0)
                {
                    itemToReplace = ListOfExpReqItems.SingleOrDefault(m => m.UniqueId == model.UniqueId);
                }

                #endregion

                #region Edits an Item using Database ExpenseRequisitionItem Id

                if (model.ExpenseRequisitionItemId > 0)
                {
                    itemToReplace = ListOfExpReqItems.SingleOrDefault(m => m.ExpenseRequisitionItemId == model.ExpenseRequisitionItemId);
                }


                #endregion

                ListOfExpReqItems.Remove(itemToReplace);

                #region Creates in memory Unique Id 

                int lastItem(List<ExpenseRequisitionItemObj> reqItems)
                {
                    var rtNo = reqItems.Select(x => x.UniqueId).LastOrDefault();
                    return Convert.ToInt32(rtNo);
                }

                model.UniqueId = lastItem(ListOfExpReqItems) + 1;

                #endregion

                #region Builds Expense Requisition Item Object 

                ListOfExpReqItems.Add(new ExpenseRequisitionItemObj
                {
                    ExpenseCategoryId = model.ExpenseCategoryId,
                    ExpenseClassificationId = model.ExpenseClassificationId,
                    BeneficiaryId = model.BeneficiaryId,
                    ExpenseTypeId = model.ExpenseTypeId,
                    ProductItemId = model.ProductItemId,
                    Status = (int)RequestItemStatus.Workflow_Processing,
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    ExpenseItemId = model.ExpenseItemId,
                    ExpenseItemSettingId = model.ExpenseItemSettingId,
                    BeneficiaryType = model.BeneficiaryType,
                    UnitPrice = model.UnitPrice,
                    TotalAmount = model.TotalAmount,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    GeneralRemark = model.GeneralRemark,
                    UniqueId = model.UniqueId,
                    ExpenseRequisitionId = model.ExpenseRequisitionId,
                    ExpenseRequisitionItemId = model.ExpenseRequisitionItemId,
                });

                #endregion

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });

            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }

        #endregion

        #region Add ExpenseRequisition
        public ActionResult AddExpenseRequisition(int clientId, int productId)
        {
            try
            {
                Session["_ListOfExpReqItems_"] = null;
                ViewBag.Error = "";
                ViewBag.SessionError = "";

                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                } 

                #endregion


                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegExpenseRequisitionObj());
                }
                 
              
                return View(new RegExpenseRequisitionObj { ProductId = productId, ClientId = clientId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseRequisitionObj());
            }
        }
        public JsonResult ProcessAddExpenseRequisitionRequest(RegExpenseRequisitionObj model)
        {
            try
            {
                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region model validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product required " });

                }
                if (model.BeneficiaryType < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "BeneficiaryType required " });

                }
                if (model.DepartmentId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Department required " });

                }
                if (string.IsNullOrEmpty(model.GeneralRemark))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "GeneralRemark required " });

                }
                if (string.IsNullOrEmpty(model.Title))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Title required " });

                }
                if (model.TotalAmount < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Total Amount required " });

                }
                if(model.Title.Length < 5)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Minimium length  for 5 characters required on Title" });
                }

                #endregion

                #region Get Client Product Workflow Stages
                 
                var searchObjForWFS = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = model.ProductId,
                    ClientId = model.ClientId,

                }; 

                var retValforWFS = ExpenseLookUpServices.LoadFilteredWorkFlowStages(searchObjForWFS, userData.Username);
                if (retValforWFS?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Work Flow Stages Not Configured For This Client " });
                } 
                var workflowInitiation = retValforWFS.WorkflowStages.FirstOrDefault(x => x.Name.Trim() == WorkflowStage.Initiation.ToString());


                #endregion

                #region Check if current Item Session List is null

                var ListOfExpReqItems = (List<ExpenseRequisitionItemObj>)Session["_ListOfExpReqItems_"];
                if (ListOfExpReqItems == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }
                 
               

                #endregion

                #region Build Request Object

                var requestObj = new RegExpenseRequisitionObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    DepartmentId = model.DepartmentId,
                    Status = (int)RequestStatus.Registered,
                    GeneralRemark = model.GeneralRemark,
                    RegisteredBy = userData.UserId,
                    RequestNumber = "EXP" + DateTime.Now.Day + DateTime.Now.Minute + DateTime.Now.Second + "XPAT",
                    RequestType = model.RequestType,
                    RequesterId = userData.UserId,
                    TimeStampRegistered = DateTime.Now.ToString("D"),
                    Title = model.Title,
                    TotalAmount = model.TotalAmount,
                    WorkflowStageId = workflowInitiation.WorkFlowStageId,
                    ExpenseRequisitionItems = ListOfExpReqItems,
                    BeneficiaryType = model.BeneficiaryType,
                };

                #endregion

                #region Request and Response Validations

                var response = ExpenseLookupServices.AddExpenseRequisition(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to Process Data" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -2
                };

                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseRequisitions != null)
                {
                    var ExpenseRequisitions = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).ToList();
                    Session["_ExpenseRequisitionList_"] = ExpenseRequisitions;
                }

                #endregion
                  
                Session["_ListOfExpReqItems_"] = null;
                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }
        #endregion


        #region ExpenseRequisitionObj Detail
        public ActionResult _ExpenseRequisitionDetail(int ExpenseRequisitionId)
        {

            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";

                #region Client Product productItem Session Check

                var userClientSession = (AppSession)Session["_UserClientSession_"];
                if (userClientSession == null || userClientSession.ClientId < 1 || userClientSession.ProductId < 1 || userClientSession.ProductItemId < 1)
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                var ClientId = userClientSession.ClientId;
                var ProductId =   userClientSession.ProductId;
                var ProductItemId =   userClientSession.ProductItemId;

                #endregion

                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseRequisitionObj());
                }

                #endregion

                #region ExpenseRequisitionId Check

                  if (ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new ExpenseRequisitionObj());
                }
                #endregion

                #region Check if Expense Item List is null else Fetch from Expense Item Service

                if (Session["_globalItems_"] is List<ExpenseItemObj> GlobalExpenseItems && GlobalExpenseItems.Any())
                {
                    Session["_globalItems_"] = GlobalExpenseItems.ToList();
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj()
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = 1
                    };
                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                    var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                    Session["_globalItems_"] = globalItemList;
                }

                #endregion

                #region Check if ExpenseRequisition Session List if null 

                if(Session["_ExpenseRequisitionList_"] is List<ExpenseRequisitionObj> ExpenseRequisitionList && ExpenseRequisitionList.Any())
                {
                    var selectedExpRQFrmSession = ExpenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);

                    if (selectedExpRQFrmSession == null || selectedExpRQFrmSession.ExpenseRequisitionId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ExpenseRequisitionObj());
                    }
                    Session["_ListOfExpReqItems_"] = selectedExpRQFrmSession.ExpenseRequisitionItems.ToList();
                    return View(selectedExpRQFrmSession);
                }

                #endregion

                #region Request and Response Validations

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseRequisitionObj list is empty!";
                    return View(new List<ExpenseRequisitionObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  ExpenseRequisitionObj list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<ExpenseRequisitionObj>());
                }
                if (retVal.ExpenseRequisitions == null || !retVal.ExpenseRequisitions.Any())
                {
                    ViewBag.Error = " ExpenseRequisitionObj list is empty!";
                    return View(new List<ExpenseRequisitionObj>());
                }


                #endregion

                #region Initialization of Requisitions and requisition items into sessions

                var SelectedExpenseRequisition = retVal.ExpenseRequisitions.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);

                Session["_ExpenseRequisitionList_"] = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId)
                                .Where(m => m.ClientId == ClientId && m.ProductItemId == ProductItemId && m.ProductId ==ProductId).ToList();

                Session["_ListOfExpReqItems_"] = SelectedExpenseRequisition.ExpenseRequisitionItems.ToList();

                if (SelectedExpenseRequisition == null || SelectedExpenseRequisition.ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseRequisitionObj());
                }
                Session["_CurrentSelExpenseRequisition_"] = SelectedExpenseRequisition;

                #endregion
                return View(SelectedExpenseRequisition);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseRequisitionObj());
            }
        }
        #endregion

        #region Edit ExpenseRequisition

        public ActionResult _EditExpenseRequisition(int ExpenseRequisitionId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";

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

                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseRequisitionObj());
                }

                #endregion

                #region ExpenseRequisitionId Check
                if (ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new ExpenseRequisitionObj());
                }
                #endregion

                #region Check if Expense Item List is null else Fetch from Expense Item Service

                if (Session["_globalItems_"] is List<ExpenseItemObj> GlobalExpenseItems && GlobalExpenseItems.Any())
                {
                    Session["_globalItems_"] = GlobalExpenseItems.ToList();
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj()
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = 1
                    };
                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                    var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                    Session["_globalItems_"] = globalItemList;
                }

                #endregion
                
                #region Check if ExpenseRequisition Session List if null 

                if (Session["_ExpenseRequisitionList_"] is List<ExpenseRequisitionObj> ExpenseRequisitionList && ExpenseRequisitionList.Any())
                {
                    var selectedExpRQFrmSession = ExpenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);

                    if (selectedExpRQFrmSession == null || selectedExpRQFrmSession.ExpenseRequisitionId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ExpenseRequisitionObj());
                    }
                    Session["_ListOfExpReqItems_"] = selectedExpRQFrmSession.ExpenseRequisitionItems.ToList();
                    return View(selectedExpRQFrmSession);
                }

                #endregion

                #region Request Response validations

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseRequisitionObj list is empty!";
                    return View(new ExpenseRequisitionObj());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  ExpenseRequisitionObj list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new ExpenseRequisitionObj());
                }

                #endregion

                #region Initilization   into Sessions

                var expenseRequisitionList = retVal.ExpenseRequisitions.Where(m => m.ProductId == ProductId && m.ProductItemId == ProductItemId && m.ClientId == ClientId).ToList();
                var SelectedExpenseRequisitionObj = expenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);
                Session["_ListOfExpReqItems_"] = SelectedExpenseRequisitionObj.ExpenseRequisitionItems;


                if (SelectedExpenseRequisitionObj == null || SelectedExpenseRequisitionObj.ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseRequisitionObj());
                }
                //Session["_ListOfExpReqItems_"] = requisitionItems;
                Session["_CurrentSelExpenseRequisition_"] = SelectedExpenseRequisitionObj;


                #endregion

                return View(SelectedExpenseRequisitionObj);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseRequisitionObj());
            }
        }

        public JsonResult ProcessEditExpenseRequisitionRequest(ExpenseRequisitionObj model)
        {
            try
            {
                #region Current User Session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion
                 
                #region Model Validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product required " });

                }
                if (model.DepartmentId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Department required " });

                }
                if (string.IsNullOrEmpty(model.GeneralRemark))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "GeneralRemark required " });

                }
                if (string.IsNullOrEmpty(model.Title))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Title required " });

                }
                if (model.TotalAmount < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Total Amount required " });

                }
                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }

                #endregion

                #region Cast Expense Requisition Items Session 
                var ListOfExpReqItems = (List<ExpenseRequisitionItemObj>)Session["_ListOfExpReqItems_"];
                #endregion

                #region Build Reuest object
                

                foreach (var reqItem in ListOfExpReqItems)
                {
                    reqItem.BeneficiaryId = model.BeneficiaryId;
                    reqItem.ExpenseRequisitionId = Convert.ToInt32(model.ExpenseRequisitionId);
                }
                var passObj = new EditExpenseRequisitionObj()
                {

                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    DepartmentId = model.DepartmentId,
                    Status = (int)RequestStatus.Processing,
                    GeneralRemark = model.GeneralRemark,
                    RegisteredBy = userData.UserId,
                    RequestNumber = model.RequestNumber,
                    RequestType = model.RequestType,
                    RequesterId = userData.UserId,
                    TimeStampRegistered = DateTime.Now.ToString("D"),
                    Title = model.Title,
                    TotalAmount = model.TotalAmount,
                    WorkflowStageId = model.WorkflowStageId,
                    ExpenseRequisitionItems = ListOfExpReqItems,
                    ExpenseRequisitionId = model.ExpenseRequisitionId,
                };


                #endregion

                #region Request response validation

                var response = ExpenseLookupServices.UpdateExpenseRequisition(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }

                #endregion

                Session["_CurrentSelExpenseRequisition_"] = null;
                ViewBag.ClientId = model.ClientId;
                ViewBag.ProductId = model.ProductId;
                ViewBag.ProductItemId = model.ProductItemId;


                #region Request and Response

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -2
                };

                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseRequisitions != null)
                {
                    var ExpenseRequisitions = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId)
                        .Where(m => m.ClientId == model.ClientId && m.ProductId == model.ProductId && m.ProductItemId == model.ProductItemId)
                        .ToList();
                    Session["_ExpenseRequisitionList_"] = ExpenseRequisitions;
                }

                #endregion

                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }

        public PartialViewResult _EditExpenseRequisitionItemsList(int? id)
        {
            try
            {
                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name);

                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return PartialView(new List<RegExpenseRequisitionObj>());
                }

                #endregion

                #region Check if Expense Item List is null else Fetch from Expense Item Service

                if (Session["_globalItems_"] is List<ExpenseItemObj> GlobalExpenseItems && GlobalExpenseItems.Any())
                {
                    Session["_globalItems_"] = GlobalExpenseItems.ToList();
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj()
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = 1
                    };
                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                    var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                    Session["_globalItems_"] = globalItemList;
                }

                #endregion

                #region Delete Item from Session

                if (Session["_ListOfExpReqItems_"] is List<ExpenseRequisitionItemObj> itemList && itemList.Any())
                {
                    if (id > 0)
                    {
                        var itemToDelete = itemList.FirstOrDefault(i => i.UniqueId == id);
                        itemList.Remove(itemToDelete);
                        Session["_ExpenseRequisitionItemList_"] = itemList;
                        return PartialView(new List<RegExpenseRequisitionObj>());
                    }

                    Session["_ExpenseRequisitionItemList_"] = new List<ExpenseRequisitionItemObj>();
                    return PartialView(new List<RegExpenseRequisitionObj>());
                }

                #endregion
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return PartialView(new List<RegExpenseRequisitionObj>());
            }
            return PartialView(new List<RegExpenseRequisitionObj>());
        }
        #endregion

        #region Copy ExpenseRequisitionObj

        public ActionResult CopyExpenseRequisition(int ExpenseRequisitionId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";

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

                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseRequisitionObj());
                }

                #endregion

                #region ExpenseRequisitionId Check
                if (ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new ExpenseRequisitionObj());
                }
                #endregion

                #region Check if Expense Item List is null else Fetch from Expense Item Service

                if (Session["_globalItems_"] is List<ExpenseItemObj> GlobalExpenseItems && GlobalExpenseItems.Any())
                {
                    Session["_globalItems_"] = GlobalExpenseItems.ToList();
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj()
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = 1
                    };
                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                    var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                    Session["_globalItems_"] = globalItemList;
                }

                #endregion

                #region Check if ExpenseRequisition Session List if null 

                if (Session["_ExpenseRequisitionList_"] is List<ExpenseRequisitionObj> ExpenseRequisitionList && ExpenseRequisitionList.Any())
                {
                    var selectedExpRQFrmSession = ExpenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);

                    if (selectedExpRQFrmSession == null || selectedExpRQFrmSession.ExpenseRequisitionId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new ExpenseRequisitionObj());
                    }
                    Session["_ListOfExpReqItems_"] = selectedExpRQFrmSession.ExpenseRequisitionItems.ToList();
                    return View(selectedExpRQFrmSession);
                }

                #endregion

                #region Request Response validations

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseRequisitionObj list is empty!";
                    return View(new ExpenseRequisitionObj());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  ExpenseRequisitionObj list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new ExpenseRequisitionObj());
                }

                #endregion

                #region Initilization   into Sessions

                var expenseRequisitionList = retVal.ExpenseRequisitions.Where(m => m.ProductId == ProductId && m.ProductItemId == ProductItemId && m.ClientId == ClientId).ToList();
                var SelectedExpenseRequisitionObj = expenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);
                Session["_ListOfExpReqItems_"] = SelectedExpenseRequisitionObj.ExpenseRequisitionItems;


                if (SelectedExpenseRequisitionObj == null || SelectedExpenseRequisitionObj.ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseRequisitionObj());
                }
                //Session["_ListOfExpReqItems_"] = requisitionItems;
                Session["_CurrentSelExpenseRequisition_"] = SelectedExpenseRequisitionObj;


                #endregion

                return View(expenseRequisitionList);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseRequisitionObj());
            }
        }

        public JsonResult ProcessCopyExpenseRequisitionRequest(RegExpenseRequisitionObj model)
        {
            try
            {
                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region model validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Product required " });

                }
                if (model.BeneficiaryType < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "BeneficiaryType required " });

                }
                if (model.DepartmentId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Department required " });

                }
                if (string.IsNullOrEmpty(model.GeneralRemark))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "GeneralRemark required " });

                }
                if (string.IsNullOrEmpty(model.Title))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Title required " });

                }
                if (model.TotalAmount < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Total Amount required " });

                }

                #endregion

                #region Get Client Product Workflow Stages

                var searchObjForWFS = new FilteredLookupSearchObj
                {
                    AdminUserId = userData.UserId,
                    ProductId = model.ProductId,
                    ClientId = model.ClientId,

                };

                var retValforWFS = ExpenseLookUpServices.LoadFilteredWorkFlowStages(searchObjForWFS, userData.Username);
                if (retValforWFS?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Work Flow Stages Not Configured For This Client " });
                }
                var workflowInitiation = retValforWFS.WorkflowStages.FirstOrDefault(x => x.Name.Trim() == WorkflowStage.Initiation.ToString());


                #endregion

                #region Check if current Item Session List is null

                var ListOfExpReqItems = (List<ExpenseRequisitionItemObj>)Session["_ListOfExpReqItems_"];
                if (ListOfExpReqItems == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region Remove Expense Requisition and Item ID's

                foreach (var req in ListOfExpReqItems)
                {
                    req.ExpenseRequisitionItemId = 0;
                    req.ExpenseRequisitionId = 0;
                }

                #endregion

                    #region Build Request Object

                    var requestObj = new RegExpenseRequisitionObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    DepartmentId = model.DepartmentId,
                    Status = (int)RequestStatus.Registered,
                    GeneralRemark = model.GeneralRemark,
                    RegisteredBy = userData.UserId,
                    RequestNumber = "EXP" + DateTime.Now.Day + DateTime.Now.Minute + DateTime.Now.Second + "XPAT",
                    RequestType = model.RequestType,
                    RequesterId = userData.UserId,
                    TimeStampRegistered = DateTime.Now.ToString("D"),
                    Title = model.Title,
                    TotalAmount = model.TotalAmount,
                    WorkflowStageId = workflowInitiation.WorkFlowStageId,
                    ExpenseRequisitionItems = ListOfExpReqItems,
                    BeneficiaryType = model.BeneficiaryType,
                };

                #endregion

                #region Request and Response Validations

                var response = ExpenseLookupServices.AddExpenseRequisition(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to Process Data" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -2
                };

                var retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseRequisitions != null)
                {
                    var ExpenseRequisitions = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).ToList();
                    Session["_ExpenseRequisitionList_"] = ExpenseRequisitions;
                }

                #endregion

                ViewBag.ClientId = model.ClientId;
                ViewBag.ProductId = model.ProductId;
                ViewBag.ProductItemId = model.ProductItemId;

                Session["_ListOfExpReqItems_"] = null;
                return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
            }
            catch (Exception ex)
            {
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
            }
        }

        #endregion

        #region Proeview Expense Requisition Item

        public PartialViewResult _PreviewRequisitionItemsList(int? id)
        {
            try
            {
                #region current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);

                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return PartialView(new List<RegExpenseRequisitionObj>());
                }

                #endregion

                #region get some of the properties of global Expense items from endpoint or session

                if (Session["_globalItems_"] is List<ExpenseItemObj> ExpenseItemList && ExpenseItemList.Any())
                {
                    Session["_globalItems_"] = ExpenseItemList.ToList();
                }
                else
                {
                    var itemSearchObj = new ExpenseItemSearchObj()
                    {
                        AdminUserId = userData.UserId,
                        ExpenseItemId = 0,
                        Status = 1
                    };

                    var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                    var globalItemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                    Session["_globalItems_"] = globalItemList;
                }

                #endregion

                #region delete item from items session

                if (Session["_ListOfExpReqItems_"] is List<ExpenseRequisitionItemObj> itemList && itemList.Any())
                {
                    if (id > 0)
                    {
                        var itemToDelete = itemList.FirstOrDefault(i => i.UniqueId == id || i.ExpenseRequisitionItemId == id);
                        itemList.Remove(itemToDelete);
                        Session["_ExpenseRequisitionItemList_"] = itemList;
                        return PartialView(new List<RegExpenseRequisitionObj>());
                    }

                    Session["_ExpenseRequisitionItemList_"] = new List<ExpenseRequisitionItemObj>();
                    return PartialView(new List<RegExpenseRequisitionObj>());
                }

                #endregion


            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return PartialView(new List<RegExpenseRequisitionObj>());
            }
            return PartialView(new List<RegExpenseRequisitionObj>());
        }



        public ActionResult _PreviewExpenseRequisitionItem(int ExpenseRequisitionId, int ExpenseRequisitionItemId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                #region current session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegExpenseRequisitionItemObj());
                }

                #endregion

                #region requisition and item ids validation

                if (ExpenseRequisitionId < 1)
                {
                    ExpenseRequisitionId = 0;
                    return View(new RegExpenseRequisitionItemObj());
                }

                if (ExpenseRequisitionItemId < 1)
                {
                    ExpenseRequisitionId = 0;
                    return View(new RegExpenseRequisitionItemObj());
                }

                #endregion

                ExpenseRequisitionRespObj retVal = null;
                if (Session["_ExpenseRequisitionList_"] is List<ExpenseRequisitionObj> ExpenseRequisitionList && ExpenseRequisitionList.Any())
                {
                    var RequisitionList = ExpenseRequisitionList.OrderBy(m => m.ExpenseRequisitionId).ToList();

                    var currentReq = RequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);
                    var ThisReqItem = currentReq.ExpenseRequisitionItems.Find(m => m.ExpenseRequisitionItemId == ExpenseRequisitionItemId);
                    return View(ThisReqItem);
                }

                var searchObj = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                retVal = ExpenseLookupServices.LoadExpenseRequisitions(searchObj, userData.Username);

                var expenseRequisitionList = retVal.ExpenseRequisitions.OrderBy(m => m.ExpenseRequisitionId).ToList();

                var expenseRequisitionObj = expenseRequisitionList.Find(m => m.ExpenseRequisitionId == ExpenseRequisitionId);
                var thisReqItem = expenseRequisitionObj.ExpenseRequisitionItems.Find(m => m.ExpenseRequisitionItemId == ExpenseRequisitionItemId);

                if (thisReqItem == null || thisReqItem.ExpenseRequisitionItemId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RegExpenseRequisitionItemObj());
                }


                return View(thisReqItem);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseRequisitionItemObj());
            }
        }

        #endregion
    }
}