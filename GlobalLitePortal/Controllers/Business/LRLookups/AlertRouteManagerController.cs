using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal.PortalCore;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIServices;
using PluglexHelper.CoreService;
using XPLUG.WEBTOOLKIT;

namespace GlobalLitePortal.Controllers.Business.LRLookups
{
    public class AlertRouteManagerController : Controller
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
                    return View(new List<AlertRouteObj>());
                }

                if (Session["_AlertRouteList_"] is List<AlertRouteObj> mylist && mylist.Any())
                {
                    return View(mylist);
                }

                var searchObj = new AlertRouteSearchObj
                {
                    AdminUserId = userData.UserId,
                    AlertRouteId = 0,
                    Status = 0
                };

                var retVal = AlertRouteService.LoadAlertRoutes(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "AlertRoute list is empty!";
                    return View(new List<AlertRouteObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " AlertRoute list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<AlertRouteObj>());
                }
                if (retVal.AlertRoutes == null || !retVal.AlertRoutes.Any())
                {
                    ViewBag.Error = "AlertRoute list is empty!";
                    return View(new List<AlertRouteObj>());
                }

                //Order By AlertRoute Name 
                var AlertRoutes = retVal.AlertRoutes.OrderBy(m => m.SMSRouteName).ToList();
                Session["_AlertRouteList_"] = AlertRoutes;
                return View(AlertRoutes);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<AlertRouteObj>());
            }
        }
        
        #region AlertRoute Detail
            public ActionResult _AlertRouteDetail(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertRouteObj());
                    }

                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertRouteObj());
                    }

                    if (!(Session["_AlertRouteList_"] is List<AlertRouteObj> AlertRoutes) || AlertRoutes.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertRouteObj());
                    }

                    var thisAlertRoute = AlertRoutes.Find(m => m.AlertRouteId == id);
                    if (thisAlertRoute == null || thisAlertRoute.AlertRouteId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertRouteObj());
                    }

                    return View(thisAlertRoute);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertRouteObj());
                }
            }
        #endregion
        #region Add AlertRoute
            public ActionResult _AddAlertRoute()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegAlertRouteObj());
                    }

                    return View(new RegAlertRouteObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegAlertRouteObj());
                }
            }
            public JsonResult ProcessAddAlertRouteRequest(RegAlertRouteObj model)
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
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Alert Route Name" });
                    }

                    if (model.SMSRouteId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Kindly select valid SMS Route" });
                    }

                    if (model.AlertNetworkId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Kindly select valid Alert Network" });
                    }

                    if (model.SMSProviderId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Kindly select valid SMS Provider" });
                    }

                    model.AdminUserId = userData.UserId;
                    model.Status = model.StatusVal ? 1 : 0;

                    if (Session["_AlertRouteList_"] is List<AlertRouteObj> previousAlertRouteList)
                    {
                        if (previousAlertRouteList.Count(x => x.Name.ToLower().Trim().ToStandardHash() == model.Name.ToLower().Trim().ToStandardHash()) > 0)
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Alert Item Information  Already Exist!" });
                    }


                    var response = AlertRouteService.AddAlertRoute(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_AlertRouteList_"] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Edit AlertRoute
            public ActionResult _EditAlertRoute(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertRouteObj());
                    }
                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertRouteObj());
                    }

                    if (!(Session["_AlertRouteList_"] is List<AlertRouteObj> AlertRouteList) || AlertRouteList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertRouteObj());
                    }

                    var AlertRoute = AlertRouteList.Find(m => m.AlertRouteId == id);
                    if (AlertRoute == null || AlertRoute.AlertRouteId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertRouteObj());
                    }

                    Session["_CurrentSelAlertRoute_"] = AlertRoute;

                    AlertRoute.StatusVal = AlertRoute.Status == 1;
                    return View(AlertRoute);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertRouteObj());
                }
            }
            public JsonResult ProcessEditAlertRouteRequest(AlertRouteObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selAlertRoute = Session["_CurrentSelAlertRoute_"] as AlertRouteObj;
                    if (selAlertRoute == null || selAlertRoute.AlertRouteId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.AlertRouteId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertRoute Name" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertRoute Name" });
                    }

                    var passObj = new EditAlertRouteObj
                    {
                        AdminUserId = userData.UserId,
                        Name = model.Name,
                        AlertRouteId = selAlertRoute.AlertRouteId,
                        SMSProviderId = model.SMSProviderId,
                        SMSRouteId = selAlertRoute.SMSRouteId,
                        AlertNetworkId = selAlertRoute.AlertNetworkId,
                        Status = model.StatusVal ? 1 : 0,
                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = AlertRouteService.UpdateAlertRoute(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelAlertRoute_"] = null;
                    Session["_AlertRouteList_"] = null;
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
                    var previousAlertRouteList = Session["_AlertRouteList_"] as List<AlertRouteObj>;
                    if (previousAlertRouteList == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                    }

                    var thisItem = previousAlertRouteList.Find(m => m.AlertRouteId == id);
                    if (thisItem == null || thisItem.AlertRouteId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                    }

                    var passObj = new DeleteAlertRouteObj
                    {
                        AdminUserId = userData.UserId,
                        AlertRouteId = id
                    };

                    var response = AlertRouteService.DeleteAlertRoute(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }

                    Session["_AlertRouteList_"] = null;
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