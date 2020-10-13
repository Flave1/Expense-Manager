using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using GlobalLitePortal.PortalCore.Model;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APIObjs.Business;
using PluglexHelper.CoreService; 
using PlugLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using XPLUG.WEBTOOLKIT;
 
namespace PlugLitePortal.Controllers.Setup
{
    public class WorkflowTaskManagerController : Controller
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

                #region Current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name);
                if (userData == null || userData.UserId < 1)
                {
                    ViewBag.Error = "Session Has Expired! Please Re-Login";
                    return View(new List<WorkflowTaskObj>());
                }


                #endregion
 
                #region get the pending and updated tasks from workflow task endpoint

                
                var searchObj = new WorkflowTaskSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowTaskId = 0,
                    Status = -2
                };
                var retVal = WorkflowTaskServices.LoadWorkflowTasks(searchObj, userData.Username);

                #endregion

                #region get some requisition properties from endpoint or session

                if (Session["_AExpenseRequisitionList_"] is List<ExpenseRequisitionObj> ExpenseRequisitionList && ExpenseRequisitionList.Any())
                {
                    Session["_AExpenseRequisitionList_"] = ExpenseRequisitionList.Where(s => s.ClientId == ClientId && s.ProductId == ProductId).ToList();
                }
                else  
                {
                    var searchObjReq = new ExpenseRequisitionSearchObj
                    {
                        AdminUserId = userData.UserId,
                        ExpenseRequisitionId = 0,
                        Status = -1000
                    };
                    var searchObjForReqs = ExpenseLookupServices.LoadExpenseRequisitions(searchObjReq, userData.Username).ExpenseRequisitions.Where(s=>s.ClientId == ClientId && s.ProductId == ProductId);
                    var reqitems = searchObjForReqs.OrderBy(m => m.ExpenseRequisitionId).ToList();
                    Session["_AExpenseRequisitionList_"] = reqitems.ToList();
                }

                #endregion

                #region response validations

                if (retVal?.Status == null)
                {
                    ViewBag.Error = " WorkflowTask list is empty!";
                    return View(new List<WorkflowTaskObj>());
                }

                if (!retVal.Status.IsSuccessful)
                {
                    ViewBag.Error = string.IsNullOrEmpty(retVal.Status.Message.FriendlyMessage)
                        ? "  WorkflowTask list is empty!"
                        : retVal.Status.Message.FriendlyMessage;
                    return View(new List<WorkflowTaskObj>());
                }
                if (retVal.WorkflowTasks == null || !retVal.WorkflowTasks.Any())
                {
                    ViewBag.Error = " WorkflowTask list is empty!";
                    return View(new List<WorkflowTaskObj>());
                }

                #endregion
                               
                #region filtering task list

                var WorkflowTasks = retVal.WorkflowTasks.OrderBy(m => m.WorkflowTaskId).Where(m => m.Status == (int)TaskStatus.Pending || m.Status == (int)TaskStatus.Viewed);
                var taskBasedOnClientProd = WorkflowTasks.Where(m => m.ClientId == ClientId && m.ProductId == ProductId);
                var taskBasedOnAssignee =  taskBasedOnClientProd.Where(x => x.AssigneeId == userData.UserId).ToList();

                #endregion

                return View(taskBasedOnAssignee);
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new List< WorkflowTaskObj>());
            }
        }

        #region Add WorkflowTask
        public ActionResult AddWorkflowTask(int clientId, int productId)
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
                    return View(new RegWorkflowTaskObj());
                }

                return View(new RegWorkflowTaskObj { ProductId = productId, ClientId = clientId, ProductItemId = userClientSession.ProductItemId });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegWorkflowTaskObj());
            }
        }
        public JsonResult ProcessAddWorkflowTaskRequest(RegWorkflowTaskObj model)
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

                #region model validations

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
                if (string.IsNullOrEmpty(model.AssigneeEmail) || model.AssigneeEmail.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Email required" });
                }

                if (string.IsNullOrEmpty(model.DueDate) || model.DueDate.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "DueDate is required" });
                }
                if (string.IsNullOrEmpty(model.DueTime) || model.DueTime.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "DueTime is required" });
                }
                if (string.IsNullOrEmpty(model.StartDate) || model.StartDate.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "StartDate is required" });
                }
                if (string.IsNullOrEmpty(model.TaskDescription) || model.TaskDescription.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "TaskDescription is required" });
                }
                if (string.IsNullOrEmpty(model.TaskTitle) || model.TaskTitle.Length < 0)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "TaskDescription is required" });
                }

                #endregion

                var previousWorkflowTaskList =  (List<WorkflowTaskObj>)Session["_WorkflowTaskList_"];

                #region build request object

                var requestObj = new RegWorkflowTaskObj
                {
                    ClientId = model.ClientId,
                    ProductId = model.ProductId,
                    AdminUserId = userData.UserId,
                    Status = 1,
                    ProductItemId = model.ProductItemId,
                    ApprovalLevelId = model.ApprovalLevelId,
                    AssigneeEmail = model.AssigneeEmail,
                    AssigneeId = model.AssigneeId,
                    DueDate = model.DueDate,
                    DueTime = model.DueTime,
                    ExpenseRequisitionId = model.ExpenseRequisitionId,
                    RequestType = model.RequestType,
                    IsEmailSent = model.IsEmailSent,
                    StartDate = model.StartDate,
                    StartTime = model.StartTime,
                    TaskDescription = model.TaskDescription,
                    TaskTitle = model.TaskTitle,
                    TimeStampCreated = DateTime.Now.ToString("D"),
                    WorkflowSettingId = model.WorkflowSettingId,
                };

                #endregion

                #region request and response validations

                var response = WorkflowTaskServices.AddWorkflowTask(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
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

        #region WorkflowComment List
        public PartialViewResult RecommendationList(int ExpenseRequisitionId)
        {

            try
            {
                ViewBag.Error = "";
                ViewBag.SessionError = "";
                #region current user session check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return PartialView(new List<WorkflowDetailObj>());
                }

                #endregion

                #region Id check
                if (ExpenseRequisitionId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return PartialView(new List<WorkflowDetailObj>());
                }
                #endregion

                #region get approval work flow level name from session or service if session is empty

                if (Session["_ApprovalLevels_"] is List<WorkFlowApprovalLevelObj> WorkFlowApprovalLevelList && WorkFlowApprovalLevelList.Any())
                {
                    Session["_ApprovalLevels_"] = WorkFlowApprovalLevelList.ToList();
                }
                else
                {
                    var reqObjAppLvel = new WorkFlowApprovalLevelSearchObj
                    {
                        AdminUserId = userData.UserId,
                        StartDate = " ",
                        Status = -2,
                        StopDate = "",
                        WorkFlowApprovalLevelId = 0,
                    };
                    var retValForAppLv = ExpenseLookUpServices.LoadWorkFlowApprovalLevels(reqObjAppLvel, userData.Username);
                    var appLevels = retValForAppLv.WorkflowApprovalLevels.ToList();
                    Session["_ApprovalLevels_"] = appLevels;
                }

                #endregion

                #region return Approval levels comment if session is not null
                if (Session["_WorkflowComments_"] is List<WorkflowDetailObj> WorkflowDetailList && WorkflowDetailList.Any())
                {
                    return PartialView(WorkflowDetailList.Where(m => m.ExpenseRequisitionId == ExpenseRequisitionId && m.Status != (int)WorkflowStatus.Processing).ToList());
                }
                #endregion

                #region requests  for approval level comments and response  check
                var reqObj = new WorkflowDetailSearchObj
                {
                    AdminUserId = userData.UserId,
                    StartDate = " ",
                    Status = -2,
                    StopDate = "",
                    WorkflowDetailId = 0,
                };
                var retVal = WorkflowDetailServices.LoadWorkflowDetails(reqObj, userData.Username);

                var thisWorkflowComments = retVal.WorkflowDetails.Where(m => m.ExpenseRequisitionId == ExpenseRequisitionId && m.Status != (int)WorkflowStatus.Processing).ToList();
                if (thisWorkflowComments == null || !thisWorkflowComments.Any())
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return PartialView(new List<WorkflowDetailObj>());
                }
                Session["_WorkflowComments_"] = thisWorkflowComments;

                #endregion

                return PartialView(thisWorkflowComments);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return PartialView(new List<WorkflowDetailObj>());
            }
        }
        #endregion


    }
}