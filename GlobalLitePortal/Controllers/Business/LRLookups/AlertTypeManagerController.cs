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
    public class AlertTypeManagerController : Controller
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
                    return View(new List<AlertTypeObj>());
                }

                if (Session["_AlertTypeList_"] is List<AlertTypeObj> mylist && mylist.Any())
                {
                    return View(mylist);
                }

                var searchObj = new AlertTypeSearchObj
                {
                    AdminUserId = userData.UserId,
                    AlertTypeId = 0,
                    Status = 0
                };

                var retVal = AlertTypeService.LoadAlertTypes(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "Alert Type list is empty!";
                    return View(new List<AlertTypeObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " Alert Type list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<AlertTypeObj>());
                }
                if (retVal.AlertTypes == null || !retVal.AlertTypes.Any())
                {
                    ViewBag.Error = "Alert Type list is empty!";
                    return View(new List<AlertTypeObj>());
                }

                //Order By AlertType Name 
                var AlertTypes = retVal.AlertTypes.OrderBy(m => m.AlertProviderId).ToList();
                Session["_AlertTypeList_"] = AlertTypes;
                return View(AlertTypes);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<AlertTypeObj>());
            }
        }


        #region AlertType Detail
            public ActionResult _AlertTypeDetail(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertTypeObj());
                    }

                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertTypeObj());
                    }

                    if (!(Session["_AlertTypeList_"] is List<AlertTypeObj> AlertTypes) || AlertTypes.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertTypeObj());
                    }

                    var thisAlertType = AlertTypes.Find(m => m.AlertTypeId == id);
                    if (thisAlertType == null || thisAlertType.AlertTypeId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertTypeObj());
                    }

                    return View(thisAlertType);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertTypeObj());
                }
            }
        #endregion
        #region Add AlertType
            public ActionResult _AddAlertType()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegAlertTypeObj());
                    }

                    return View(new RegAlertTypeObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegAlertTypeObj());
                }
            }
            public JsonResult ProcessAddAlertTypeRequest(RegAlertTypeObj model)
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

                    if (model.AlertProviderId < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Alert Provider" });
                    }
                    
                    model.AdminUserId = userData.UserId;
                   
                    var response = AlertTypeService.AddAlertType(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_AlertTypeList_"] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Edit AlertType
            public ActionResult _EditAlertType(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertTypeObj());
                    }
                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertTypeObj());
                    }

                    if (!(Session["_AlertTypeList_"] is List<AlertTypeObj> AlertTypeList) || AlertTypeList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertTypeObj());
                    }

                    var AlertType = AlertTypeList.Find(m => m.AlertTypeId == id);
                    if (AlertType == null || AlertType.AlertTypeId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertTypeObj());
                    }

                    Session["_CurrentSelAlertType_"] = AlertType;

                    AlertType.StatusVal = AlertType.Status == 1;
                    return View(AlertType);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertTypeObj());
                }
            }
            public JsonResult ProcessEditAlertTypeRequest(AlertTypeObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selAlertType = Session["_CurrentSelAlertType_"] as AlertTypeObj;
                    if (selAlertType == null || selAlertType.AlertTypeId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.AlertTypeId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.Title) || model.Title.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertType Name" });
                    }

                   
                    var passObj = new EditAlertTypeObj
                    {
                        AdminUserId = userData.UserId,
                        Title = model.Title,
                        AlertTypeId = selAlertType.AlertTypeId,
                        Status = model.StatusVal ? 1 : 0,
                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = AlertTypeService.UpdateAlertType(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelAlertType_"] = null;
                    Session["_AlertTypeList_"] = null;
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
                    var previousAlertTypeList = Session["_AlertTypeList_"] as List<AlertTypeObj>;
                    if (previousAlertTypeList == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                    }

                    var thisItem = previousAlertTypeList.Find(m => m.AlertTypeId == id);
                    if (thisItem == null || thisItem.AlertTypeId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                    }

                    var passObj = new DeleteAlertTypeObj
                    {
                        AdminUserId = userData.UserId,
                        AlertTypeId = id
                    };

                    var response = AlertTypeService.DeleteAlertType(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }

                    Session["_AlertTypeList_"] = null;
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