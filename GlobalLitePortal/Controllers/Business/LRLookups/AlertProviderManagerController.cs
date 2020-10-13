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
    public class AlertProviderManagerController : Controller
    {
        [PortalAuthorize(Roles = "PortalAdmin,CRMAdmin,CRMManager")]
        public ActionResult Index()
        {
            try
            {
                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<AlertProviderObj>());
                }

                if (Session["_AlertProviderList_"] is List<AlertProviderObj> mylist && mylist.Any())
                {
                    return View(mylist);
                }

                var searchObj = new AlertProviderSearchObj
                {
                    AdminUserId = userData.UserId,
                    AlertProviderId = 0,
                    Status = 0
                };

                var retVal = AlertProviderService.LoadAlertProviders(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "AlertProvider list is empty!";
                    return View(new List<AlertProviderObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " AlertProvider list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<AlertProviderObj>());
                }
                if (retVal.AlertProviders == null || !retVal.AlertProviders.Any())
                {
                    ViewBag.Error = "AlertProvider list is empty!";
                    return View(new List<AlertProviderObj>());
                }

                //Order By AlertProvider Name 
                var AlertProviders = retVal.AlertProviders.OrderBy(m => m.Name).ToList();
                Session["_AlertProviderList_"] = AlertProviders;
                return View(AlertProviders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<AlertProviderObj>());
            }
        }


        #region AlertProvider Detail
            public ActionResult _AlertProviderDetail(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertProviderObj());
                    }

                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertProviderObj());
                    }

                    if (!(Session["_AlertProviderList_"] is List<AlertProviderObj> AlertProviders) || AlertProviders.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderObj());
                    }

                    var thisAlertProvider = AlertProviders.Find(m => m.AlertProviderId == id);
                    if (thisAlertProvider == null || thisAlertProvider.AlertProviderId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderObj());
                    }

                    return View(thisAlertProvider);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertProviderObj());
                }
            }
        #endregion
        #region Add AlertProvider
            public ActionResult _AddAlertProvider()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegAlertProviderObj());
                    }

                    return View(new RegAlertProviderObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegAlertProviderObj());
                }
            }
            public JsonResult ProcessAddAlertProviderRequest(RegAlertProviderObj model)
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

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid SMS Route Name" });
                    }

                    if (string.IsNullOrEmpty(model.ShortName) || model.ShortName.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Alert Short Name" });
                    }

                    model.AdminUserId = userData.UserId;
                    model.Status = model.StatusVal ? 1 : 0;

                    if (Session["_AlertProviderList_"] is List<AlertProviderObj> previousAlertProviderList)
                    {
                        if (previousAlertProviderList.Count(x => x.Name.ToLower().Trim().ToStandardHash() == model.Name.ToLower().Trim().ToStandardHash()) > 0)
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Alert Item Information  Already Exist!" });
                    }


                    var response = AlertProviderService.AddAlertProvider(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_AlertProviderList_"] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Edit AlertProvider
            public ActionResult _EditAlertProvider(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertProviderObj());
                    }
                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertProviderObj());
                    }

                    if (!(Session["_AlertProviderList_"] is List<AlertProviderObj> AlertProviderList) || AlertProviderList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderObj());
                    }

                    var AlertProvider = AlertProviderList.Find(m => m.AlertProviderId == id);
                    if (AlertProvider == null || AlertProvider.AlertProviderId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertProviderObj());
                    }

                    Session["_CurrentSelAlertProvider_"] = AlertProvider;

                    AlertProvider.StatusVal = AlertProvider.Status == 1;
                    return View(AlertProvider);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertProviderObj());
                }
            }
            public JsonResult ProcessEditAlertProviderRequest(AlertProviderObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selAlertProvider = Session["_CurrentSelAlertProvider_"] as AlertProviderObj;
                    if (selAlertProvider == null || selAlertProvider.AlertProviderId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.AlertProviderId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertProvider Name" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertProvider Name" });
                    }

                    var passObj = new EditAlertProviderObj
                    {
                        AdminUserId = userData.UserId,
                        Name = model.Name,
                        AlertProviderId = selAlertProvider.AlertProviderId,
                        ShortName = model.ShortName,
                        Status = model.StatusVal ? 1 : 0,
                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = AlertProviderService.UpdateAlertProvider(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelAlertProvider_"] = null;
                    Session["_AlertProviderList_"] = null;
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
                var previousAlertProviderList = Session["_AlertProviderList_"] as List<AlertProviderObj>;
                if (previousAlertProviderList == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                }

                var thisItem = previousAlertProviderList.Find(m => m.AlertProviderId == id);
                if (thisItem == null || thisItem.AlertProviderId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                }

                var passObj = new DeleteAlertProviderObj
                {
                    AdminUserId = userData.UserId,
                    AlertProviderId = id
                };

                var response = AlertProviderService.DeleteAlertProvider(passObj, User.Identity.Name);
                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                }

                Session["_AlertProviderList_"] = null;
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