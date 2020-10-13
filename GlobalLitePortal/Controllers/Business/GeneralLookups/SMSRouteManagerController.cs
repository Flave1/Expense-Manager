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

namespace GlobalLitePortal.Controllers.Business.GeneralLookups
{
    public class SMSRouteManagerController : Controller
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
                    return View(new List<SMSRouteObj>());
                }

                if (Session["_SMSRouteList_"] is List<SMSRouteObj> mylist && mylist.Any())
                {
                    return View(mylist);
                }

                var searchObj = new SMSRouteSearchObj
                {
                    AdminUserId = userData.UserId,
                    SMSRouteId = 0,
                    Status = 0
                };

                var retVal = SMSRouteService.LoadSMSRoutes(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "SMSRoute list is empty!";
                    return View(new List<SMSRouteObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " SMSRoute list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<SMSRouteObj>());
                }
                if (retVal.SMSRoutes == null || !retVal.SMSRoutes.Any())
                {
                    ViewBag.Error = "SMSRoute list is empty!";
                    return View(new List<SMSRouteObj>());
                }

                //Order By SMSRoute Name 
                var SMSRoutes = retVal.SMSRoutes.OrderBy(m => m.Name).ToList();
                Session["_SMSRouteList_"] = SMSRoutes;
                return View(SMSRoutes);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<SMSRouteObj>());
            }
        }


        #region SMSRoute Detail
            public ActionResult _SMSRouteDetail(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new SMSRouteObj());
                    }

                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new SMSRouteObj());
                    }

                    if (!(Session["_SMSRouteList_"] is List<SMSRouteObj> SMSRoutes) || SMSRoutes.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSRouteObj());
                    }

                    var thisSMSRoute = SMSRoutes.Find(m => m.SMSRouteId == id);
                    if (thisSMSRoute == null || thisSMSRoute.SMSRouteId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSRouteObj());
                    }

                    return View(thisSMSRoute);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new SMSRouteObj());
                }
            }
        #endregion
        #region Add SMSRoute
            public ActionResult _AddSMSRoute()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegSMSRouteObj());
                    }

                    return View(new RegSMSRouteObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegSMSRouteObj());
                }
            }
            public JsonResult ProcessAddSMSRouteRequest(RegSMSRouteObj model)
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

                    if (model.Rate < 0)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid SMS Rate" });
                    }

                    model.AdminUserId = userData.UserId;
                    model.Status = model.StatusVal ? 1 : 0;

                    if (Session["_SMSRouteList_"] is List<SMSRouteObj> previousSMSRouteList)
                    {
                        if (previousSMSRouteList.Count(x => x.Name.ToLower().Trim().ToStandardHash() == model.Name.ToLower().Trim().ToStandardHash()) > 0)
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "SMS Route Information  Already Exist!" });
                    }


                    var response = SMSRouteService.AddSMSRoute(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_SMSRouteList_"] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Edit SMSRoute
            public ActionResult _EditSMSRoute(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new SMSRouteObj());
                    }
                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new SMSRouteObj());
                    }

                    if (!(Session["_SMSRouteList_"] is List<SMSRouteObj> SMSRouteList) || SMSRouteList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSRouteObj());
                    }

                    var SMSRoute = SMSRouteList.Find(m => m.SMSRouteId == id);
                    if (SMSRoute == null || SMSRoute.SMSRouteId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSRouteObj());
                    }

                    Session["_CurrentSelSMSRoute_"] = SMSRoute;

                    SMSRoute.StatusVal = SMSRoute.Status == 1;
                    return View(SMSRoute);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new SMSRouteObj());
                }
            }
            public JsonResult ProcessEditSMSRouteRequest(SMSRouteObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selSMSRoute = Session["_CurrentSelSMSRoute_"] as SMSRouteObj;
                    if (selSMSRoute == null || selSMSRoute.SMSRouteId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.SMSRouteId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid SMSRoute Name" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid SMSRoute Name" });
                    }

                    var passObj = new EditSMSRouteObj
                    {
                        AdminUserId = userData.UserId,
                        Name = model.Name,
                        SMSRouteId = selSMSRoute.SMSRouteId,
                        Rate = model.Rate,
                        Bonus = model.Bonus,
                        AppBonus = model.AppBonus,
                        Loyalty = model.Loyalty,
                        Status = model.StatusVal ? 1 : 0,
                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = SMSRouteService.UpdateSMSRoute(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelSMSRoute_"] = null;
                    Session["_SMSRouteList_"] = null;
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
                var previousSMSRouteList = Session["_SMSRouteList_"] as List<SMSRouteObj>;
                if (previousSMSRouteList == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                }

                var thisItem = previousSMSRouteList.Find(m => m.SMSRouteId == id);
                if (thisItem == null || thisItem.SMSRouteId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                }

                var passObj = new DeleteSMSRouteObj
                {
                    AdminUserId = userData.UserId,
                    SMSRouteId = id
                };

                var response = SMSRouteService.DeleteSMSRoute(passObj, User.Identity.Name);
                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                }

                Session["_SMSRouteList_"] = null;
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