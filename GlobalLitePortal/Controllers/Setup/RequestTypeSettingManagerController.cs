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
    public class RequestTypeSettingManagerController : Controller
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

                #region Current User Session Check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<RequestTypeSetting>());
                }

                #endregion

                #region Check if Request type is null else return to list

                if (Session["_RequestTypeSettingList_"] is List<RequestTypeSetting> RequestTypeSetting && RequestTypeSetting.Any())
                {
                    return View(RequestTypeSetting.Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList());
                }

                #endregion

                #region Request and Response Validations 

                var searchObj = new RequestTypeSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    RequestTypeSettingId = 0,
                    Status = 1,
                    StartDate = "",
                    StopDate = "",
                };
                var retVal = RequestTypeSettingServices.LoadRequestTypeSettings(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " RequestTypeSetting list is empty!";
                    return View(new List<RequestTypeSetting>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  RequestTypeSetting list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<RequestTypeSetting>());
                }
                if (retVal.RequestTypeSettings == null || !retVal.RequestTypeSettings.Any())
                {
                    ViewBag.Error = " RequestTypeSetting list is empty!";
                    return View(new List<RequestTypeSetting>());
                }

                #endregion

                #region Initialize List into Session

                var RequestTypeSettings = retVal.RequestTypeSettings.OrderBy(m => m.RequestTypeSettingId).ToList();
                Session["_RequestTypeSettingList_"] = RequestTypeSettings.Where(m => m.ClientId == ClientId
                                                        && m.ProductId == ProductId 
                                                        && m.ProductItemId == ProductItemId).ToList();


                #endregion

                var clientProdRequestTypeSettings = RequestTypeSettings.Where(m => m.ClientId == ClientId 
                                                    && m.ProductId == ProductId ).ToList();
                return View(clientProdRequestTypeSettings);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<RequestTypeSetting>());
            }
        }

        #region Add RequestTypeSetting
        public ActionResult AddRequestTypeSetting(int clientId, int productId)
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
                    return View(new RegRequestTypeSettingObj());
                }

                return View(new RegRequestTypeSettingObj { ProductId = productId, ClientId = clientId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegRequestTypeSettingObj());
            }
        }
        public JsonResult ProcessAddRequestTypeSettingRequest(RegRequestTypeSettingObj model)
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

                #region Model Validations Check

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

                if (model.RequestType < 1)
                {
                    return Json(new { isauthenticated = true, issuccessful = false, isreload = false, error = "Product required " });
                }

                #endregion

                #region Check if Item Already Exist from Session


                var requestTypeSettingList = (List<RequestTypeSetting>)Session["_RequestTypeSettingList_"];
                if (requestTypeSettingList != null)
                {
                    if (requestTypeSettingList.Any(x => x.RequestType == model.RequestType
                                                               && x.ClientId == model.ClientId
                                                               && x.ProductId == model.ProductId
                                                               && x.ProductItemId == model.ProductItemId))
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Request Type Already Exist!" });
                }

                #endregion

                #region Build Request Object

                var requestObj = new RegRequestTypeSettingObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,
                    AllowedTaskTimeSpan = model.AllowedTaskTimeSpan,//Convert.ToInt32(UtilTools.CurrentTimeStamp()),
                    AmountAllowed = model.AmountAllowed,
                    RequestType = model.RequestType,
                    TimeElapseAction = 1,//Convert.ToInt32(DateTime.Now.Day), //model.TimeElapseAction,
                    TimeStampRegistered = UtilTools.CurrentTimeStamp(),
                    RequestFrequencyType = model.RequestFrequencyType,
                };

                #endregion

                #region Request and response validations checks

                var response = RequestTypeSettingServices.AddRequestTypeSetting(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new RequestTypeSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    RequestTypeSettingId = 0,
                    Status = 1,
                    StartDate = "",
                    StopDate = "",
                };

                var retVal = RequestTypeSettingServices.LoadRequestTypeSettings(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.RequestTypeSettings != null)
                {
                    var RequestTypeSettings = retVal.RequestTypeSettings.OrderBy(m => m.RequestTypeSettingId).ToList();
                    Session["_RequestTypeSettingList_"] = RequestTypeSettings.Where(m => m.ClientId == model.ClientId
                                                    && m.ProductId == model.ProductId
                                                    && m.ProductItemId == model.ProductItemId).ToList(); ;
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
         
        #region RequestTypeSetting Detail
        public ActionResult _RequestTypeSettingDetail(int RequestTypeSettingId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RequestTypeSetting());
                }

                if (RequestTypeSettingId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new RequestTypeSetting());
                }

                if (!(Session["_RequestTypeSettingList_"] is List<RequestTypeSetting> RequestTypeSettings) || RequestTypeSettings.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RequestTypeSetting());
                }

                var thisRequestTypeSetting = RequestTypeSettings.Find(m => m.RequestTypeSettingId == RequestTypeSettingId);
                if (thisRequestTypeSetting == null || thisRequestTypeSetting.RequestTypeSettingId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RequestTypeSetting());
                }

                return View(thisRequestTypeSetting);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RequestTypeSetting());
            }
        }
        #endregion

        #region Edit RequestTypeSetting

        public ActionResult _EditRequestTypeSetting(int RequestTypeSettingId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RequestTypeSetting());
                }
                if (RequestTypeSettingId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new RequestTypeSetting());
                }

                if (!(Session["_RequestTypeSettingList_"] is List<RequestTypeSetting> RequestTypeSettingList) || RequestTypeSettingList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RequestTypeSetting());
                }

                var RequestTypeSetting = RequestTypeSettingList.Find(m => m.RequestTypeSettingId == RequestTypeSettingId);
                if (RequestTypeSetting == null || RequestTypeSetting.RequestTypeSettingId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RequestTypeSetting());
                }

                Session["_CurrentSelRequestTypeSetting_"] = RequestTypeSetting;

               // RequestTypeSetting.StatusVal = RequestTypeSetting.Status == 1;
                return View(RequestTypeSetting);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RequestTypeSetting());
            }
        }
        public JsonResult ProcessEditRequestTypeSettingRequest(RequestTypeSetting model)
        {
            try
            {
                #region Current User session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                var selRequestTypeSetting = Session["_CurrentSelRequestTypeSetting_"] as RequestTypeSetting;

                #region Model validation check

                if (selRequestTypeSetting == null || selRequestTypeSetting.RequestTypeSettingId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Client required " });
                }
                if (model.RequestTypeSettingId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "RequestTypeSettingId required" });
                }
                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }
                #endregion

                #region Check if item already exist from Session

                var requestTypeSettingList = (List<RequestTypeSetting>)Session["_RequestTypeSettingList_"];
                if (requestTypeSettingList != null)
                {
                    if (requestTypeSettingList.Count(x => x.RequestType == model.RequestType
                                                               && x.ClientId == model.ClientId
                                                               && x.ProductId == model.ProductId
                                                               && x.ProductItemId == model.ProductItemId
                                                               && x.RequestTypeSettingId != model.RequestTypeSettingId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Request Type Already Exist!" });
                }


                #endregion

                #region Build request Object

                var requestObj = new EditRequestTypeSettingObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,
                    AllowedTaskTimeSpan = model.AllowedTaskTimeSpan,//Convert.ToInt32(UtilTools.CurrentTimeStamp()),
                    AmountAllowed = model.AmountAllowed,
                    RequestType = model.RequestType,
                    TimeElapseAction = 1,//Convert.ToInt32(DateTime.Now.Day), //model.TimeElapseAction,
                    TimeStampRegistered = UtilTools.CurrentTimeStamp(),
                    RequestFrequencyType = model.RequestFrequencyType,
                    RequestTypeSettingId = model.RequestTypeSettingId,

                };

                #endregion

                #region Request and response validations

                var response = RequestTypeSettingServices.UpdateRequestTypeSetting(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }

                Session["_CurrentSelRequestTypeSetting_"] = null;

                var searchObj = new RequestTypeSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    RequestTypeSettingId = 0,
                    Status = -2
                };
                 

                var retVal = RequestTypeSettingServices.LoadRequestTypeSettings(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.RequestTypeSettings != null)
                {
                    var RequestTypeSettings = retVal.RequestTypeSettings.OrderBy(m => m.RequestTypeSettingId).ToList();
                    Session["_RequestTypeSettingList_"] = RequestTypeSettings.Where(m => m.ClientId == model.ClientId
                                                    && m.ProductId == model.ProductId
                                                    && m.ProductItemId == model.ProductItemId).ToList();
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