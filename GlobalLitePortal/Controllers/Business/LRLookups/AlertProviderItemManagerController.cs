using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GlobalLitePortal.PortalCore;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using PluglexHelper.CoreService;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.Business.LRLookups
{
    public class AlertProviderItemManagerController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin,CRMAdmin,CRMManager")]
        public ActionResult Index(int? providerId)
        {
            try
            {
                var provId = providerId ?? 0;
              return View(new AlertProviderItemInfoObj{AlertProviderId = provId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new AlertProviderItemInfoObj());
            }
        }
        #region Display View
            public ActionResult _ProviderItemView(int providerId)
            {
                try
                {
                    ViewBag.Error = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name);
                    if (userData == null || userData.UserId < 1)
                    {
                        ViewBag.Error = "Session Has Expired! Please Re-Login";
                        return View(new List<AlertProviderItemObj>());
                    }
                    if (Session["_AlertProviderItemList_" + providerId] is List<AlertProviderItemObj> mylist && mylist.Any())
                    {
                        return View(mylist);
                    }

                    var searchObj = new AlertProviderItemSearchObj
                    {
                        AdminUserId = userData.UserId,
                        AlertProviderItemId = 0,
                        AlertProviderId = providerId,
                        AlertItemId = 0,
                        Status = 0
                    };

                    var retVal = AlertProviderItemService.LoadAlertProviderItems(searchObj, userData.Username);
                    if (retVal?.Status == null)
                    {
                        ViewBag.Error = "Alert Provider Item list is empty!";
                        return View(new List<AlertProviderItemObj>());
                    }

                    if (!retVal.Status.IsSuccessful)
                    {
                        ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                            ? " Alert Provider Item list is empty!"
                            : retVal.Status.Message.FriendlyMessage;
                        return View(new List<AlertProviderItemObj>());
                    }
                    if (retVal.AlertProviderItems == null || !retVal.AlertProviderItems.Any())
                    {
                        ViewBag.Error = "Alert Provider Item list is empty!";
                        return View(new List<AlertProviderItemObj>());
                    }

                    //Order By AlertProviderItem Name 
                    var AlertProviderItems = retVal.AlertProviderItems.OrderBy(m => m.AlertProviderName).ToList();
                    Session["_AlertProviderItemList_" + providerId] = AlertProviderItems;

                    return View(retVal.AlertProviderItems);

                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Unable to load Users' list! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new List<AlertProviderItemObj>());
                }
            }
        #endregion
        #region AlertProviderItem Detail
            public ActionResult _AlertProviderItemDetail(int id, int providerId)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertProviderItemObj());
                    }

                    if (id < 1 || providerId < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertProviderItemObj());
                    }

                    if (!(Session["_AlertProviderItemList_" + providerId] is List<AlertProviderItemObj> AlertProviderItems) || AlertProviderItems.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderItemObj());
                    }

                    var thisAlertProviderItem = AlertProviderItems.Find(m => m.AlertProviderItemId == id);
                    if (thisAlertProviderItem == null || thisAlertProviderItem.AlertProviderItemId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderItemObj());
                    }

                    return View(thisAlertProviderItem);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertProviderItemObj());
                }
            }
        #endregion
        #region Add AlertProviderItem
            public ActionResult _AddAlertProviderItem(int providerId, string providerName)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegAlertProviderItemObj());
                    }

                    if (providerId < 1 || string.IsNullOrEmpty(providerName) || providerName.Length < 2)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegAlertProviderItemObj());
                    }
                    return View(new RegAlertProviderItemObj{AlertProviderId = providerId, AlertProviderName = providerName});
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegAlertProviderItemObj());
                }
            }
            public JsonResult ProcessAddAlertProviderItemRequest(RegAlertProviderItemObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }
                    
                    if (model.AlertProviderId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Kindly select valid Alert Provider" });
                    }

                    if (model.AlertItemId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Kindly select valid Alert Item" });
                    }
                    
                    model.AdminUserId = userData.UserId;
                    model.Status = model.StatusVal ? 1 : 0;
                
                    var response = AlertProviderItemService.AddAlertProviderItem(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_AlertProviderItemList_" + model.AlertProviderId] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Edit AlertProviderItem
            public ActionResult _EditAlertProviderItem(int id, int providerId)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertProviderItemObj());
                    }
                    if (id < 1 || providerId < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertProviderItemObj());
                    }

                    if (!(Session["_AlertProviderItemList_" + providerId] is List<AlertProviderItemObj> AlertProviderItemList) || AlertProviderItemList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderItemObj());
                    }

                    var AlertProviderItem = AlertProviderItemList.Find(m => m.AlertProviderItemId == id);
                    if (AlertProviderItem == null || AlertProviderItem.AlertProviderItemId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderItemObj());
                    }

                    Session["_CurrentSelAlertProviderItem_"] = AlertProviderItem;

                    AlertProviderItem.StatusVal = AlertProviderItem.Status == 1;
                    return View(AlertProviderItem);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertProviderItemObj());
                }
            }
            public JsonResult ProcessEditAlertProviderItemRequest(AlertProviderItemObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selAlertProviderItem = Session["_CurrentSelAlertProviderItem_"] as AlertProviderItemObj;
                    if (selAlertProviderItem == null || selAlertProviderItem.AlertProviderItemId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                 
                    if (model.AlertItemId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Kindly select valid Alert Item" });
                    }

                    var passObj = new EditAlertProviderItemObj
                        {
                            AdminUserId = userData.UserId,
                            AlertProviderItemId = selAlertProviderItem.AlertProviderItemId,
                            AlertProviderId = selAlertProviderItem.AlertProviderId,
                            AlertItemId = model.AlertItemId,
                            Status = model.StatusVal ? 1 : 0,
                        };

                        if (!GenericVal.Validate(model, out var msg))
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                        }


                        var response = AlertProviderItemService.UpdateAlertProviderItem(passObj, userData.Username);
                        if (response?.Status == null)
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                        }

                        if (!response.Status.IsSuccessful)
                        {
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                        }

                        Session["_CurrentSelAlertProviderItem_"] = null;
                        Session["_AlertProviderItemList_" + selAlertProviderItem.AlertProviderId] = null;
                        return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Delete Item
        public JsonResult ProcessDeleteRequest(int id)
        {
            try
            {

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (id < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Invalid / Empty Selection" });
                }
                var previousAlertProviderItemList = Session["_AlertProviderItemList_"] as List<AlertProviderItemObj>;
                if (previousAlertProviderItemList == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                }

                var thisItem = previousAlertProviderItemList.Find(m => m.AlertProviderItemId == id);
                if (thisItem == null || thisItem.AlertProviderItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                }

                var passObj = new DeleteAlertProviderItemObj
                {
                    AdminUserId = userData.UserId,
                    AlertProviderItemId = id
                };

                var response = AlertProviderItemService.DeleteAlertProviderItem(passObj, User.Identity.Name);
                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                }

                Session["_AlertProviderItemList_" + id] = null;
                return Json(new { IsAuthenticated = true, IsSuccessful = true, Error = "" });

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