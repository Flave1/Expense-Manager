using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APIObjs.Business;
using PluglexHelper.CoreService;
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;


namespace PlugLitePortal.Controllers.Setup
{
    public class WorkflowSettingManagerController : Controller
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
                var ProductItemId =  userClientSession.ProductItemId;

                #endregion

                #region Current User session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<WorkflowSettingObj>());
                }

                #endregion

                #region Check if workflow list if null else return to view

                if (Session["_WorkflowSettingList_"] is List<WorkflowSettingObj> WorkflowSetting && WorkflowSetting.Any())
                {
                    return View(WorkflowSetting.Where(m => m.ClientId == ClientId && m.ProductId == ProductId ).ToList());
                }


                #endregion

                #region Request and response validations

                var searchObj = new WorkflowSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowSettingId = 0,
                    Status = -2
                };
                var retVal = WorkflowSettingServices.LoadWorkflowSettings(searchObj, userData.Username);
                if (retVal?.Status == null)
                {
                    ViewBag.Error = " WorkflowSetting list is empty!";
                    return View(new List<WorkflowSettingObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  WorkflowSetting list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<WorkflowSettingObj>());
                }
                if (retVal.WorkflowSettings == null || !retVal.WorkflowSettings.Any())
                {
                    ViewBag.Error = " WorkflowSetting list is empty!";
                    return View(new List<WorkflowSettingObj>());
                }

                #endregion

                #region Initialize list into session

                var WorkflowSettings = retVal.WorkflowSettings.OrderBy(m => m.WorkflowSettingId).ToList();

                Session["_WorkflowSettingList_"] = WorkflowSettings.Where(m => m.ClientId == ClientId && m.ProductId == ProductId && m.ProductItemId == ProductItemId).ToList();


                #endregion

                var clientProdWorkflowSettings = WorkflowSettings.Where(m => m.ClientId == ClientId && m.ProductId == ProductId  ).ToList();
                return View(clientProdWorkflowSettings);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List<WorkflowSettingObj>());
            }
        }

        #region Add WorkflowSetting
        public ActionResult AddWorkflowSetting(int clientId, int productId)
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
                    return View(new RegWorkflowSettingObj());
                }

                return View(new RegWorkflowSettingObj { ProductId = productId, ClientId = clientId, ProductItemId =userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegWorkflowSettingObj());
            }
        }
        public JsonResult ProcessAddWorkflowSettingRequest(RegWorkflowSettingObj model)
        {
            try
            {
                #region current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }


                #endregion

                #region Model validation checks

                if (model == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, issuccessful = false, isreload = false, Error = "client required " });
                }

                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, issuccessful = false, isreload = false, Error = "Product Item required " });
                }

                if (model.ProductId < 1)
                {
                    return Json(new { IsAuthenticated = true, issuccessful = false, isreload = false, Error = "Product required " });
                }
                if (model.RequestType < 1)
                {
                    return Json(new { IsAuthenticated = true, issuccessful = false, isreload = false, Error = "Request Type required " });
                }

                if (string.IsNullOrEmpty(model.ApprovalWorkflow))
                {
                    return Json(new { IsAuthenticated = true, issuccessful = false, isreload = false, Error = "Please Ok Button" });
                }

                #endregion

                #region Check if item already exist

                var previousWorkflowSettingList = (List<WorkflowSettingObj>)Session["_WorkflowSettingList_"];
                if (previousWorkflowSettingList != null)
                {
                    if (previousWorkflowSettingList.Count(x => x.RequestType == model.RequestType
                                                               && x.ClientId == model.ClientId
                                                               && x.ProductId == model.ProductId
                                                               && x.ProductItemId == model.ProductItemId) > 0)
                        return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Expense Lookup  Already Exist!" });
                }

                #endregion

                #region Build Request object

                var requestObj = new RegWorkflowSettingObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    RegisteredBy = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    RequestType = model.RequestType,
                    TimeStampRegiestered = DateTime.Now.ToString("yy-MMM-dd ddd"),
                    ApprovalWorkflow = model.ApprovalWorkflow,
                    Status = model.StatusVal ? 1 : 0
                };

                #endregion

                #region Request and response validations

                var response = WorkflowSettingServices.AddWorkflowSetting(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }

                var searchObj = new WorkflowSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowSettingId = 0,
                    Status = -2
                };

                var retVal = WorkflowSettingServices.LoadWorkflowSettings(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.WorkflowSettings != null)
                {
                    var WorkflowSettings = retVal.WorkflowSettings.OrderBy(m => m.WorkflowSettingId).ToList();
                    Session["_WorkflowSettingList_"] = WorkflowSettings.Where(m => m.ClientId == model.ClientId
                                                        && m.ProductId == model.ProductId
                                                        && m.ProductItemId == model.ProductItemId);
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

        #region WorkflowSetting Detail
        public ActionResult _WorkflowSettingDetail(int WorkflowSettingId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new WorkflowSettingObj());
                }

                if (WorkflowSettingId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new WorkflowSettingObj());
                }

                if (!(Session["_WorkflowSettingList_"] is List<WorkflowSettingObj> WorkflowSettings) || WorkflowSettings.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new WorkflowSettingObj());
                }

                var thisWorkflowSetting = WorkflowSettings.Find(m => m.WorkflowSettingId == WorkflowSettingId);
                if (thisWorkflowSetting == null || thisWorkflowSetting.WorkflowSettingId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new WorkflowSettingObj());
                }

                return View(thisWorkflowSetting);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new WorkflowSettingObj());
            }
        }
        #endregion

        #region Edit WorkflowSetting

        public ActionResult EditWorkflowSetting(int WorkflowSettingId)
        {
            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View("EditWorkflowSetting", new WorkflowSettingObj());
                }
                if (WorkflowSettingId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View("EditWorkflowSetting", new WorkflowSettingObj());
                }

                if (!(Session["_WorkflowSettingList_"] is List<WorkflowSettingObj> WorkflowSettingList) || WorkflowSettingList.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View("EditWorkflowSetting", new WorkflowSettingObj());
                }

                var WorkflowSetting = WorkflowSettingList.Find(m => m.WorkflowSettingId == WorkflowSettingId);
                if (WorkflowSetting == null || WorkflowSetting.WorkflowSettingId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View("EditWorkflowSetting", new WorkflowSettingObj());
                }

                Session["_CurrentSelWorkflowSetting_"] = WorkflowSetting;

                WorkflowSetting.StatusVal = WorkflowSetting.Status == 1 ? true : false;
                return View(WorkflowSetting);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View("EditWorkflowSetting", new WorkflowSettingObj());
            }
        }

        public JsonResult ProcessEditWorkflowSettingRequest(WorkflowSettingObj model)
        {
            try
            {
                #region Current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();

                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region Check if Settings list is null else return to view

                var selWorkflowSetting = Session["_CurrentSelWorkflowSetting_"] as WorkflowSettingObj;
                if (selWorkflowSetting == null || selWorkflowSetting.WorkflowSettingId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }

                #endregion

                #region model validations 


                if (model.ClientId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Client required " });
                }
                if (model.WorkflowSettingId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "WorkflowSettingId required" });
                }
                if (model.RequestType < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Request Type required" });
                }
                if (model.ProductItemId < 1)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "ProductItemId required" });
                }
                if (string.IsNullOrEmpty(model.ApprovalWorkflow))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Approval Workflow required" });
                }
                if (!GenericVal.Validate(model, out var msg))
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = $"Validation Error Occurred! Detail: {msg}" });
                }

                #endregion

                #region Build Request object

                var passObj = new EditWorkflowSettingObj()
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    RegisteredBy = userData.UserId,
                    ProductItemId = model.ProductItemId,
                    RequestType = model.RequestType,
                    TimeStampRegiestered = DateTime.Now.ToString("yy-MMM-dd ddd"),
                    ApprovalWorkflow = model.ApprovalWorkflow,
                    Status = model.StatusVal ? 1 : 0,
                    WorkflowSettingId = model.WorkflowSettingId,
                };


                #endregion

                #region Request and response validations

                var response = WorkflowSettingServices.UpdateWorkflowSetting(passObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add course of study" : response.Status.Message.TechnicalMessage });
                }

                Session["_CurrentSelWorkflowSetting_"] = null;

                var searchObj = new WorkflowSettingSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowSettingId = 0,
                    Status = -2
                };

                var retVal = WorkflowSettingServices.LoadWorkflowSettings(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.WorkflowSettings != null)
                {
                    var WorkflowSettings = retVal.WorkflowSettings.OrderBy(m => m.WorkflowSettingId).ToList();
                    Session["_WorkflowSettingList_"] = WorkflowSettings.Where(m => m.ClientId == model.ClientId 
                                                        &&m.ProductId == model.ProductId
                                                        &&m.ProductItemId == model.ProductItemId);
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