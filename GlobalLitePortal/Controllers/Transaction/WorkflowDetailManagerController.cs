using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GlobalLitePortal;
using PluglexHelper.CoreService; 
using System.Collections.Generic;
using PlugLitePortalHelper.APIObjs;
using GlobalLitePortalHelper.APICore;
using GlobalLitePortalHelper.APIObjs;
using PlugLitePortalHelper.APIServices;
using GlobalLitePortal.PortalCore.Model;
using XPLUG.WEBTOOLKIT;
using PluglexHelper.PortalObjs;
using PluglexHelper.PortalManager;
using GlobalLitePortalHelper.APIObjs.Business;

namespace PlugLitePortal.Controllers.Setup
{
    public class WorkflowDetailManagerController : Controller
    {
         
        #region Add WorkflowDetail
        public ActionResult AddWorkflowDetail(int WorkflowTaskId)
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

                #region current user check

                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    ViewBag.SessionError = "Your session has expired! Please re-login";
                    return View(new RegWorkflowDetailObj());
                }

                #endregion

                #region current Id check

                if (WorkflowTaskId < 1)
                {
                    ViewBag.Error = "Invalid selection";
                    return View(new RegWorkflowDetailObj());
                }

                #endregion

                #region get current workflow task from service

                var searchObj = new WorkflowTaskSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowTaskId = 0,
                    Status = -1000
                };
                var retVal = WorkflowTaskServices.LoadWorkflowTasks(searchObj, userData.Username);
                var WorkflowTaskList = retVal.WorkflowTasks.ToList();

                var assigneeflowTask = WorkflowTaskList.Find(m => m.WorkflowTaskId == WorkflowTaskId);
                if (assigneeflowTask == null || assigneeflowTask.WorkflowTaskId < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RegWorkflowDetailObj());
                }
                Session["_CurrentSelWorkflowTask_"] = assigneeflowTask;

                #endregion

                #region extract app users name


                var searchObj2 = new UserSearchObj
                {
                    AdminUserId = userData.UserId,
                    Status = -2,
                    StopDate = "",
                    StartDate = "",
                    UserId = 0,
                };

                var retValForUsers = new PortalUserManager().LoadUsers(searchObj2, userData.Username);
                var userList = retValForUsers.Users.ToList();
                Session["_UserList_"] = userList;

                #endregion
                                
                #region get requisitions and extract beneficiaryId and general remark for this Task

                var searchObjForReq = new ExpenseRequisitionSearchObj
                {
                    AdminUserId = userData.UserId,
                    ExpenseRequisitionId = 0,
                    Status = -1000
                };
                var retValForReq = ExpenseLookupServices.LoadExpenseRequisitions(searchObjForReq, userData.Username);
                if (retValForReq?.Status == null)
                {
                    ViewBag.Error = "Error Occurred! Please try again later";
                    return View(new RegWorkflowDetailObj());
                }
                Session["_ExpenseRequisitionList_"] = retValForReq.ExpenseRequisitions.ToList();
                var currentReq = retValForReq.ExpenseRequisitions.FirstOrDefault(m => m.ExpenseRequisitionId == assigneeflowTask.ExpenseRequisitionId);
                if (currentReq == null)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RegWorkflowDetailObj());
                }
                var BeneficiaryId = currentReq.ExpenseRequisitionItems.FirstOrDefault().BeneficiaryId;

                #endregion

                #region get beneficiary from service and extract benficiary name using beneficiary id from requisitions

                var searchObjForBene = new BeneficiarySearchObj
                {
                    AdminUserId = userData.UserId,
                    BeneficiaryId = 0,
                    Status = -1000
                };
                var retValForBene = BeneficiaryServices.LoadBeneficiaries(searchObjForBene, userData.Username);

                if (retValForBene.Beneficiaries.Count < 1)
                {
                    ViewBag.Error = "Error Occurred! Unable to process selected item";
                    return View(new RegWorkflowDetailObj());
                }
                var beneficiaryName = retValForBene.Beneficiaries.FirstOrDefault(m => m.BeneficiaryId == BeneficiaryId);
                Session["_BeneficiaryList_"] = retValForBene.Beneficiaries.ToList();

                #endregion

                return View(new RegWorkflowDetailObj
                { 
                    GeeneralRemark = currentReq.GeneralRemark,
                    BeneficiaryName = beneficiaryName.FirstName+" "+ beneficiaryName.LastName
                }); 
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Error Occurred! Please try again later";
                UtilTools.LogE(ex.StackTrace, ex.Source, ex.Message);
                return View(new RegWorkflowDetailObj());
            }
        }
        public JsonResult ProcessAddWorkflowDetailRequest(RegWorkflowDetailObj model)
        {
            try
            {
                #region model validations 
                var userData = MvcApplication.GetUserData(User.Identity.Name) ?? new UserData();
                if (userData.UserId < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }
                if (model.Status < 1)
                {
                    return Json(new { IsSuccessful = false, Error = "Task Status required to save changes", IsAuthenticated = true });
                }
                if (model.Recommendation.Length < 5)
                {
                    return Json(new { IsSuccessful = false, Error = "Minimium of five characters for recommendation required", IsAuthenticated = true });
                }

                if (string.IsNullOrEmpty(model.Recommendation) || model.Recommendation.Length < 1 || model.Recommendation.Length == 0)
                {
                    return Json(new { IsSuccessful = false, Error = "Recommendation field required to save changes", IsAuthenticated = true });
                }
                #endregion

                #region current task session check
                var currentTask = (WorkflowTaskObj)Session["_CurrentSelWorkflowTask_"];
                if (currentTask == null)
                {
                    return Json(new { IsSuccessful = false, Error = "Your session has expired", IsAuthenticated = false });
                }
                #endregion

                #region build request object

                var requestObj = new RegWorkflowDetailObj
                {
                    ClientId = currentTask.ClientId,
                    ProductId = currentTask.ProductId,
                    AdminUserId = userData.UserId,
                    ApprovalLevelId = currentTask.ApprovalLevelId,
                    AssigneeId = currentTask.AssigneeId,
                    ExpenseRequisitionId = currentTask.ExpenseRequisitionId,
                    IsEmailSent = currentTask.IsEmailSent,
                    Recommendation = model.Recommendation.Trim(),
                    RequestType = currentTask.RequestType,
                    TaskTitle = currentTask.TaskTitle,
                    TimeStampRegistered = DateTime.Now.ToString("dddd-mmmm-yyyy"),
                    WorkflowTaskId = currentTask.WorkflowTaskId,
                    Status = model.Status,
                };

                #endregion

                #region Request and response validations

                var response = WorkflowDetailServices.AddWorkflowDetail(requestObj, userData.Username);
                if (response?.Status == null)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = "Error Occurred! Please try again later" });
                }

                if (!response.Status.IsSuccessful)
                {
                    return Json(new { IsAuthenticated = true, IsSuccessful = false, IsReload = false, Error = string.IsNullOrEmpty(response.Status.Message.TechnicalMessage) ? "Process Failed! Unable to add nomination Source" : response.Status.Message.TechnicalMessage });
                }


                var searchObj = new WorkflowDetailSearchObj
                {
                    AdminUserId = userData.UserId,
                    WorkflowDetailId = 0,
                    Status = -2
                };

                var retVal = WorkflowDetailServices.LoadWorkflowDetails(searchObj, userData.Username);
                if (retVal?.Status != null && retVal.WorkflowDetails != null)
                {
                    var WorkflowDetails = retVal.WorkflowDetails.OrderBy(m => m.WorkflowDetailId).Where(m => m.ClientId == currentTask.ClientId && m.ProductId == currentTask.ProductId).ToList();
                    Session["_WorkflowDetailList_"] = WorkflowDetails;
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