using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using PluglexHelper.CoreService; 
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;

namespace PlugLitePortal.Controllers.Setup
{
    public class ExpenseItemSettingManagerController : Controller
    {
        public ActionResult Index(int? clientId, int? productId )
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

                #region Current User Session Check
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<ExpenseItemSettingObj>());
                }

                #endregion
                
                #region Other Objects Names
                var categorySearchObj = new ExpenseCategorySearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseCategoryId = 0,
                    Status = 1
                };

                var classificationSearchObj = new ExpenseClassificationSearchObj()
                {
                    AdminUserId = userData.UserId,
                    ExpenseClassificationId = 0,
                    Status = 1
                };


                var typeSearchObj = new ExpenseTypeSearchObj()
                {
                    AdminUserId = userData.UserId,
                    ExpenseTypeId = 0,
                    Status = 1
                };
                var itemSearchObj = new ExpenseItemSearchObj()
                {
                    AdminUserId = userData.UserId,
                    ExpenseItemId = 0,
                    Status = 1
                };


                #endregion

                #region Client Expense Item And Dependent Objects Session Check


                if (Session["_ExpenseItemSettingList_"] is List<ExpenseItemSettingObj> ExpenseItemSetting && ExpenseItemSetting.Any())
                {
                    if (Session["_Category_"] is List<ExpenseCategoryObj> expenseCategorys && !expenseCategorys.Any())
                    {
                        Session["_Category_"] = new List<ExpenseCategoryObj>();
                    }
                    if (Session["_Classification_"] is List<ExpenseClassificationObj> expenseClassifications && !expenseClassifications.Any())
                    {
                        Session["_Classification_"] = new List<ExpenseCategoryObj>();
                    }
                    if (Session["_Types_"] is List<ExpenseTypeObj> expenseType && !expenseType.Any())
                    {
                        Session["_Types_"] = new List<ExpenseCategoryObj>();
                    }
                    if (Session["_Items_"] is List<ExpenseItemObj> expenseItem && !expenseItem.Any())
                    {
                        Session["_Items_"] = new List<ExpenseItemObj>();
                    }
                    var clientExpenseItemList = ExpenseItemSetting.Where(m => m.ClientId == ClientId && m.ProductId == ProductId  ).ToList();
                    return View(clientExpenseItemList);
                }

                #endregion

                #region Request and response Validations

                var searchObj = new ExpenseItemSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseItemSettingId = 0,
                    Status = 1
                };
                var retVal = ExpenseItemSettingServices.LoadExpenseItemSettings(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " ExpenseItemSetting list is empty!";
                    return View(new List<ExpenseItemSettingObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " ExpenseItemSetting list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<ExpenseItemSettingObj>());
                }
                if (retVal.ExpenseItemSettings == null || !retVal.ExpenseItemSettings.Any())
                {
                    ViewBag.Error = " ExpenseItemSetting list is empty!";
                    return View(new List<ExpenseItemSettingObj>());
                }
                 
                #endregion

                #region Initialize object Lists to sessions


                var categoryRetVal = ExpenseLookUpServices.LoadExpenseCategories(categorySearchObj, userData.Username);
                var catList = categoryRetVal.ExpenseCategories.OrderBy(m => m.ExpenseCategoryId).ToList();
                Session["_Category_"] = catList;

                var classificationRetVal = ExpenseLookUpServices.LoadClassifications(classificationSearchObj, userData.Username);
                var classificationList = classificationRetVal.ExpenseClassifications.OrderBy(m => m.ExpenseClassificationId).ToList();
                Session["_Classification_"] = classificationList;

                var typeRetVal = ExpenseLookUpServices.LoadExpenseTypes(typeSearchObj, userData.Username);
                var typeList = typeRetVal.ExpenseTypes.OrderBy(m => m.ExpenseTypeId).ToList();
                Session["_Types_"] = typeList;

                var itemRetVal = ExpenseLookUpServices.LoadExpenseItems(itemSearchObj, userData.Username);
                var itemList = itemRetVal.ExpenseItems.OrderBy(m => m.ExpenseItemId).ToList();
                Session["_Items_"] = itemList;

                var ExpenseItemSettings = retVal.ExpenseItemSettings.OrderBy(m => m.ExpenseItemSettingId).ToList();
                Session["_ExpenseItemSettingList_"] = ExpenseItemSettings.Where(m =>
                       m.ClientId == ClientId && m.ProductId == ProductId).ToList();


                #endregion

                var clientProdExpItemList = ExpenseItemSettings.Where(m =>
                       m.ClientId == ClientId && m.ProductId == ProductId ).ToList();

                return View(clientProdExpItemList);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< ExpenseItemSettingObj>());
            }
        }

        #region Add ExpenseItemSetting
        public ActionResult AddExpenseItemSetting(int clientId, int productId )
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
                    return View(new RegExpenseItemSettingObj());
                }

                return View(new RegExpenseItemSettingObj { ProductId = productId, ClientId = clientId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegExpenseItemSettingObj());
            }
        }
        public JsonResult ProcessAddExpenseItemSettingRequest(RegExpenseItemSettingObj model)
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

                #region Model Validations

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product required " });
                }
                
                if (model.ExpenseCategoryId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "ExpenseCategory required " });
                }
                if (model.ExpenseClassificationId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Expense Classification required " });
                }

                if (model.ExpenseItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Expense Item required " });
                }
                
                if (model.ExpenseTypeId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Expense Type required " });
                }
                
                if (model.PreferedVendorId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Prefered Vendor required " });
                }
                
                if (model.RequestFrequency < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Request Frequency required " });
                }
                if (model.RequestFrequencyType < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Request Frequency Type required " });
                }
                
                if (model.UnitPrice < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Unit Price  required " });
                }

                #endregion

                #region Check if item Exist From Session

                var previousExpenseItemSettingList = (List<ExpenseItemSettingObj>)Session["_ExpenseItemSettingList_"];
                if (previousExpenseItemSettingList != null)
                {
                    if (previousExpenseItemSettingList.Count(x => x.ExpenseItemId == model.ExpenseItemId
                                                               && x.ClientId == model.ClientId &&
                                                               x.ProductId == model.ProductId
                                                               && x.ProductItemId == model.ProductItemId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Expense Lookup  Already Exist!" });
                }

                #endregion

                #region Build Request Object

                var requestObj = new RegExpenseItemSettingObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    RegisteredBy = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    ExpenseCategoryId = model.ExpenseCategoryId,
                    ExpenseClassificationId = model.ExpenseClassificationId,
                    ExpenseItemId = model.ExpenseItemId,
                    ExpenseTypeId = model.ExpenseTypeId,
                    IsEnabled = true,
                    PreferedVendorId = model.PreferedVendorId,
                    RegularQuantity = model.RegularQuantity,
                    RequestFrequency = model.RequestFrequency,
                    RequestFrequencyType = model.RequestFrequencyType,
                    Status = 1,
                    UnitPrice = model.UnitPrice,
                };

                #endregion

                #region Response And Validations 

                var response = ExpenseItemSettingServices.AddExpenseItemSetting(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new ExpenseItemSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseItemSettingId = 0,
                    Status = -2
                };

                var retVal = ExpenseItemSettingServices.LoadExpenseItemSettings(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseItemSettings != null)
                {
                    var ExpenseItemSettings = retVal.ExpenseItemSettings.OrderBy(m => m.ExpenseItemSettingId).ToList();
                    Session["_ExpenseItemSettingList_"] = ExpenseItemSettings.Where(m => m.ClientId == model.ClientId && m.ProductId == model.ProductId ).ToList();
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
        #endregion

        #region ExpenseItemSetting Detail
        public ActionResult _ExpenseItemSettingDetail(int ExpenseItemSettingId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseItemSettingObj());
                }

                if (ExpenseItemSettingId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new ExpenseItemSettingObj());
                }

                if (!(Session["_ExpenseItemSettingList_"] is List<ExpenseItemSettingObj> ExpenseItemSettings) || ExpenseItemSettings.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseItemSettingObj());
                }

                var thisExpenseItemSetting = ExpenseItemSettings.Find(m => m.ExpenseItemSettingId == ExpenseItemSettingId);
                if (thisExpenseItemSetting == null || thisExpenseItemSetting.ExpenseItemSettingId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseItemSettingObj());
                }

                return View(thisExpenseItemSetting);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseItemSettingObj());
            }
        }
        #endregion

        #region Edit ExpenseItemSetting

        public ActionResult _EditExpenseItemSetting(int ExpenseItemSettingId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";

                #region User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new ExpenseItemSettingObj());
                }

                #endregion

                if (ExpenseItemSettingId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new ExpenseItemSettingObj());
                }

                if (!(Session["_ExpenseItemSettingList_"] is List<ExpenseItemSettingObj> ExpenseItemSettingList) || ExpenseItemSettingList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseItemSettingObj());
                }

                var ExpenseItemSetting = ExpenseItemSettingList.Find(m => m.ExpenseItemSettingId == ExpenseItemSettingId);
                if (ExpenseItemSetting == null || ExpenseItemSetting.ExpenseItemSettingId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new ExpenseItemSettingObj());
                }

                Session["_CurrentSelExpenseItemSetting_"] = ExpenseItemSetting;

                ExpenseItemSetting.StatusVal = ExpenseItemSetting.Status == 1;
                return View(ExpenseItemSetting);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new ExpenseItemSettingObj());
            }
        }
        public JsonResult ProcessEditExpenseItemSettingRequest(ExpenseItemSettingObj model)
        {
            try
            {
                #region User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                } 
                #endregion

                #region Check if Item Session is null 
                var selExpenseItemSetting = Session["_CurrentSelExpenseItemSetting_"] as ExpenseItemSettingObj;
                if (selExpenseItemSetting == null || selExpenseItemSetting.ExpenseItemSettingId < 1)
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
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product required " });
                }

                if (model.ExpenseCategoryId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "ExpenseCategory required " });
                }
                if (model.ExpenseClassificationId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Expense Classification required " });
                }

                if (model.ExpenseItemId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Expense Item required " });
                }

                if (model.ExpenseTypeId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Expense Type required " });
                }

                if (model.PreferedVendorId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Prefered Vendor required " });
                }

                if (model.RequestFrequency < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Request Frequency required " });
                }
                if (model.RequestFrequencyType < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Request Frequency Type required " });
                }

                if (model.UnitPrice < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Unit Price  required " });
                }
                if (model.ExpenseItemSettingId < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Unit Price  required " });
                }

                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }


                #endregion

                #region Build Request Object

                var passObj = new EditExpenseItemSettingObj()
                {

                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    RegisteredBy = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    ExpenseCategoryId = model.ExpenseCategoryId,
                    ExpenseClassificationId = model.ExpenseClassificationId,
                    ExpenseItemId = model.ExpenseItemId,
                    ExpenseTypeId = model.ExpenseTypeId,
                    IsEnabled = true,
                    PreferedVendorId = model.PreferedVendorId,
                    RegularQuantity = model.RegularQuantity,
                    RequestFrequency = model.RequestFrequency,
                    RequestFrequencyType = model.RequestFrequencyType,
                    Status = 1,
                    UnitPrice = model.UnitPrice,
                    ExpenseItemSettingId = model.ExpenseItemSettingId,

                };

                #endregion
                 
                #region Response and Validations Check
                var response = ExpenseItemSettingServices.UpdateExpenseItemSetting(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }

                Session["_CurrentSelExpenseItemSetting_"] = null;

                var searchObj = new ExpenseItemSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseItemSettingId = 0,
                    Status = -2
                };

                var retVal = ExpenseItemSettingServices.LoadExpenseItemSettings(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.ExpenseItemSettings != null)
                {
                    var ExpenseItemSettings = retVal.ExpenseItemSettings.OrderBy(m => m.ExpenseItemSettingId).ToList();
                    Session["_ExpenseItemSettingList_"] = ExpenseItemSettings.Where(m => m.ClientId == model.ClientId && m.ProductId == model.ProductId && m.ProductItemId == model.ProductItemId).ToList();
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
        #endregion


    }
}