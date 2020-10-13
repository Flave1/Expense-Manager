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
    public class SMSProviderManagerController : Controller
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
                    return View(new List<SMSProviderObj>());
                }

                if (Session["_SMSProviderList_"] is List<SMSProviderObj> mylist && mylist.Any())
                {
                    return View(mylist);
                }

                var searchObj = new SMSProviderSearchObj
                {
                    AdminUserId = userData.UserId,
                    SMSProviderId = 0,
                    Status = 0
                };

                var retVal = SMSProviderService.LoadSMSProviders(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = "SMSProvider list is empty!";
                    return View(new List<SMSProviderObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? " SMSProvider list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<SMSProviderObj>());
                }
                if (retVal.SMSProviders == null || !retVal.SMSProviders.Any())
                {
                    ViewBag.Error = "SMSProvider list is empty!";
                    return View(new List<SMSProviderObj>());
                }

                //Order By SMSProvider Name 
                var SMSProviders = retVal.SMSProviders.OrderBy(m => m.Name).ToList();
                Session["_SMSProviderList_"] = SMSProviders;
                return View(SMSProviders);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<SMSProviderObj>());
            }
        }


        #region SMSProvider Detail
            public ActionResult _SMSProviderDetail(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new SMSProviderObj());
                    }

                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new SMSProviderObj());
                    }

                    if (!(Session["_SMSProviderList_"] is List<SMSProviderObj> SMSProviders) || SMSProviders.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSProviderObj());
                    }

                    var thisSMSProvider = SMSProviders.Find(m => m.SMSProviderId == id);
                    if (thisSMSProvider == null || thisSMSProvider.SMSProviderId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSProviderObj());
                    }

                    return View(thisSMSProvider);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new SMSProviderObj());
                }
            }
        #endregion
        #region Add SMSProvider
            public ActionResult _AddSMSProvider()
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new RegSMSProviderObj());
                    }

                    return View(new RegSMSProviderObj());
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new RegSMSProviderObj());
                }
            }
            public JsonResult ProcessAddSMSProviderRequest(RegSMSProviderObj model)
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
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid SMS Provider Name" });
                    }
                   
                    model.AdminUserId = userData.UserId;
                    model.Status = model.StatusVal ? 1 : 0;

                    if (Session["_SMSProviderList_"] is List<SMSProviderObj> previousSMSProviderList)
                    {
                        if (previousSMSProviderList.Count(x => x.Name.ToLower().Trim().ToStandardHash() == model.Name.ToLower().Trim().ToStandardHash()) > 0)
                            return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "SMS Provider Information  Already Exist!" });
                    }


                    var response = SMSProviderService.AddSMSProvider(model, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_SMSProviderList_"] = null;
                    return Json(new { IsAuthenticated = true, IsSuccessful = true, IsReload = false, Error = "" });
                }
                catch (Exception ex)
                {
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Process Error Occurred! Please try again later" });
                }
            }
        #endregion
        
        #region Edit SMSProvider
            public ActionResult _EditSMSProvider(int id)
            {
                try
                {
                    ViewBag.Error = "";
                    ViewBag.SessionError = "";
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                    if (userData.UserId < 1)
                    {
                        ViewBag.SessionError = "Your session has expired! Please re-login";
                        return View(new SMSProviderObj());
                    }
                    if (id < 1)
                    {
                        ViewBag.Error = "Invalid selection";
                        return View(new SMSProviderObj());
                    }

                    if (!(Session["_SMSProviderList_"] is List<SMSProviderObj> SMSProviderList) || SMSProviderList.Count < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSProviderObj());
                    }

                    var SMSProvider = SMSProviderList.Find(m => m.SMSProviderId == id);
                    if (SMSProvider == null || SMSProvider.SMSProviderId < 1)
                    {
                        ViewBag.Error = "Error Occurred! Unable to process selected item";
                        return View(new SMSProviderObj());
                    }

                    Session["_CurrentSelSMSProvider_"] = SMSProvider;

                    SMSProvider.StatusVal = SMSProvider.Status == 1;
                    return View(SMSProvider);
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                    return View(new SMSProviderObj());
                }
            }
            public JsonResult ProcessEditSMSProviderRequest(SMSProviderObj model)
            {
                try
                {
                    var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                    if (userData.UserId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    var selSMSProvider = Session["_CurrentSelSMSProvider_"] as SMSProviderObj;
                    if (selSMSProvider == null || selSMSProvider.SMSProviderId < 1)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                    }

                    if (model.SMSProviderId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid Selection" });
                    }

                    if (string.IsNullOrEmpty(model.Name) || model.Name.Length < 2)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Invalid SMSProvider Name" });
                    }
                  
                    var passObj = new EditSMSProviderObj
                    {
                        AdminUserId = userData.UserId,
                        Name = model.Name,
                        SMSProviderId = selSMSProvider.SMSProviderId,
                        Status = model.StatusVal ? 1 : 0,

                    };

                    if (!GenericVal.Validate(model, out var msg))
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                    }


                    var response = SMSProviderService.UpdateSMSProvider(passObj, userData.Username);
                    if (response?.Status == null)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                    }

                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                    }

                    Session["_CurrentSelSMSProvider_"] = null;
                    Session["_SMSProviderList_"] = null;
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
                    var previousSMSProviderList = Session["_SMSProviderList_"] as List<SMSProviderObj>;
                    if (previousSMSProviderList == null)
                    {
                        return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = true });
                    }

                    var thisItem = previousSMSProviderList.Find(m => m.SMSProviderId == id);
                    if (thisItem == null || thisItem.SMSProviderId < 1)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = "Selected item does not exist in the collection" });
                    }

                    var passObj = new DeleteSMSProviderObj
                    {
                        AdminUserId = userData.UserId,
                        SMSProviderId = id
                    };

                    var response = SMSProviderService.DeleteSMSProvider(passObj, User.Identity.Name);
                    if (!response.Status.IsSuccessful)
                    {
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, Error = response.Status.Message.FriendlyMessage });
                    }

                    Session["_SMSProviderList_"] = null;
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