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

namespace GlobalLitePortal.Controllers.Business.GeneralLookups
{
    public class AlertNetworkManagerController : Controller
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
                    return View(new List<AlertNetworkObj>());
                }

                if (Session["_AlertNetworkList_"] is List<AlertNetworkObj> mylist && mylist.Any())
                {
                    return View(mylist);
                }

                var searchObj = new AlertNetworkSearchObj
                {
                    AdminUserId = userData.UserId,
                    AlertNetworkId = 0,
                    Status = 0
                };

                var retVal = AlertNetworkService.LoadAlertNetworks(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "AlertNetwork list is empty!";
                    return View(new List<AlertNetworkObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " AlertNetwork list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<AlertNetworkObj>());
                }
                if (retVal.AlertNetworks == null || !retVal.AlertNetworks.Any())
                {
                    ViewBag.Error = "AlertNetwork list is empty!";
                    return View(new List<AlertNetworkObj>());
                }

                //Order By AlertNetwork Name 
                var AlertNetworks = retVal.AlertNetworks.OrderBy(m => m.Name).ToList();
                Session["_AlertNetworkList_"] = AlertNetworks;
                return View(AlertNetworks);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<AlertNetworkObj>());
            }
        }


        #region AlertNetwork Detail
            public ActionResult _AlertNetworkDetail(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertNetworkObj());
                    }

                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertNetworkObj());
                    }

                    if (!(Session["_AlertNetworkList_"] is List<AlertNetworkObj> AlertNetworks) || AlertNetworks.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertNetworkObj());
                    }

                    var thisAlertNetwork = AlertNetworks.Find(m => m.AlertNetworkId == id);
                    if (thisAlertNetwork == null || thisAlertNetwork.AlertNetworkId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertNetworkObj());
                    }

                    return View(thisAlertNetwork);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertNetworkObj());
                }
            }
        #endregion
        #region Add AlertNetwork
            public ActionResult _AddAlertNetwork()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegAlertNetworkObj());
                    }

                    return View(new RegAlertNetworkObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegAlertNetworkObj());
                }
            }
            public JsonResult ProcessAddAlertNetworkRequest(RegAlertNetworkObj model)
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

                    if (string.IsNullOrEmpty(model.OperatorCodes) || model.OperatorCodes.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Alert Short Name" });
                    }

                    model.AdminUserId = userData.UserId;
                    model.Status = model.StatusVal ? 1 : 0;

                    if (Session["_AlertNetworkList_"] is List<AlertNetworkObj> previousAlertNetworkList)
                    {
                        if (previousAlertNetworkList.Count(x => x.Name.ToLower().Trim().ToStandardHash() == model.Name.ToLower().Trim().ToStandardHash()) > 0)
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Alert Item Information  Already Exist!" });
                    }


                    var response = AlertNetworkService.AddAlertNetwork(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_AlertNetworkList_"] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion

        #region Edit AlertNetwork
            public ActionResult _EditAlertNetwork(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new AlertNetworkObj());
                    }
                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new AlertNetworkObj());
                    }

                    if (!(Session["_AlertNetworkList_"] is List<AlertNetworkObj> AlertNetworkList) || AlertNetworkList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertNetworkObj());
                    }

                    var AlertNetwork = AlertNetworkList.Find(m => m.AlertNetworkId == id);
                    if (AlertNetwork == null || AlertNetwork.AlertNetworkId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new AlertNetworkObj());
                    }

                    Session["_CurrentSelAlertNetwork_"] = AlertNetwork;

                    AlertNetwork.StatusVal = AlertNetwork.Status == 1;
                    return View(AlertNetwork);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new AlertNetworkObj());
                }
            }
            public JsonResult ProcessEditAlertNetworkRequest(AlertNetworkObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selAlertNetwork = Session["_CurrentSelAlertNetwork_"] as AlertNetworkObj;
                    if (selAlertNetwork == null || selAlertNetwork.AlertNetworkId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.AlertNetworkId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertNetwork Name" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid AlertNetwork Name" });
                    }

                    var passObj = new EditAlertNetworkObj
                    {
                        AdminUserId = userData.UserId,
                        Name = model.Name,
                        AlertNetworkId = selAlertNetwork.AlertNetworkId,
                        OperatorCodes = model.OperatorCodes,
                        Status = model.StatusVal ? 1 : 0,
                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = AlertNetworkService.UpdateAlertNetwork(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelAlertNetwork_"] = null;
                    Session["_AlertNetworkList_"] = null;
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
                var previousAlertNetworkList = Session["_AlertNetworkList_"] as List<AlertNetworkObj>;
                if (previousAlertNetworkList == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                }

                var thisItem = previousAlertNetworkList.Find(m => m.AlertNetworkId == id);
                if (thisItem == null || thisItem.AlertNetworkId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                }

                var passObj = new DeleteAlertNetworkObj
                {
                    AdminUserId = userData.UserId,
                    AlertNetworkId = id
                };

                var response = AlertNetworkService.DeleteAlertNetwork(passObj, User.Identity.Name);
                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                }

                Session["_AlertNetworkList_"] = null;
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